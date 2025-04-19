using System;
using System.Collections.Generic;
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

using iTunesFetcher.Models;

namespace iTunesFetcher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private LocalTrackListViewModel _localTrackListViewModel = new();
    
    [ObservableProperty] private string _statusMessage = "Готово";
    
    [ObservableProperty] private int? _progressMaximum;
    
    [ObservableProperty] private int? _progressValue;

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
            var selectedFolderPath = folders[0].Path.AbsolutePath;
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
            using (var tagFile = TagLib.File.Create(filePath, ReadStyle.PictureLazy))
            {
                return new TrackViewModel(new LocalTrackModel()
                {
                    FilePath = filePath,
                    
                    Title = tagFile.Tag.Title,
                    Artist = tagFile.Tag.FirstPerformer,
                    Album = tagFile.Tag.Album,
                    Artwork = tagFile.Tag.Pictures.FirstOrDefault()?.Data.Data,
                    
                    Duration = 0,
                });
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

    [RelayCommand]
    private void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    } 

    private async Task LoadTracksAsync(string folderPath)
    {
        await Task.Run(async () =>
        {
            StatusMessage = "Сканирование папки...";
            LocalTrackListViewModel.TrackList.Clear();
            LocalTrackListViewModel.SelectedTrack = null;

            try
            {
                var supportedExtensions = new[] { ".mp3", ".flac", ".m4a", ".ogg", ".wma" };
                var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                    .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()));

                ProgressMaximum = files.Count();
                ProgressValue = 0;

                int count = 0;

                var tasks = new List<Task>();
                var lockObject = new object();

                foreach (var filePath in files)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        var track = LoadTrackInfo(filePath);

                        if (track != null)
                        {
                            lock (lockObject)
                            {
                                LocalTrackListViewModel.TrackList.Add(track);
                                ++count;
                            }
                        }

                        lock (lockObject)
                        {
                            ++ProgressValue;
                            StatusMessage = $"Сканирование папки: {ProgressValue}/{ProgressMaximum}";
                        }
                    }));
                }

                await Task.WhenAll(tasks);
                ProgressValue = 0;

                StatusMessage = $"Найдено треков: {count}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка сканирования: {ex.Message}";
            }
        });
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