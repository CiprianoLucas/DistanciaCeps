using Back.Domain.Entities;
using Back.Infra.Database;
using Back.Infra.Dependence;

namespace Back.Domain.Services;

public class TokenService
{
    private readonly UserRepository _userRepository;
    private readonly TokenDependence _tokenDependence;

    public TokenService(UserRepository userRepository, TokenDependence tokenDependence)
    {
        _userRepository = userRepository;
        _tokenDependence = tokenDependence;
    }

    public string GenerateToken(User user)
    {
        return _tokenDependence.GenerateToken(user.Email);
    }

    public async Task<User> VerifyToken(string? token)
    {
        string? email = _tokenDependence.VerifyToken(token);
        User? user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Token inválido");
        }
        return user;
    }
}
