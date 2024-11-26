using System.Collections.Generic;
using Cloud_Store.Models.ViewModels;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cloud_Store.Tests.Models.ViewModels;

[TestClass]
[TestSubject(typeof(HomeViewModel))]
public class HomeViewModelTest
{

    [TestMethod]
    public void ObjectTest()
    {
        HomeViewModel homeViewModel = new HomeViewModel();
        Assert.IsNotNull(homeViewModel);
        
        homeViewModel.Files = new List<string>();
        homeViewModel.Folders = new List<string>();
        homeViewModel.CurrentPath = "C:\\";
        homeViewModel.Files.Add("file1.txt");
        homeViewModel.Folders.Add("folder1");
        
        Assert.IsNotNull(homeViewModel.Files);
        Assert.IsNotNull(homeViewModel.Folders);
        Assert.IsNotNull(homeViewModel.CurrentPath);
    }
}