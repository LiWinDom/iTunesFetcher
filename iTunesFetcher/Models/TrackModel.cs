namespace iTunesFetcher.Models;

public class TrackModel
{
    public string FilePath { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public uint TrackNumber { get; set; }
    public uint Year { get; set; }
    public byte[]? AlbumArtData { get; set; }
}