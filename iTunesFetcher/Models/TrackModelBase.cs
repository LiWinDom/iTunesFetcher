using System;
using System.Text.Json.Serialization;

namespace iTunesFetcher.Models;

public abstract class TrackModelBase
{
    [JsonPropertyName("trackName")]
    public string? Title { get; set; }
    
    [JsonPropertyName("artistName")]
    public string? Artist { get; set; }
    
    [JsonPropertyName("collectionName")]
    public string? Album { get; set; }
    
    [JsonPropertyName("primaryGenreName")]
    public string? Genre { get; set; }
    
    [JsonPropertyName("releaseDate")]
    public uint? ReleaseYear { get; set; }
    
    [JsonPropertyName("trackTimeMillis")]
    public required uint Duration { get; set; }
    
    [JsonPropertyName("trackNumber")]
    public uint? TrackNumber { get; set; }
    
    [JsonPropertyName("discNumber")]
    public uint? DiscNumber { get; set; }
    
    [JsonPropertyName("trackCount")]
    public uint? TrackCount { get; set; }
    
    [JsonPropertyName("discCount")]
    public uint? DiscCount { get; set; }
    
    [JsonIgnore]
    public byte[]? Artwork { get; set; }
    
    [JsonIgnore]
    public string? Lyrics { get; set; }
}