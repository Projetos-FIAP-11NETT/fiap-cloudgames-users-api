using FiapCloudGames.Users.Domain.Contracts.Repositories.Generic;
using FiapCloudGames.Users.Domain.Entities;

namespace FiapCloudGames.Users.Domain.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<bool> ExistsEmailAsync(string email);
}