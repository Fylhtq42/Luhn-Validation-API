using LuhnValidationApi.Services;

namespace LuhnValidationApi.Tests;

public class LuhnValidatorTests
{
    private readonly ILuhnValidator _validator = new LuhnValidator();

    [Theory]
    [InlineData("4532015112830366")]
    [InlineData("6011000990139424")]
    public void ValidateLuhn_ReturnsTrue_ForValidLuhnStrings(string input)
    {
        var result = _validator.ValidateLuhn(input);

        Assert.True(result);
    }

    [Theory]
    [InlineData("1234567890123456")]
    [InlineData("9999999999999999")]
    public void ValidateLuhn_ReturnsFalse_ForInvalidLuhnStrings(string input)
    {
        var result = _validator.ValidateLuhn(input);

        Assert.False(result);
    }
}