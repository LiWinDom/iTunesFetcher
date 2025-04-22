using System;
using CommunityToolkit.Mvvm.Input;

namespace iTunesFetcher.ViewModels;

public partial class MenuBarViewModel : ViewModelBase
{
    public event EventHandler? OpenFolderRequested;
    public event EventHandler? ExitRequested;

    [RelayCommand]
    private void OpenFolder()
    {
        OpenFolderRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Exit()
    {
        ExitRequested?.Invoke(this, EventArgs.Empty);
    }
}