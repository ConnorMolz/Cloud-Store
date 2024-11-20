using Cloud_Store.Models.ViewModels;

namespace Cloud_Store.Services;
using System.IO;

public interface IFileService
{
    Task WriteFileAsync();
    Task<FileStream> GetFileAsync(string path);
    Task DeleteFileAsync(string path);
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
    
    public Task WriteFileAsync()
    {
        throw new NotImplementedException();
    }

    public Task<FileStream> GetFileAsync(string path)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFileAsync(string path)
    {
        throw new NotImplementedException();
    }

    public Task<HomeViewModel> GetFileListAsync(string path)
    {
        Console.WriteLine(_rootpath);
        string searchPath;
        if (path == "")
        {
            searchPath = _rootpath;
        }
        else
        {
            searchPath = path;
        }
        Console.WriteLine(searchPath);
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