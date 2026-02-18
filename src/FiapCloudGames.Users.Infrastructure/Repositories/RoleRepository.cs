namespace FiapCloudGames.Users.Infrastructure.Repositories;

using FiapCloudGames.Users.Domain.Contracts.Repositories;
using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Infrastructure.Data;
using FiapCloudGames.Users.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoleRepository(AppDbContext dataContext) : Repository<Role>(dataContext), IRoleRepository
{
    private readonly AppDbContext _dataContext = dataContext;

    public async Task<Role> FindUserRoleAsync()
    {
        var role = await _dataContext.Roles
            .FirstOrDefaultAsync(r => r.Name == "User");

        return role;
    }

    public async Task<Role> FindAdminRoleAsync()
    {
        var role = await _dataContext.Roles
            .FirstOrDefaultAsync(r => r.Name == "Admin");

        return role;
    }
}