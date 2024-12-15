using Back.Domain.Entities;

namespace Back.App.DTOs;

public class UserRegisterDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class UserLoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class UserRegisterResponseDto
{
    public string Username;
    public string Email;
    public int Id;

    public UserRegisterResponseDto(string username, string email, int id)
    {
        Username = username;
        Email = email;
        Id = id;
    }

    public static UserRegisterResponseDto Create(User user)
    {
        return new UserRegisterResponseDto(
            user.Username,
            user.Email,
            user.Id
        );
    }
}

public class UserLoginResponseDto
{
    public string Token;

    public UserLoginResponseDto(string token)
    {
        Token = token;
    }

    public static UserLoginResponseDto Create(string token)
    {
        return new UserLoginResponseDto(token);
    }
}