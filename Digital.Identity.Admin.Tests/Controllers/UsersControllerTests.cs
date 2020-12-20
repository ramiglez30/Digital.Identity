using Digital.Identity.Admin.Controllers;
using Digital.Identity.Admin.Models.Api;
using Digital.Identity.Admin.Models.EF;
using Digital.Identity.Admin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digital.Identity.Admin.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private readonly Mock<ILogger<UsersController>> _logger;
        private readonly Mock<IUserService> _userService;

        public UsersControllerTests()
        {
            _logger = new Mock<ILogger<UsersController>>();
            _userService = new Mock<IUserService>();
        }

        [TestMethod]
        public async Task Get_Returns_200_OK_When_Default_Paged()
        {
            // Arrange 
            var users = new List<UserDto>
            {
                new UserDto
                {
                    UserName = "user1",
                    Email = "user1@email.com",
                    Id = Guid.NewGuid().ToString()
                },
                new UserDto
                {
                    UserName = "user2",
                    Email = "user2@email.com",
                    Id = Guid.NewGuid().ToString()
                }
            };

            _userService.Setup(us => us.GetUsersAsync(It.IsAny<PagedList>())).Returns(()=>Task.FromResult((IList<UserDto>)users));

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var results  = (ObjectResult) await controller.Get();

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, results.StatusCode);
            Assert.AreEqual(2, ((IList<UserDto>)results.Value).Count);
        }

        [TestMethod]
        public async Task Get_Returns_200_OK_When_User_Exist()
        {
            // Arrange 
            var guid = Guid.NewGuid().ToString();
            var user = new UserDto
            {
                UserName = "user1",
                Email = "user1@email.com",
                Id = guid
            };

            _userService.Setup(us => us.GetUserAsync(It.IsAny<string>())).Returns(() => Task.FromResult(user));

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var result = (ObjectResult)await controller.Get(guid);

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(guid, ((UserDto)result.Value).Id);
        }

        [TestMethod]
        public async Task Get_Returns_404_NotFound_When_User_Not_Exist()
        {
            // Arrange 
            var guid = Guid.NewGuid().ToString();

            _userService.Setup(us => us.GetUserAsync(It.IsAny<string>())).Returns(() => Task.FromResult(null as UserDto));

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var result = (StatusCodeResult)await controller.Get(guid);

            // Assert
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task Post_Returns_200_OK_When_User_Added()
        {
            // Arrange 
            var guid = Guid.NewGuid().ToString();
            var newUser = new CreateUserInput { Email = "user1@email.com", Password = "A123*", UserName = "username" };
            var userDto = new UserDto { Email = "user1@email.com",  UserName = "username", Id = guid };

            _userService.Setup(us => us.CreateUserAsync(It.IsAny<CreateUserInput>())).Returns(() => Task.FromResult(userDto));

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var result = (ObjectResult)await controller.Post(newUser);

            // Assert
            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
            Assert.AreEqual(guid, ((UserDto)result.Value).Id);
        }

        [TestMethod]
        public async Task Post_Returns_409_OK_When_User_Cannot_Be_Added()
        {
            // Arrange 
            var guid = Guid.NewGuid().ToString();
            var newUser = new CreateUserInput { Email = "user1@email.com", Password = "A123*", UserName = "username" };

            _userService.Setup(us => us.CreateUserAsync(It.IsAny<CreateUserInput>())).Throws<ArgumentException>();

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var result = (ObjectResult)await controller.Post(newUser);

            // Assert
            Assert.AreEqual(StatusCodes.Status409Conflict, result.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Returns_200_OK_When_User_Deleted()
        {
            // Arrange 
            var guid = Guid.NewGuid().ToString();

            _userService.Setup(us => us.DeleteUserAsync(It.IsAny<string>())).Returns(() => Task.CompletedTask);

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var result = (StatusCodeResult)await controller.Delete(guid);

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Returns_404_NotFound_When_User_Not_Exist()
        {
            // Arrange 
            var guid = Guid.NewGuid().ToString();

            _userService.Setup(us => us.DeleteUserAsync(It.IsAny<string>())).Throws<KeyNotFoundException>();

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var result = (StatusCodeResult)await controller.Delete(guid);

            // Assert
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task Put_Returns_200_OK_When_User_Updated()
        {
            // Arrange 
            var guid = Guid.NewGuid().ToString();
            var updateUser = new EditUserInput { Email = "user@email.com", UserName = "username" };
            var outputuser = new UserDto { Email = "user@email.com", UserName = "username", Id = guid };

            _userService.Setup(us => us.EditUserAsync(It.IsAny<string>(), It.IsAny<EditUserInput>())).Returns(() => Task.FromResult(outputuser));

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var result = (ObjectResult)await controller.Put(guid, updateUser);

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(updateUser.UserName, ((UserDto)result.Value).UserName);
        }

        [TestMethod]
        public async Task Put_Returns_404_NotFound_When_User_Not_Exist()
        {
            // Arrange 
            var guid = Guid.NewGuid().ToString();
            var updateUser = new EditUserInput { Email = "user@email.com", UserName = "username" };

            _userService.Setup(us => us.EditUserAsync(It.IsAny<string>(), It.IsAny<EditUserInput>())).Throws<KeyNotFoundException>();

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var result = (StatusCodeResult)await controller.Put(guid, updateUser);

            // Assert
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task Put_Returns_409_Conflict_When_User_Could_Not_Be_Updated()
        {
            // Arrange 
            var guid = Guid.NewGuid().ToString();
            var updateUser = new EditUserInput { Email = "user@email.com", UserName = "username" };

            _userService.Setup(us => us.EditUserAsync(It.IsAny<string>(), It.IsAny<EditUserInput>())).Throws<InvalidOperationException>();

            // Act
            var controller = new UsersController(_logger.Object, _userService.Object);

            var result = (StatusCodeResult)await controller.Put(guid, updateUser);

            // Assert
            Assert.AreEqual(StatusCodes.Status409Conflict, result.StatusCode);
        }
    }
}
