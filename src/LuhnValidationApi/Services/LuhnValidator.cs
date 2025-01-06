using LuhnValidationApi.Exceptions;

namespace LuhnValidationApi.Services;

// Just a small comment (I left it here because it is just the homework): There are two ways to organize an interface and class.
// Some people prefer splitting them into separate files, but I personally don’t like that approach.
// If the interface and class are small, keeping them in one file makes it easier to review and maintain.

public interface ILuhnValidator
{
    /// <summary>
    ///     Checks if the provided credit card number is valid based on the Luhn algorithm.
    /// </summary>
    /// <param name="creditCardNumber">The credit card number as a string.</param>
    /// <returns>True if valid; otherwise, false.</returns>
    bool IsValid(string creditCardNumber);
}

public class LuhnValidator : ILuhnValidator
{
    public bool IsValid(string creditCardNumber)
    {
        if (string.IsNullOrWhiteSpace(creditCardNumber))
            throw new InvalidCreditCardException("Credit card number cannot be empty or whitespace.");

        // Note: We scan the string twice because of this check. It’s no problem to move it into the Luhn loop,
        // but in enterprise code—where the complexity is the same—I prefer a clearer, more readable approach.
        if (!creditCardNumber.All(char.IsDigit))
            throw new InvalidCreditCardException("Credit card number must contain only digits.");

        var sum = 0;
        var doubleDigit = false;

        for (var i = creditCardNumber.Length - 1; i >= 0; i--)
        {
            var digit = ConvertCharToDigit(creditCardNumber, i);

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