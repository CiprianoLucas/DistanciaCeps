namespace Back.Domain.Validations;
using System;
using System.Text.RegularExpressions;
using System.Net.Mail;


public static class PasswordValidator
{
    public static void Validate(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException(nameof(password), "A senha não pode estar em branco.");
        }

        if (password.Length < 8)
        {
            throw new ArgumentException("A senha deve ter pelo menos 8 caracteres.");
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            throw new ArgumentException("A senha deve conter pelo menos uma letra maiúscula.");
        }

        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            throw new ArgumentException("A senha deve conter pelo menos uma letra minúscula.");
        }

        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            throw new ArgumentException("A senha deve conter pelo menos um número.");
        }
    }
}

public static class UsernameValidator
{
    public static void Validate(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentNullException(nameof(username), "O nome de usuário não pode estar em branco.");
        }

        if (username.Length < 5 || username.Length > 20)
        {
            throw new ArgumentException("O nome de usuário deve ter entre 5 e 20 caracteres.");
        }

        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
        {
            throw new ArgumentException("O nome de usuário deve conter apenas letras, números e o caractere de sublinhado (_).");
        }
    }
}

public static class EmailValidator
{
    public static void Validate(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email), "O e-mail não pode estar em branco.");
        }

        try
        {
            var mailAddress = new MailAddress(email);
        }
        catch (FormatException)
        {
            throw new ArgumentException("O e-mail fornecido não possui um formato válido.");
        }
    }
}