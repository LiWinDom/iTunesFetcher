using CommunityToolkit.Mvvm.ComponentModel;

namespace iTunesFetcher.ViewModels;

public partial class StatusViewModel : ViewModelBase
{
    [ObservableProperty] private string _statusText = "";
    [ObservableProperty] private int _progressBarValue = 0;
    [ObservableProperty] private int _progressBarMaximum = 1;
}