using Cloud_Store.Services;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cloud_Store.Tests.Services;

[TestClass]
[TestSubject(typeof(HashingService))]
public class HashingServiceTest
{

    [TestMethod]
    public void HashingTest()
    {
        HashingService hashingService = new HashingService();
        string password = "password";
        string hashedPassword = hashingService.HashPassword(password);
        
        Assert.IsNotNull(hashingService);
        Assert.IsNotNull(hashedPassword);
    }
    
    [TestMethod]
    public void HashCheckTest()
    {
        HashingService hashingService = new HashingService();
        string password = "password";
        string hashedPassword = hashingService.HashPassword(password);
        
        Assert.IsNotNull(hashingService);
        Assert.IsTrue(hashingService.VerifyPassword(password, hashedPassword));
    }
}