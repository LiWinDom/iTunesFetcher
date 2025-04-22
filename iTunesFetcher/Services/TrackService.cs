using System;
using System.Linq;
using iTunesFetcher.Models;

namespace iTunesFetcher.Services;

public static class TrackService
{
    public static TrackModelBase? LoadTrackInfo(string filePath)
    {
        try
        {
            var tagFile = TagLib.File.Create(filePath);
            using (tagFile)
            {
                return new LocalTrackModel()
                {
                    FilePath = filePath,
                    
                    Title = tagFile.Tag.Title,
                    Artist = tagFile.Tag.FirstPerformer,
                    Album = tagFile.Tag.Album,
                    
                    Genre = tagFile.Tag.FirstGenre,
                    ReleaseYear = tagFile.Tag.Year,
                    Duration = 0, // TODO
                    TrackNumber = tagFile.Tag.Track,
                    DiscNumber = tagFile.Tag.Disc,
                    TrackCount = tagFile.Tag.TrackCount,
                    DiscCount = tagFile.Tag.DiscCount,
                    Artwork = tagFile.Tag.Pictures.FirstOrDefault()?.Data.Data,
                    Lyrics = tagFile.Tag.Lyrics,
                };
            }
        }
        catch (TagLib.CorruptFileException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка чтения файла (поврежден): {filePath} - {ex.Message}");
        }
        catch (TagLib.UnsupportedFormatException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка чтения файла (формат): {filePath} - {ex.Message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Общая ошибка чтения файла: {filePath} - {ex.Message}");
        }
        return null;
    }
}