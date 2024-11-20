namespace Cloud_Store.Models.ViewModels;

public class HomeViewFormModel
{
    
    public bool ShowFileForm { get; set; }
    public string? NewFiles { get; set; }
    
    public bool ShowNewFolderForm { get; set; }
    public string? NewFolderName { get; set; }
    
    public HomeViewFormModel()
    {
        ShowFileForm = false;
        ShowNewFolderForm = false;
        NewFiles = string.Empty;
        NewFolderName = string.Empty;
    }
}