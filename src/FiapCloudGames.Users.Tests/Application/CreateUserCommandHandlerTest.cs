using FiapCloudGames.Users.Application.UserFeature.Commands.CreateUser;
using FiapCloudGames.Users.Auth;
using FiapCloudGames.Users.Domain.Contracts.Repositories;
using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Tests.Factories;
using Moq;
using Moq.AutoMock;

namespace FiapCloudGames.Users.Tests.Application;

public class CreateUserCommandHandlerTests
{
    [Theory]
    [InlineData("João Silva", "joao@email.com", "123456#f", "qw-09")]
    public async Task Handle_MustReturnTrue_WhenCommandIsValid(string name, string email, string password, string firebaseUserId)
    {
        // Arrange

        var role = RoleFactory.CreateUserRole();

        var mock = new AutoMocker();
        mock.GetMock<IRoleRepository>().Setup(x=>x.FindUserRoleAsync()).ReturnsAsync(role);

        mock.GetMock<IAuthService>()
            .Setup(x => x.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),                
                new List<string>() { role.Name },
                It.IsAny<Guid>()))
            .ReturnsAsync(firebaseUserId);

        mock.GetMock<IUserRepository>()
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        mock.GetMock<IUserRepository>()
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var _createUserCommandHandler = mock.CreateInstance<CreateUserCommandHandler>();

        var command = new CreateUserCommand(name, email, password);

        // Act
        var result = await _createUserCommandHandler.Handle(command, CancellationToken.None);

        // Assert

        mock.GetMock<IUserRepository>().Verify(
            x => x.AddAsync(It.Is<User>(u => u.FirebaseUserId == firebaseUserId)),
            Times.Once);

        mock.GetMock<IUserRepository>().Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.True(result);
    }
}

