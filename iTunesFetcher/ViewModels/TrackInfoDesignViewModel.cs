using Avalonia.Media.Imaging;
using Avalonia.Platform;
using iTunesFetcher.Models;

namespace iTunesFetcher.ViewModels;

public class TrackInfoDesignViewModel : TrackInfoViewModel
{
    public TrackInfoDesignViewModel() : base(new LocalTrackModel()
    {
        FilePath = "/Users/LiWinDom/Music/music.mp3",
        Title = "Music Title",
        Artist = "Artist",
        Album = "Album",
        Genre = "Genre",
        ReleaseYear = 2007,
        Duration = 120000,
        TrackNumber = 1,
        DiscNumber = 2,
        TrackCount = 3,
        DiscCount = 4,
        Lyrics = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident",
    })
    {
        using var stream = AssetLoader.Open(new System.Uri("avares://iTunesFetcher/Assets/DefaultArtwork.png"));
        Artwork = Bitmap.DecodeToWidth(stream, 640);
    }
}