using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

using iTunesFetcher.Models;

namespace iTunesFetcher.ViewModels;

public partial class TrackViewModel : ViewModelBase
{

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _artist;

    [ObservableProperty]
    private string _album;

    [ObservableProperty]
    private Bitmap? _artwork;

    public TrackViewModel(LocalTrackModel model)
    {
        _title = model.Title ?? Path.GetFileNameWithoutExtension(model.FilePath);
        _artist = model.Artist ?? "Неизвестный артист";
        _album = model.Album ?? "Неизвестный альбом";
        
        if (model.Artwork != null)
        {
            using (var stream = new MemoryStream(model.Artwork))
            {
                _artwork = Bitmap.DecodeToWidth(stream, 96);
            }
        }
    }
}