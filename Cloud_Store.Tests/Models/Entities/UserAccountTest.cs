using Cloud_Store.Models.Entities;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cloud_Store.Tests.Models.Entities;

[TestClass]
[TestSubject(typeof(UserAccount))]
public class UserAccountTest
{

    [TestMethod]
    public void TestObject()
    {
        UserAccount testUserAccount = new UserAccount();
        Assert.IsNotNull(testUserAccount);
        
        testUserAccount.Id = 1;
        testUserAccount.Username = "test";
        testUserAccount.Password = "test";
        testUserAccount.Role = "test";
        
        Assert.AreEqual(testUserAccount.Id, 1);
        Assert.AreEqual(testUserAccount.Username, "test");
        Assert.AreEqual(testUserAccount.Password, "test");
        Assert.AreEqual(testUserAccount.Role, "test");
    }
}