using FiapCloudGames.Users.Domain.Contracts.Repositories.Generic;
using FiapCloudGames.Users.Domain.Entities;

namespace FiapCloudGames.Users.Domain.Contracts.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role> FindUserRoleAsync();

    Task<Role> FindAdminRoleAsync();
}