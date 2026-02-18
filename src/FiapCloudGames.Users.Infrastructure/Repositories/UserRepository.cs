using FiapCloudGames.Users.Domain.Contracts.Repositories;
using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Infrastructure.Data;
using FiapCloudGames.Users.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Users.Infrastructure.Repositories;

public class UserRepository
    (
        AppDbContext dataContext
    )
    : Repository<User>(dataContext), IUserRepository
{
    private readonly AppDbContext _dataContext = dataContext;

    public async Task<bool> ExistsEmailAsync(string email)
    {
        var exists = await _dataContext.Users
            .AnyAsync(u => u.Email == email);

        return exists;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _dataContext.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(s => s.Email == email);

        return user;
    }
}