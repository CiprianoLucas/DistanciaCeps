using Back.Domain.Entities;
using Back.Infra.Database;
using Back.Infra.Dependence;


namespace Back.Domain.Services;
public class UserService
{
    private readonly UserRepository _UserRepository;
    private readonly AuthDependence _AuthDependence;

    public UserService(UserRepository repository, AuthDependence authDependence)
    {
        _UserRepository = repository;
        _AuthDependence = authDependence;
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _UserRepository.GetByEmailAsync(email);
    }

    public async Task<User> CreateUserAsync(string username, string password, string email)
    {
        if (await GetByEmailAsync(email) != null)
            throw new InvalidOperationException("Usuário já cadastrado com este e-mail.");

        string passwordHash = _AuthDependence.HashPassword(password);

        var user = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            Email = email
        };

        user = await _UserRepository.AddAsync(user);

        return user;
    }

    public async Task<User> VerifyPasswordAsync(string email, string password)
    {
        User? User = await GetByEmailAsync(email);

        if (User == null)
        {
            throw new KeyNotFoundException($"Usuário com Email {email} não foi encontrado.");
        }
        _AuthDependence.VerifyPasswordAsync(User.PasswordHash, password);

        return User;
    }
}