
namespace FiapCloudGames.Users.Shared.Abstractions;
public interface ICorrelationIdAccessor
{
    Guid CorrelationId { get; }
}
