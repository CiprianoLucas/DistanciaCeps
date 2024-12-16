using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Back.Infra.Dependence;

public class AuthDependence
{

    public void VerifyPasswordAsync(string passwordHash, string password)
    {
        string[] parts = passwordHash.Split(':');

        string storedSalt = parts[0];
        string storedHash = parts[1];

        byte[] salt = Convert.FromBase64String(storedSalt);

        string computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        if (storedHash == computedHash)
        {
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
        };
    }

    public string HashPassword(string password)
    {
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string saltBase64 = Convert.ToBase64String(salt);

        string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return $"{saltBase64}:{hash}";
    }
}