using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using WorkQR.Application;
using WorkQR.Domain.Dictionaries;
using WorkQR.Domain.Models;
using WorkQR.Infrastructure.Abstraction;

namespace WorkQR.Tests.xUnit
{
    public class WorktimeEventServiceTests
    {
        private readonly WorktimeEventService _sut;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        private readonly Mock<IApplicationUserRepository> _applicationUserRepositoryMock = new();
        private readonly Mock<IWorktimeEventRepository> _worktimeEventRepository = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

        public WorktimeEventServiceTests()
        {
            _sut = new WorktimeEventService(_worktimeEventRepository.Object, _applicationUserRepositoryMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object);
        }

        [Theory]
        [InlineData(8, 16)]
        [InlineData(4, 12)]
        [InlineData(6, 18)]
        public async void GetUserWorktimeEventsToday_ShouldReturnWorktimeEventsList_WhenEmployeeExists(int startHour, int endHour)
        {
            // Arrange
            string username = "sbaran";
            ApplicationUser loggedInUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Szymon",
                LastName = "Baran",
                UserName = username,
            };
            GetUserDetailsVM model = new()
            {
                UserId = loggedInUser.Id,
                DateFrom = DateTime.Today,
                DateTo = DateTime.Today.AddDays(1),
                Description = null
            };
            List<WorktimeEvent> worktimeEvents = new()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUser = loggedInUser,
                    EventType = EventType.StartWork,
                    EventTime = DateTime.Today.AddHours(startHour),
                    Description = null
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUser = loggedInUser,
                    EventType = EventType.StartBreak,
                    EventTime = DateTime.Today.AddHours(startHour + 2),                   
                    Description = null

                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUser = loggedInUser,
                    EventType = EventType.EndBreak,
                    EventTime = DateTime.Today.AddHours(startHour + 2).AddMinutes(20),                   
                    Description = null

                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ApplicationUser = loggedInUser,
                    EventType = EventType.EndWork,
                    EventTime = DateTime.Today.AddHours(endHour),                   
                    Description = null

                },

            };

            _httpContextAccessorMock.Setup(h => h.HttpContext.User.Identity.Name).Returns(loggedInUser.UserName);
            _userManagerMock.Setup(x => x.FindByNameAsync(loggedInUser.UserName)).ReturnsAsync(loggedInUser);
            _worktimeEventRepository.Setup(x => x.GetWorktimeEvents(It.IsAny<GetUserDetailsVM>())).ReturnsAsync(worktimeEvents);

            // Act
            var list = await _sut.GetUserWorktimeEventsToday();

            // Assert
            Assert.NotEmpty(list);
            Assert.True((list.Sum(x => x.DurationInSecs) / 60 / 60) == endHour - startHour);
        }

        [Fact]
        public async void GetUserWorktimeEventsToday_ShouldThrowException_WhenEmployeeUsernameNotExists()
        {
            // Arrange
            _httpContextAccessorMock.Setup(h => h.HttpContext.User.Identity.Name).Returns(() => null);
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            // Act
            var exception = await Record.ExceptionAsync(() => _sut.GetUserWorktimeEventsToday());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<UnauthorizedAccessException>(exception);
        }
    }
}