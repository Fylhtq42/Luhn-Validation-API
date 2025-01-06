using LuhnValidationApi.Exceptions;

namespace LuhnValidationApi.Services;

    public interface ICreditCardService
    {
        /// <summary>
        /// Validates a raw credit card input (may include spaces or dashes).
        /// </summary>
        /// <param name="creditCardNumber">Raw user input, e.g., "4532-0151-1283-0366".</param>
        /// <returns>True if valid. Throws an exception if invalid input or fails Luhn.</returns>
        bool IsValidCreditCard(string creditCardNumber);
    }

    public class CreditCardService : ICreditCardService
    {
        private readonly ILuhnValidator _luhnValidator;

        public CreditCardService(ILuhnValidator luhnValidator)
        {
            _luhnValidator = luhnValidator;
        }

        public bool IsValidCreditCard(string creditCardNumber)
        {
            if (string.IsNullOrWhiteSpace(creditCardNumber))
            {
                throw new InvalidCreditCardException("Credit card number cannot be empty or whitespace.");
            }

            var strippedNumber = CleanupCardNumber(creditCardNumber);
            
            // Note: We scan the string twice because of this check and cleanup. It’s no problem to move it into the Luhn loop,
            // but in enterprise code—where the complexity of alg is the same—I prefer a clearer, more readable approach.
            if (string.IsNullOrEmpty(strippedNumber))
            {
                throw new InvalidCreditCardException("No digits found in the credit card number.");
            }

            if (strippedNumber.Length < 13 || strippedNumber.Length > 19)
            {
                throw new InvalidCreditCardException("Credit card number must be between 13 and 19 digits.");
            }

            return _luhnValidator.ValidateLuhn(strippedNumber);
        }

        private static string CleanupCardNumber(string creditCardNumber)
        {
            // 2. Strip out spaces, dashes, etc.
            // e.g., "4532-0151-1283-0366" -> "4532015112830366"
            return new string(creditCardNumber.Where(char.IsDigit).ToArray());
        }
    }