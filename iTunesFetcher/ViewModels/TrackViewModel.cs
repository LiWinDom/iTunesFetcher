using System;
using System.Diagnostics;
using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace iTunesFetcher.ViewModels;

public partial class TrackViewModel : ObservableObject
{
    public string FilePath { get; }

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _artist;

    [ObservableProperty]
    private string _album;

    [ObservableProperty]
    private uint _trackNumber;

    [ObservableProperty]
    private Bitmap _artworkData;

    [ObservableProperty]
    private uint _year;

    public TrackViewModel(Models.TrackModel model)
    {
        FilePath = model.FilePath;
        _title = model.Title;
        _artist = model.Artist;
        _album = model.Album;
        _trackNumber = model.TrackNumber;
        
        if (model.AlbumArtData != null)
        {
            using (var stream = new MemoryStream(model.AlbumArtData))
            {
                _artworkData = Bitmap.DecodeToWidth(stream, 96);
            }
        }

        _year = model.Year;
    }

    public void UpdateFromModel(Models.TrackModel model)
    {
        Title = model.Title;
        Artist = model.Artist;
        Album = model.Album;
        TrackNumber = model.TrackNumber;
        if (model.AlbumArtData != null)
        {
            using (var stream = new MemoryStream(model.AlbumArtData))
            {
                ArtworkData = Bitmap.DecodeToWidth(stream, 96);
            }
        }
        Year = model.Year;
    }
}