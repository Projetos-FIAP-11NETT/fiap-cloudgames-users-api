
namespace FiapCloudGames.Users.Application.Abstractions;
public interface ICorrelationIdAccessor
{
    string CorrelationId { get; }
}
