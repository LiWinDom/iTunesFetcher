using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using iTunesFetcher.Models;
using iTunesFetcher.Services;

namespace iTunesFetcher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        StatusService.StatusTextChanged += (_, text) => StatusViewModel.StatusText = text;
        StatusService.ProgressBarValueChanged += (_, value) => StatusViewModel.ProgressBarValue = value;
        StatusService.ProgressBarMaximumChanged += (_, max) => StatusViewModel.ProgressBarMaximum = max;
        
        MenuBarViewModel.OpenFolderRequested += async (_, _) => await OpenFolder();
        MenuBarViewModel.ExitRequested += (_, _) => Exit();
    }
    
    [ObservableProperty] private MenuBarViewModel _menuBarViewModel = new();
    [ObservableProperty] private StatusViewModel _statusViewModel = new();
    [ObservableProperty] private LocalTrackListViewModel _localTrackListViewModel = new();
    
    private List<string> _trackPaths = new();

    [RelayCommand]
    private async Task OpenFolder()
    {
        var folderPath = await RequestFolder();
        if (folderPath == null)
        {
            return;
        }

        await Task.Run(async () =>
        {
            _trackPaths.Clear();
            LocalTrackListViewModel.TrackList.Clear();
            
            StatusService.StatusText = "Сканирование папки...";
            StatusService.ProgressBarValue = 0;
            var files = FileService.ScanFolder(folderPath);
            StatusService.ProgressBarMaximum = files.Count;
            
            List<Task> tasks = new();
            var lockObject = new object();
            
            int count = 0;
            foreach (var file in files)
            {
                tasks.Add(Task.Run(() =>
                {
                    var track = TrackService.LoadTrackInfo(file);
                    if (track != null)
                    {
                        var viewModel = new TrackViewModel(track);
                        lock (lockObject)
                        {
                            _trackPaths.Add(file);
                            LocalTrackListViewModel.TrackList.Add(viewModel);
                        }
                    }
                    StatusService.StatusText = $"Просканировано файлов: {++count}/{files.Count}";
                    StatusService.ProgressBarValue = count;
                }));
            }
            await Task.WhenAll(tasks);
            StatusService.StatusText = $"Добавлено {LocalTrackListViewModel.TrackList.Count} треков";
            StatusService.ProgressBarValue = 0;
        });
    }
    
    private async Task<string?> RequestFolder()
    {
        TopLevel topLevel = null;
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            topLevel = desktop.MainWindow;
        }
        if (topLevel == null)
        {
            StatusService.StatusText = "Не удалось получить доступ к окну";
            return null;
        }

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Выберите папку с музыкой",
            AllowMultiple = false,
        });

        if (folders.Count == 0)
        {
            return null;
        }
        
        var selectedFolderPath = folders[0].Path.AbsolutePath;
        if (string.IsNullOrEmpty(selectedFolderPath))
        {
            StatusService.StatusText = "Не удалось получить путь к папке";
            return null;
        }
        
        return selectedFolderPath;
    }
    
    private void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    } 
}