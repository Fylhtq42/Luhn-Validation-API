namespace LuhnValidationApi.Exceptions;

public class InvalidCreditCardException(string message) : Exception(message);