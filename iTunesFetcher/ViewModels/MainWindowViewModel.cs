using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TagLib;

namespace iTunesFetcher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<TrackViewModel> TrackList { get; } = new();

    [ObservableProperty] private TrackViewModel? _selectedTrack;

    [ObservableProperty] private string _statusMessage = "Готово";
    
    [ObservableProperty] private int? _trackCount;
    
    [ObservableProperty] private int? _currentTrack;

    [RelayCommand]
    private async Task OpenFolder()
    {
        var topLevel = GetTopLevel();
        if (topLevel == null)
        {
            StatusMessage = "Не удалось получить доступ к окну";
            return;
        }

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Выберите папку с музыкой",
            AllowMultiple = false,
        });

        if (folders.Count >= 1)
        {
            var selectedFolderPath = folders[0].Path.LocalPath;
            if (!string.IsNullOrEmpty(selectedFolderPath))
            {
                await LoadTracksAsync(selectedFolderPath);
            }
            else
            {
                StatusMessage = "Не удалось получить путь к папке";
            }
        }
    }

    private static TrackViewModel? LoadTrackInfo(string filePath)
    {
        try
        {
            using (var tagFile = TagLib.File.Create(filePath))
            {
                var trackModel = new Models.TrackModel
                {
                    FilePath = filePath,
                    Title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(filePath),
                    Artist = tagFile.Tag.FirstPerformer ?? "Неизвестный артист",
                    Album = tagFile.Tag.Album ?? "Неизвестный альбом",
                    TrackNumber = tagFile.Tag.Track,
                    AlbumArtData = tagFile.Tag.Pictures?.FirstOrDefault()?.Data.Data,
                    Year = tagFile.Tag.Year,
                };
                
                return new TrackViewModel(trackModel);
            }
        }
        catch (CorruptFileException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка чтения файла (поврежден): {filePath} - {ex.Message}");
        }
        catch (UnsupportedFormatException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка чтения файла (формат): {filePath} - {ex.Message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Общая ошибка чтения файла: {filePath} - {ex.Message}");
        }
        return null;
    }

    private async Task LoadTracksAsync(string folderPath)
    {
        StatusMessage = "Сканирование папки...";
        TrackList.Clear();
        SelectedTrack = null;

        try
        {
            var supportedExtensions = new[] { ".mp3", ".flac", ".m4a", ".ogg", ".wma" };
            var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()));

            TrackCount = files.Count();
            CurrentTrack = 0;

            int count = 0;

            var tasks = new List<Task>();
            var lockObject = new object();
            
            foreach (var filePath in files)
            {
                var task = Task.Run(() =>
                {
                    var track = LoadTrackInfo(filePath);

                    if (track != null)
                    {
                        lock (lockObject)
                        {
                            TrackList.Add(track);
                            ++count;
                        }
                    }

                    lock (lockObject)
                    {
                        ++CurrentTrack;
                    }
                });
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
            CurrentTrack = 0;

            StatusMessage = $"Найдено треков: {count}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка сканирования: {ex.Message}";
        }
    }

    private TopLevel? GetTopLevel()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }

        return null;
    }
}