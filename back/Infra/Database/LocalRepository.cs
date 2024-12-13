using Microsoft.EntityFrameworkCore;
using Back.Domain.Entities;
using Back.Domain.Interfaces;

namespace Back.Infra.Database;

public class LocalRepository : LocalInterface
{
    private readonly AppDbContext _dbContext;

    public LocalRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Local local)
    {
        await _dbContext.Locals.AddAsync(local);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Local?> GetByCepAsync(string id)
    {
        return await _dbContext.Locals.FindAsync(id);
    }

    public async Task<IEnumerable<Local>> GetAllAsync()
    {
        return await _dbContext.Locals.ToListAsync();
    }
}
