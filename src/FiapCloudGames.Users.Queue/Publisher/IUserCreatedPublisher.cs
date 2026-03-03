namespace FiapCloudGames.Queue.Publisher
{
    public interface IUserCreatedPublisher
    {
        Task PublishAsync( Guid idUser, string email, string name);
    }
}
