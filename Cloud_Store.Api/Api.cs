using Cloud_Store.Models.ViewModels;
using Cloud_Store.Services;

namespace Cloud_Store.Api;

public class Api
{
    private static FileService _fileService = new();
    public static WebApplication CreateApis(WebApplication app)
    {
        // GET File_list
        app.MapGet("/api/file_list", (string path) =>
        {
            HomeViewModel homeViewModel = _fileService.GetFileListAsync(path).Result;
            return Results.Ok(homeViewModel);

        }).WithName("Get Filelist").WithOpenApi();
        
        // GET File
        app.MapGet("/api/file", (string path, string fileName) =>
        {
            Stream fileStream = _fileService.GetFileAsync(path, fileName).Result;
            return Results.File(fileStream, "application/octet-stream", fileName);

        }).WithName("Get File").WithOpenApi();
        
        // POST File
        app.MapPost("/api/file", async (string currentPath, string fileName, Stream fileStream) =>
        {
            await _fileService.WriteFileAsync(currentPath, fileName, fileStream);
            return Results.Ok();

        }).WithName("Post File").WithOpenApi();
        
        // DELETE File
        app.MapDelete("/api/file", (string path, string fileName) =>
        {
            _fileService.DeleteFileAsync(path, fileName).Wait();
            return Results.Ok();

        }).WithName("Delete File").WithOpenApi();
        
        // POST Folder
        app.MapPost("/api/folder", (string currentPath, string folderName) =>
        {
            _fileService.CreateFolderAsync(currentPath, folderName).Wait();
            return Results.Ok();

        }).WithName("Post Folder").WithOpenApi();
        
        

        return app;
    }
}