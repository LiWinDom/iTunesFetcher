using System.Text.Json.Serialization;

namespace iTunesFetcher.Models;

public class ITunesTrackModel : TrackModelBase
{
    [JsonPropertyName("trackViewUrl")]
    public required string TrackViewUrl { get; set; }
    
    [JsonPropertyName("previewUrl")]
    public required string PreviewUrl { get; set; }
}