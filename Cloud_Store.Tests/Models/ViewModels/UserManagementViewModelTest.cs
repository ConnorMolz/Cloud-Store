using Cloud_Store.Models.ViewModels;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cloud_Store.Tests.Models.ViewModels;

[TestClass]
[TestSubject(typeof(UserManagementViewModel))]
public class UserManagementViewModelTest
{

    [TestMethod]
    public void ObjectTest()
    {
        UserManagementViewModel userManagementViewModel = new UserManagementViewModel();
        Assert.IsNotNull(userManagementViewModel);
        
        Assert.IsFalse(userManagementViewModel.InEditMode);
        Assert.IsFalse(userManagementViewModel.InCreateMode);
        Assert.IsFalse(userManagementViewModel.OpenModal);
        
        Assert.IsNull(userManagementViewModel.UserAccounts);
        Assert.IsNull(userManagementViewModel.CurrentUser);
        Assert.IsNull(userManagementViewModel.Username);
        Assert.IsNull(userManagementViewModel.Password);
        Assert.IsNull(userManagementViewModel.Role);
        
    }
}