using Cloud_Store.Models.ViewModels;

namespace Cloud_Store.Services;
using System.IO;

public interface IFileService
{
    Task WriteFileAsync(string currentPath, string fileName, Stream fileStream);
    Task<Stream> GetFileAsync(string path, string fileName);
    Task DeleteFileAsync(string path, string fileName);
    Task CreateFolderAsync(string currentPath, string folderName);
    Task DeleteFolderAsync(string folderName);
    Task<HomeViewModel> GetFileListAsync(string path);
    string RemoveLastFolder(string path);
}

public class FileService : IFileService
{
    private string _rootpath;

    public FileService()
    {
        _rootpath = Directory.GetCurrentDirectory();
        _rootpath = Path.Combine(_rootpath, "Files");
    }
    
    public async Task WriteFileAsync(string currentPath, string fileName, Stream fileStream)
    {
        string fullPath = Path.Combine(currentPath, fileName);
        using var destinationStream = File.Create(fullPath);
        await fileStream.CopyToAsync(destinationStream);
    }

    public async Task<Stream> GetFileAsync(string path, string fileName)
    {
        string fullPath = Path.Combine(path, fileName);
        
        // Ensure the file exists
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"File {fileName} not found in the specified path.");
        }

        // Return a FileStream
        return new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    public async Task DeleteFileAsync(string path, string fileName)
    {
        string fullPath = Path.Combine(path, fileName);
        File.Delete(fullPath);
    }

    public async Task CreateFolderAsync(string currentPath, string folderName)
    {
        string fullPath = Path.Combine(currentPath, folderName);
        Directory.CreateDirectory(fullPath);
    }

    public async Task DeleteFolderAsync(string folderName)
    {
        string fullPath = Path.Combine(_rootpath, folderName);
        Directory.Delete(fullPath, true);
    }

    public Task<HomeViewModel> GetFileListAsync(string path)
    {
        string searchPath;
        if (path == "")
        {
            searchPath = _rootpath;
        }
        else
        {
            searchPath = path;
        }
        string[] files = Directory.GetFiles(searchPath);
        string[] folders = Directory.GetDirectories(searchPath);
        
        for(int i = 0; i < files.Length; i++)
        {
            files[i] = Path.GetFileName(files[i]);
        }
        for(int i = 0; i < folders.Length; i++)
        {
            folders[i] = Path.GetFileName(folders[i]);
        }
        
        return Task.FromResult(new HomeViewModel
        {
            Files = files.ToList(),
            Folders = folders.ToList(),
            CurrentPath = searchPath
        });
    }
    
    public string RemoveLastFolder(string path)
    {
        path = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        return Path.GetDirectoryName(path) ?? path;
    }
}