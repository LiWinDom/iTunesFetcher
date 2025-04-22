using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace iTunesFetcher.Services;

public static class FileService
{
    public static List<string> ScanFolder(string folderPath)
    {
        var supportedExtensions = new[] { ".mp3", ".flac", ".m4a", ".ogg", ".wma", ".opus" };
        return Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
            .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant())).ToList();
    }
}
