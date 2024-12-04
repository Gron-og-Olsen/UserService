/*
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Moq;
using MongoDB.Driver;
using Models;
using UserService.Controllers;
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserService.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IMongoCollection<User>> _mockUserCollection;
        private Mock<ILogger<UserController>> _mockLogger;
        private Mock<IPasswordHasher<User>> _mockPasswordHasher;
        private UserController _controller;

        [TestInitialize]
        public void SetUp()
        {
            // Mock the MongoDB collection
            _mockUserCollection = new Mock<IMongoCollection<User>>();

            // Mock the Logger
            _mockLogger = new Mock<ILogger<UserController>>();

            // Mock the PasswordHasher
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();

            // Instantiate the controller with mocked dependencies
            _controller = new UserController(
                _mockUserCollection.Object,
                _mockLogger.Object,
                _mockPasswordHasher.Object);
        }

        [TestMethod]
        public async Task AddUser_ShouldReturnCreatedResult_WhenUserIsSuccessfullyAdded()
        {
            // Arrange
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "testUser",
                Password = "testPassword"
            };

            _mockPasswordHasher.Setup(p => p.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("hashedPassword");

            // Act
            var result = await _controller.AddUser(newUser);

            // Assert
            var createdResult = result as CreatedAtRouteResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual("GetUserById", createdResult.RouteName);
        }

        [TestMethod]
        public async Task GetUserByUsername_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "nonExistentUser";
            _mockUserCollection.Setup(c => c.Find(It.IsAny<FilterDefinition<User>>()).FirstOrDefaultAsync())
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUserByUsername(username);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual($"User with username {username} not found.", (notFoundResult.Value as dynamic).message);
        }

        [TestMethod]
        public async Task GetUserById_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                Username = "existingUser",
                Password = "hashedPassword"
            };

            _mockUserCollection.Setup(c => c.Find(It.IsAny<FilterDefinition<User>>()).FirstOrDefaultAsync())
                .ReturnsAsync(existingUser);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(existingUser, okResult.Value);
        }

        [TestMethod]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserCollection.Setup(c => c.Find(It.IsAny<FilterDefinition<User>>()).FirstOrDefaultAsync())
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual($"User with ID {userId} not found.", (notFoundResult.Value as dynamic).message);
        }
    }
}
*/