using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Moq;
using Cloud_Store.Data;
using Cloud_Store.Services;

namespace Cloud_Store.Tests.Services
{
    [TestClass]
    public class AuthServiceTests
    {
        private CloudStoreContext _context;
        private Mock<ProtectedSessionStorage> _mockSessionStorage;
        private Mock<NavigationManager> _mockNavigationManager;
        private AuthService _authService;

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

            // Mock dependencies
            _mockSessionStorage = new Mock<ProtectedSessionStorage>();
            _mockNavigationManager = new Mock<NavigationManager>();

            // Initialize service
            _authService = new AuthService(
                _context, 
                _mockSessionStorage.Object, 
                _mockNavigationManager.Object
            );
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