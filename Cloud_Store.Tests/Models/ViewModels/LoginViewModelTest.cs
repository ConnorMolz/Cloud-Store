using Cloud_Store.Models.ViewModels;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cloud_Store.Tests.Models.ViewModels;

[TestClass]
[TestSubject(typeof(LoginViewModel))]
public class LoginViewModelTest
{

    [TestMethod]
    public void ObjectTest()
    {
        LoginViewModel loginViewModel = new();
        Assert.IsNotNull(loginViewModel);
        Assert.IsNull(loginViewModel.Username);
        Assert.IsNull(loginViewModel.Password);
        Assert.IsNull(loginViewModel.ConfirmPassword);
        
        loginViewModel.Username = "username";
        loginViewModel.Password = "password";
        loginViewModel.ConfirmPassword = "password";
        
        Assert.AreEqual("username", loginViewModel.Username);
        Assert.AreEqual("password", loginViewModel.Password);
        Assert.AreEqual("password", loginViewModel.ConfirmPassword);
        
    }
}