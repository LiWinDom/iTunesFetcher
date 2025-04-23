using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace iTunesFetcher.ViewModels;

public partial class LocalTrackListViewModel : ViewModelBase
{
    public event EventHandler? OnSelectionChanged;
    
    public ObservableCollection<TrackItemViewModel> TrackList { get; } = new();

    private uint? _selectedIndex;
    public uint? SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (_selectedIndex != value)
            {
                _selectedIndex = value;
                OnPropertyChanged();
                OnSelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}