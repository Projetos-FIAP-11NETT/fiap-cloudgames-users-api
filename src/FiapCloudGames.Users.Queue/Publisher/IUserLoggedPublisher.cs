namespace FiapCloudGames.Queue.Publisher
{
    public interface IUserLoggedPublisher
    {
        Task PublishAsync(string idToken, string refreshToken, int expiresIn, string email);
    }
}