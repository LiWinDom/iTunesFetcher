using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

using iTunesFetcher.Models;

namespace iTunesFetcher.ViewModels;

public partial class TrackInfoViewModel : ViewModelBase
{
    [ObservableProperty] private string _filePath;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private string? _artist;
    [ObservableProperty] private string? _album;
    [ObservableProperty] private string? _genre;
    [ObservableProperty] private string? _releaseYear;
    [ObservableProperty] private string _duration;
    [ObservableProperty] private string? _trackNumber;
    [ObservableProperty] private string? _diskNumber;
    [ObservableProperty] private string? _trackCount;
    [ObservableProperty] private string? _diskCount;
    [ObservableProperty] private Bitmap? _artwork;
    [ObservableProperty] private string? _lyrics;

    public TrackInfoViewModel(LocalTrackModel track)
    {
        _filePath = track.FilePath;
        _title = track.Title;
        _artist = track.Artist ?? "Неизвестный артист";
        _album = track.Album ?? "Неизвестный альбом";
        _genre = track.Genre;
        _releaseYear = track.ReleaseYear.ToString();
        _duration = track.Duration.ToString();
        _trackNumber = track.TrackNumber.ToString();
        _diskNumber = track.DiscNumber.ToString();
        _trackCount = track.TrackCount.ToString();
        _diskCount = track.DiscCount.ToString();
        
        if (track.Artwork != null)
        {
            var stream = new MemoryStream(track.Artwork);
            using (stream)
            {
                _artwork = Bitmap.DecodeToWidth(stream, 3000);
            }
        }
        
        _lyrics = track.Lyrics;
    }
}