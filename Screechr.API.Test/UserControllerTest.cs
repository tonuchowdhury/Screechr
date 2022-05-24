using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Screechr.API.Controllers;
using Screechr.Model;
using Screechr.Service.API;
using System.Net;

namespace Screechr.API.Test
{
    [TestClass]
    public class UserControllerTest
    {
        private UserController _controller;
        private Mock<IUserService> _userService;
        private Mock<HttpContext> MockHttpContext { get; set; }

        public UserControllerTest()
        {
            MockHttpContext = new Mock<HttpContext>();
            _userService = new Mock<IUserService>();
            _controller = new UserController(Mock.Of<ILogger<UserController>>(), _userService.Object);
            _controller.ControllerContext.HttpContext = MockHttpContext.Object; 
        }

        [TestMethod]
        public void GetByIdTest()
        {
            long id = 3;
            var secretToken = "secret_token";
            var requestUser = new User { Id = 1, UserName = "user_name", SecretToken = secretToken };
            var expectedUser = new User { Id = 3, UserName = "user3" };
            MockHttpContext.Setup(m => m.Request.Headers.Authorization).Returns(secretToken);
            _userService.Setup(u => u.GetBySecretToken(secretToken)).Returns(requestUser);
            _userService.Setup(u => u.Get(id)).Returns(expectedUser);
            var result = _controller.Get(id) as OkObjectResult;

            _userService.VerifyAll();
            MockHttpContext.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(expectedUser.Id, ((User)result.Value).Id);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }
    }
}