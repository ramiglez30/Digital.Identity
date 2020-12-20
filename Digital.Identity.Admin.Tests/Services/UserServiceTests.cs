using AutoMapper;
using Digital.Identity.Admin.AutoMapper;
using Digital.Identity.Admin.Models.Api;
using Digital.Identity.Admin.Models.EF;
using Digital.Identity.Admin.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digital.Identity.Admin.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<ILogger<UserService>> _logger;
        private readonly IMapper _mapper;

        public UserServiceTests()
        {
            _userManager = UserManagerTestHelper.MockUserManager<ApplicationUser>();
            _logger = new Mock<ILogger<UserService>>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        [TestMethod]
        public async Task CreateUserAsync_Returns_InsertedUser()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var createInput = new CreateUserInput { UserName = "username1", Email = "user@email.com", Password = "Qwe123*" };
            var createdUser = new ApplicationUser { Id = guid, UserName = "username1", Email = "user@email.com" };

            _userManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).Returns(() => Task.FromResult(createdUser));

            // Act 
            var userService = new UserService(_userManager.Object, _logger.Object, _mapper);
            var insertedUser = await userService.CreateUserAsync(createInput);

            // Assert
            Assert.AreEqual(createdUser.Id, insertedUser.Id);                        
        }

        [TestMethod]
        public async Task CreateUserAsync_Throws_InvalidOperation_When_Insert_Fails()
        {
            // Arrange
            var createInput = new CreateUserInput { UserName = "username1", Email = "user@email.com", Password = "Qwe123*" };

            _userManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            // Act 
            var userService = new UserService(_userManager.Object, _logger.Object, _mapper);

            // Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async ()=> await userService.CreateUserAsync(createInput));
        }

        [TestMethod]
        public async Task DeleteUserAsync_Returns_True_When_Deleting_User()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();

            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            _userManager.Setup(um => um.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            // Act 
            var userService = new UserService(_userManager.Object, _logger.Object, _mapper);
            var isDeleted = await userService.DeleteUserAsync(guid);

            // Assert
            Assert.AreEqual(true, isDeleted);
        }

        [TestMethod]
        public async Task DeleteUserAsync_Returns_False_When_Deleting_User_Fails()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();

            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            _userManager.Setup(um => um.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed());

            // Act 
            var userService = new UserService(_userManager.Object, _logger.Object, _mapper);
            var isDeleted = await userService.DeleteUserAsync(guid);

            // Assert
            Assert.AreEqual(false, isDeleted);
        }

        [TestMethod]
        public async Task DeleteUserAsync_Throws_KeyNotFoundException()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();

            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).Throws<KeyNotFoundException>();

            // Act 
            var userService = new UserService(_userManager.Object, _logger.Object, _mapper);

            // Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async ()=> await userService.DeleteUserAsync(guid));
        }

        [TestMethod]
        public async Task EditUserAsync_Returns_Edited_User()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var editInput = new EditUserInput { Email = "user@email.com", UserName = "user1" };

            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser { Id= guid, Email = "user@email.com", UserName = "user1" });
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var userService = new UserService(_userManager.Object, _logger.Object, _mapper);
            var result = await userService.EditUserAsync(guid, editInput);

            Assert.AreEqual(editInput.Email, result.Email);
            Assert.AreEqual(guid, result.Id);
        }

        [TestMethod]
        public async Task EditUserAsync_Throws_KeyNotFoundException()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var editInput = new EditUserInput { Email = "user@email.com", UserName = "user1" };

            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(null as ApplicationUser);

            // Act
            var userService = new UserService(_userManager.Object, _logger.Object, _mapper);
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async ()=> await userService.EditUserAsync(guid, editInput));
        }

        [TestMethod]
        public async Task EditUserAsync_Throws_InvalidOperationException()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var editInput = new EditUserInput { Email = "user@email.com", UserName = "user1" };

            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser { Id = guid, Email = "user@email.com", UserName = "user1" });
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed());

            // Act
            var userService = new UserService(_userManager.Object, _logger.Object, _mapper);

            // Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await userService.EditUserAsync(guid, editInput));
        }

        [TestMethod]
        public async Task GetUserAsync_Returns_User()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var user = new ApplicationUser { Email = "user@email.com", Id = guid, UserName = "user1" };

            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var userService = new UserService(_userManager.Object, _logger.Object, _mapper);
            var userdto = await userService.GetUserAsync(guid);

            // Assert 
            Assert.AreEqual(user.Email, userdto.Email);            
            Assert.AreEqual(user.Id, userdto.Id);
            Assert.AreEqual(user.UserName, userdto.UserName);
        }
    }
}
