using Microsoft.EntityFrameworkCore;
using Back.Domain.Entities;

namespace Back.Infra.Database;

public class DistanceRepository
{
    private readonly AppDbContext _dbContext;

    public DistanceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Distance> AddAsync(Distance distance)
    {
        await _dbContext.Distances.AddAsync(distance);
        await _dbContext.SaveChangesAsync();

        return distance; 
    }

    public async Task<Distance?> GetAsync(Distance distance)
    {
        var existingDistance = await _dbContext.Distances
        .FirstOrDefaultAsync(d => (
            d.UserId == distance.UserId &&
            d.De == distance.De &&
            d.Para == distance.Para

            )
        );

        return existingDistance; 
    }


    public async Task<Distance> UpdateAsync(Distance distance)
    {
        var existingDistance = await _dbContext.Distances
        .Where(d => (
            d.De == distance.De && 
            d.Para == distance.Para &&
            d.UserId == distance.UserId))
        .ExecuteUpdateAsync(d => d.SetProperty(e => e.Distancia, distance.Distancia));

        return distance;
    }

    public async Task<Distance> AddOrUpdateAsync(Distance distance)
    {
        Distance? existingDistance = await GetAsync(distance);
        if(existingDistance == null){
            return await AddAsync(distance);
        }
        return await UpdateAsync(distance);
    }

    public async Task<Distance[]> ListByUserAsync(User user)
    {
        var distances =  await _dbContext.Distances.Where(d => d.UserId == user.Id).ToListAsync();
        return distances.ToArray();
    }
}
