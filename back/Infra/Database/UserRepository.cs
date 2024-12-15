using Microsoft.EntityFrameworkCore;
using Back.Domain.Entities;

namespace Back.Infra.Database;

public class UserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user; 
    }

    public async Task<User?> GetByEmailAsync(string? email)
    {
        if(email == null){
            throw new KeyNotFoundException("Email não informado");
        }
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
