using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Cloud_Store.Data;
using Cloud_Store.Models.Entities;
using Cloud_Store.Services;

namespace Cloud_Store.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private CloudStoreContext _context;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            // Setup in-memory SQLite database
            var options = new DbContextOptionsBuilder<CloudStoreContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            _context = new CloudStoreContext(options);
            
            // Ensure database is created
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            // Initialize service
            _userService = new UserService(_context);
        }

        [TestMethod]
        public async Task GetUserListAsync_ReturnsEmptyArray_WhenNoUsersExist()
        {
            // Act
            var users = await _userService.GetUserListAsync();

            // Assert
            Assert.AreEqual(0, users.Length);
        }

        [TestMethod]
        public async Task AddUserAsync_AddsNewUser_WhenUsernameIsUnique()
        {
            // Arrange
            var newUser = new UserAccount 
            { 
                Username = "testuser", 
                Password = "password123", 
                Role = "User" 
            };

            // Act
            await _userService.AddUserAsync(newUser);

            // Assert
            var users = await _userService.GetUserListAsync();
            Assert.AreEqual(1, users.Length);
            Assert.AreEqual("testuser", users[0].Username);
        }

        [TestMethod]
        public async Task AddUserAsync_DoesNotAddDuplicateUser()
        {
            // Arrange
            var firstUser = new UserAccount 
            { 
                Username = "testuser", 
                Password = "password123", 
                Role = "User" 
            };
            await _userService.AddUserAsync(firstUser);

            var duplicateUser = new UserAccount 
            { 
                Username = "testuser", 
                Password = "differentpassword", 
                Role = "Admin" 
            };

            // Act
            await _userService.AddUserAsync(duplicateUser);

            // Assert
            var users = await _userService.GetUserListAsync();
            Assert.AreEqual(1, users.Length);
        }

        [TestMethod]
        public async Task GetUserAsync_ReturnsUser_WhenUsernameExists()
        {
            // Arrange
            var newUser = new UserAccount 
            { 
                Username = "testuser", 
                Password = "password123", 
                Role = "User" 
            };
            await _userService.AddUserAsync(newUser);

            // Act
            var retrievedUser = await _userService.GetUserAsync("testuser");

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual("testuser", retrievedUser.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetUserAsync_ThrowsException_WhenUsernameDoesNotExist()
        {
            // Act
            await _userService.GetUserAsync("nonexistentuser");
        }

        [TestMethod]
        public async Task UpdateUserAsync_UpdatesExistingUser()
        {
            // Arrange
            var originalUser = new UserAccount 
            { 
                Username = "testuser", 
                Password = "originalpassword", 
                Role = "User" 
            };
            await _userService.AddUserAsync(originalUser);

            var updatedUser = new UserAccount 
            { 
                Username = "testuser", 
                Password = "newpassword", 
                Role = "Admin" 
            };

            // Act
            await _userService.UpdateUserAsync(updatedUser);

            // Assert
            var retrievedUser = await _userService.GetUserAsync("testuser");
            Assert.AreEqual("newpassword", retrievedUser.Password);
            Assert.AreEqual("Admin", retrievedUser.Role);
        }

        [TestMethod]
        public async Task UpdateUserAsync_DoesNotUpdateNonExistentUser()
        {
            // Arrange
            var nonExistentUser = new UserAccount 
            { 
                Username = "nonexistentuser", 
                Password = "password", 
                Role = "Admin" 
            };

            // Act
            await _userService.UpdateUserAsync(nonExistentUser);

            // Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => 
                _userService.GetUserAsync("nonexistentuser"));
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_context.Database.GetDbConnection().State != System.Data.ConnectionState.Closed)
            {
                _context.Database.CloseConnection();
            }
            _context.Dispose();
        }
    }
}