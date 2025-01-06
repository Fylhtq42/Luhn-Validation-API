using LuhnValidationApi.Exceptions;
using LuhnValidationApi.Services;

namespace LuhnValidationApi.Tests;

public class LuhnValidatorTests
{
    private readonly ILuhnValidator _validator = new LuhnValidator();

    [Fact]
    public void ThrowsInvalidCreditCardException_WhenInputIsEmpty()
    {
        var input = "";

        Assert.Throws<InvalidCreditCardException>(() => _validator.IsValid(input));
    }

    [Fact]
    public void ThrowsInvalidCreditCardException_WhenInputContainsNonDigitCharacters()
    {
        var input = "1234abcd";

        Assert.Throws<InvalidCreditCardException>(() => _validator.IsValid(input));
    }

    [Theory]
    [InlineData("4532015112830366")]
    [InlineData("17893729974")]
    public void IsValid_ReturnsTrue_ForValidCardNumbers(string input)
    {
        var result = _validator.IsValid(input);

        Assert.True(result);
    }

    [Theory]
    [InlineData("1234567890123456")]
    [InlineData("9999999999999999")]
    [InlineData("17893729975")]
    [InlineData("17393729975")]
    public void IsValid_ReturnsFalse_ForInvalidCardNumbers(string input)
    {
        var result = _validator.IsValid(input);

        Assert.False(result);
    }
}