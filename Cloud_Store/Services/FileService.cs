using Cloud_Store.Models.ViewModels;

namespace Cloud_Store.Services;
using System.IO;

public interface IFileService
{
    Task WriteFileAsync(string fileName, Stream fileStream);
    Task<FileStream> GetFileAsync(string fileName);
    Task DeleteFileAsync(string fileName);
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
    
    public async Task WriteFileAsync(string fileName, Stream fileStream)
    {
        string fullPath = Path.Combine(_rootpath, fileName);
        using var destinationStream = File.Create(fullPath);
        await fileStream.CopyToAsync(destinationStream);
    }

    public async Task<FileStream> GetFileAsync(string fileName)
    {
        string fullPath = Path.Combine(_rootpath, fileName);
        return File.OpenRead(fullPath);
    }

    public async Task DeleteFileAsync(string fileName)
    {
        string fullPath = Path.Combine(_rootpath, fileName);
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