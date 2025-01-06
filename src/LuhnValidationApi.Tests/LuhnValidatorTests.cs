using LuhnValidationApi.Services;

namespace LuhnValidationApi.Tests;

public class LuhnValidatorTests
{
    private readonly ILuhnValidator _validator;

    public LuhnValidatorTests()
    {
        _validator = new LuhnValidator();
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenEmptyString()
    {
        var input = "";

        var result = _validator.IsValid(input);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenNonNumeric()
    {
        var input = "1234abcd";

        var result = _validator.IsValid(input);

        Assert.False(result);
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