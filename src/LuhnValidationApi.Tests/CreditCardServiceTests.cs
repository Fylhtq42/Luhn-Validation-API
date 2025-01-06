using LuhnValidationApi.Exceptions;
using LuhnValidationApi.Services;
using Moq;

namespace LuhnValidationApi.Tests;

public class CreditCardServiceTests
{
    private readonly CreditCardService _creditCardService;
    private readonly Mock<ILuhnValidator> _luhnValidatorMock;

    public CreditCardServiceTests()
    {
        _luhnValidatorMock = new Mock<ILuhnValidator>();
        _creditCardService = new CreditCardService(_luhnValidatorMock.Object);
    }

    [Fact]
    public void ThrowsInvalidCreditCardException_WhenInputIsEmptyOrWhitespace()
    {
        Assert.Throws<InvalidCreditCardException>(() =>
            _creditCardService.IsValidCreditCard(""));

        Assert.Throws<InvalidCreditCardException>(() =>
            _creditCardService.IsValidCreditCard("   "));
    }

    [Fact]
    public void ThrowsInvalidCreditCardException_WhenNoDigitsAreFound()
    {
        Assert.Throws<InvalidCreditCardException>(() =>
            _creditCardService.IsValidCreditCard("----"));
    }

    [Fact]
    public void ThrowsInvalidCreditCardException_WhenNumberIsTooShort()
    {
        // e.g., 12 digits
        var shortNumber = new string('1', 12);

        Assert.Throws<InvalidCreditCardException>(() =>
            _creditCardService.IsValidCreditCard(shortNumber));
    }

    [Fact]
    public void ThrowsInvalidCreditCardException_WhenNumberIsTooLong()
    {
        // e.g., 20 digits
        var longNumber = new string('1', 20);

        Assert.Throws<InvalidCreditCardException>(() =>
            _creditCardService.IsValidCreditCard(longNumber));
    }

    [Fact]
    public void ReturnsFalse_WhenLuhnValidationFails()
    {
        var input = "9999999999999999";

        _luhnValidatorMock
            .Setup(v => v.ValidateLuhn(It.IsAny<string>()))
            .Returns(false);

        var result = _creditCardService.IsValidCreditCard(input);

        Assert.False(result);
        _luhnValidatorMock.Verify(v => v.ValidateLuhn("9999999999999999"), Times.Once);
    }

    [Fact]
    public void ReturnsTrue_WhenLuhnValidationSucceeds()
    {
        var input = "4532015112830366";

        _luhnValidatorMock
            .Setup(v => v.ValidateLuhn(It.IsAny<string>()))
            .Returns(true);

        var result = _creditCardService.IsValidCreditCard(input);

        Assert.True(result);
        _luhnValidatorMock.Verify(v => v.ValidateLuhn("4532015112830366"), Times.Once);
    }

    [Fact]
    public void DoStripsNonDigits_BeforeLuhnCheck()
    {
        var input = "4532-0151-1283-0366";

        _luhnValidatorMock
            .Setup(v => v.ValidateLuhn("4532015112830366"))
            .Returns(true);

        var result = _creditCardService.IsValidCreditCard(input);

        Assert.True(result);
        _luhnValidatorMock.Verify(v => v.ValidateLuhn("4532015112830366"), Times.Once);
    }
}