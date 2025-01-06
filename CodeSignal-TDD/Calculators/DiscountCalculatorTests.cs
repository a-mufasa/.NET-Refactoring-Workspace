using Xunit;

public class DiscountCalculatorTests
{
    private readonly DiscountCalculator calculator;

    public DiscountCalculatorTests()
    {
        // Arrange
        calculator = new DiscountCalculator();
    }

    [Theory]
    [InlineData("SAVE10", 100, 10)]
    [InlineData("SUMMER20", 100, 20)]
    [InlineData("VIP15", 100, 15)]
    [InlineData("SAVE10", 200, 20)]
    public void CalculateDiscount_WithValidCode_ReturnsCorrectAmount(string code, double subtotal, double expectedDiscount)
    {
        // Act
        calculator.ApplyDiscountCode(code);
        double discount = calculator.CalculateDiscount(subtotal);

        // Assert
        Assert.Equal(expectedDiscount, discount);
    }

    [Fact]
    public void CalculateDiscount_WithNoCode_ReturnsZero()
    {
        // Act
        double discount = calculator.CalculateDiscount(100);
        
        // Assert
        Assert.Equal(0, discount);
    }

    [Theory]
    [InlineData("INVALID")]
    [InlineData("save10")]
    [InlineData("TEST123")]
    public void ApplyDiscountCode_WithInvalidCode_ThrowsArgumentException(string invalidCode)
    {
        // Act, Assert

        Assert.Throws<ArgumentException>(() => calculator.ApplyDiscountCode(invalidCode));
    }
}