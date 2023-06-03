using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Data;
using WorkQR.Application;
using WorkQR.Domain.Models;
using WorkQR.Infrastructure.Abstraction;

namespace WorkQR.Tests.xUnit
{
    public class ApplicationUserServiceTests
    {
        private readonly ApplicationUserService _sut;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        private readonly Mock<IApplicationUserRepository> _applicationUserRepositoryMock = new Mock<IApplicationUserRepository>();
        private readonly Mock<IPositionRepository> _positionRepositoryMock = new Mock<IPositionRepository>();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        public ApplicationUserServiceTests()
        {
            _sut = new ApplicationUserService(_userManagerMock.Object, _applicationUserRepositoryMock.Object, _positionRepositoryMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async void GetCompanyEmployees_ShouldReturnEmployeesList_WhenEmployeesExists()
        {
            // Arrange
            string username = "sbaran";
            Guid? companyId = new Guid();
            ApplicationUser loggedInUser = new()
            {
                Id = new Guid().ToString(),
                FirstName = "Szymon",
                LastName = "Baran",
                UserName = username,
                Position = new()
                {
                    Id = new Guid(),
                    CompanyId = companyId,
                    BreakMinsPerDay = 20
                }
            };
            List<ApplicationUser> applicationUsers = new()
            {
                new()
                {
                    Id = new Guid().ToString(),
                    FirstName = "Janusz",
                    LastName = "Kowalski",
                    UserName = "jkowalski",
                    Position = new()
                    {
                        Id = new Guid(),
                        CompanyId = companyId,
                        BreakMinsPerDay = 15
                    }
                },
                loggedInUser
            };
            _httpContextAccessorMock.Setup(h => h.HttpContext.User.Identity.Name).Returns(loggedInUser.UserName);
            _userManagerMock.Setup(x => x.FindByNameAsync(loggedInUser.UserName)).ReturnsAsync(loggedInUser);
            _applicationUserRepositoryMock.Setup(x => x.GetCompanyEmployeesListByCompanyId(loggedInUser.Position.CompanyId.Value)).ReturnsAsync(applicationUsers);

            // Act
            var list = await _sut.GetCompanyEmployees();

            // Assert
            Assert.NotEmpty(list);
        }

        [Fact]
        public async void GetCompanyEmployees_ShouldThrowException_WhenEmployeeUsernameNotExists()
        {
            // Arrange
            _httpContextAccessorMock.Setup(h => h.HttpContext.User.Identity.Name).Returns(() => null);
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.GetCompanyEmployees());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<UnauthorizedAccessException>(exception);
        }
    }
}