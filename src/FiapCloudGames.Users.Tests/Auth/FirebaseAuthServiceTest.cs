using FiapCloudGames.Users.Auth;
using FiapCloudGames.Users.Auth.Adapter;
using FiapCloudGames.Users.Tests.Factories;
using Moq;

namespace FiapCloudGames.Users.Tests.Auth;

public sealed class FirebaseAuthServiceTest
{
    [Fact]
    public async Task CreateUserAsync_MustThrow_WhenClientFails()
    {
        // Arrange
        var firebaseClientMock = new Mock<IFirebaseService>();

        firebaseClientMock
            .Setup(x => x.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ThrowsAsync(new Exception("Firebase error"));

        var service = new AuthService(firebaseClientMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            service.CreateUserAsync(
                "user@email.com",
                "123456",
                "User",
                [RoleFactory.CreateUserRole().Name]));

        // Assert
        Assert.Equal("Firebase error", exception.Message);
    }

    [Theory]
    [InlineData("teste1@gmail.com", "123456", "Usuário1", "firebase-uid-123")]
    public async Task CreateUserAsync_MustCallClientAndReturnUid(string email, string password, string name, string firebaseUserId)
    {
        // Arrange
        var firebaseClientMock = new Mock<IFirebaseService>();

        firebaseClientMock
            .Setup(x => x.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(firebaseUserId);

        var service = new AuthService(firebaseClientMock.Object);

        // Act
        var result = await service.CreateUserAsync(email, password, name, [RoleFactory.CreateUserRole().Name]);

        // Assert
        firebaseClientMock.Verify(x => x.CreateUserAsync(email, password, name), Times.Once);
        Assert.Equal(firebaseUserId, result);
    }
}