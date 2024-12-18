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
    public string Username { get; set; }
    public string Email { get; set; }
    public int Id { get; set; }

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
    public string Username { get; set; }

    public UserLoginResponseDto(string username)
    {
        Username = username;
    }

    public static UserLoginResponseDto Create(string username)
    {
        return new UserLoginResponseDto(username);
    }
}