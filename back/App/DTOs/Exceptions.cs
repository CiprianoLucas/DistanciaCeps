namespace Back.App.DTOs;

public class ExceptionResponseDto
{
    public string Message { get; set; }

    public ExceptionResponseDto(string message)
    {
        Message = message;
    }

    public static ExceptionResponseDto Create(string message)
    {
        return new ExceptionResponseDto(message);
    }
}