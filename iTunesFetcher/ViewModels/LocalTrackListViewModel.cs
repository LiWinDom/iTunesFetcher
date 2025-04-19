using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace iTunesFetcher.ViewModels;

public partial class LocalTrackListViewModel : ViewModelBase
{
    public ObservableCollection<TrackViewModel> TrackList { get; } = new();

    [ObservableProperty] private TrackViewModel? _selectedTrack;
}