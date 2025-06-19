using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

using iTunesFetcher.Models;

namespace iTunesFetcher.ViewModels;

public partial class TrackItemViewModel : ViewModelBase
{

    [ObservableProperty] private string _title;
    [ObservableProperty] private string _artist;
    [ObservableProperty] private string _album;
    [ObservableProperty] private Bitmap? _artwork;

    public TrackItemViewModel(TrackModelBase track)
    {
        if (track is LocalTrackModel)
        {
            track.Title ??= Path.GetFileNameWithoutExtension(((LocalTrackModel)track).FilePath);
        }
        _title = track.Title ?? "Неизвестное название";
        _artist = track.Artist ?? "Неизвестный артист";
        _album = track.Album ?? "Неизвестный альбом";
        
        if (track.Artwork != null)
        {
            var stream = new MemoryStream(track.Artwork);
            using (stream)
            {
                _artwork = Bitmap.DecodeToWidth(stream, 48 * 2); // Retina scaling?? idk
            }
        }
    }
}