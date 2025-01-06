using LuhnValidationApi.Exceptions;

namespace LuhnValidationApi.Services;

// Just a small comment (I left it here because it is just the homework): There are two ways to organize an interface and class.
// Some people prefer splitting them into separate files, but I personally don’t like that approach.
// If the interface and class are small, keeping them in one file makes it easier to review and maintain.

public interface ILuhnValidator
{
    /// <summary>
    /// Performs a pure Luhn check on a string of digits.
    /// </summary>
    /// <param name="digitsOnly">A string containing only digits (e.g., "4532015112830366").</param>
    /// <returns>True if the string passes Luhn; otherwise, false.</returns>
    bool ValidateLuhn(string digitsOnly);
}

public class LuhnValidator : ILuhnValidator
{
    public bool ValidateLuhn(string digitsOnly)
    {
        var sum = 0;
        var doubleDigit = false;

        for (var i = digitsOnly.Length - 1; i >= 0; i--)
        {
            var digit = ConvertCharToDigit(digitsOnly, i);

            if (doubleDigit) digit = HandleDouble(digit);

            sum += digit;
            doubleDigit = !doubleDigit;
        }

        // We sum all digits (including the check digit) and check if the total is divisible by 10.
        // This is mathematically the same as the Wikipedia approach, where the check digit is 
        // (10 - (sum mod 10)) mod 10. Both methods provide the same final result.
        return sum % 10 == 0;
    }

    private static int ConvertCharToDigit(string creditCardNumber, int i)
    {
        return creditCardNumber[i] - '0';
    }

    private static int HandleDouble(int digit)
    {
        digit *= 2;
        if (digit >= 10)
            digit -= 9;
        return digit;
    }
}