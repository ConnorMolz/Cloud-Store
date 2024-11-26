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
        // Sanitize filename to prevent directory traversal
        fileName = Path.GetFileName(fileName);
    
        string fullPath = EnsureSafePath(Path.Combine(currentPath, fileName));
    
        // Ensure directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
    
        using var destinationStream = File.Create(fullPath);
    
        // Use a larger buffer for big files
        byte[] buffer = new byte[81920]; // 80KB buffer
        int bytesRead;
        while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await destinationStream.WriteAsync(buffer, 0, bytesRead);
        }
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
    
    private string EnsureSafePath(string path)
    {
        // Validate that the path is within the root directory
        string fullPath = Path.GetFullPath(Path.Combine(_rootpath, path));
    
        if (!fullPath.StartsWith(_rootpath))
        {
            throw new UnauthorizedAccessException("Access to the specified path is not allowed.");
        }
    
        return fullPath;
    }
}