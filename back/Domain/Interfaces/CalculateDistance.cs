using Back.Domain.Entities;
namespace Back.Domain.Interfaces;
public interface LocalInterface
{
    Task AddAsync(Local local);
    Task<Local?> GetByCepAsync(string cep);
    Task<IEnumerable<Local>> GetAllAsync();
}