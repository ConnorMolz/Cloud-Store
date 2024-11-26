using System;
using Cloud_Store.Models.ViewModels;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cloud_Store.Tests.Models.ViewModels;

[TestClass]
[TestSubject(typeof(HomeViewFormModel))]
public class HomeViewFormModelTest
{

    [TestMethod]
    public void ObjectTest()
    {
        HomeViewFormModel homeViewFormModel = new HomeViewFormModel();
        Assert.IsNotNull(homeViewFormModel);
        
        Assert.AreEqual(homeViewFormModel.ShowFileForm, false);
        Assert.AreEqual(homeViewFormModel.ShowNewFolderForm, false);
        Assert.AreEqual(homeViewFormModel.NewFiles, Array.Empty<string>());
        Assert.AreEqual(homeViewFormModel.NewFolderName, string.Empty);
        
    }
}