using Xunit;

public class PriceCalculatorTests
{
    private readonly List<Product> products;
    private readonly Customer customer;
    private readonly CartItemCollection cartItems;
    private readonly PriceCalculator calculator;

    public PriceCalculatorTests()
    {
        products = new List<Product>
        {
            new Product { Id = "1", Price = 100 },
            new Product { Id = "2", Price = 200 }
        };
        customer = new Customer { PriceLevel = PriceLevel.Regular};
        cartItems = new CartItemCollection();
        calculator = new PriceCalculator(customer, cartItems);
    }

    [Theory]
    [InlineData(PriceLevel.Regular, 100, 100)]
    [InlineData(PriceLevel.Premium, 100, 90)]
    [InlineData(PriceLevel.VIP, 100, 85)]
    public void CalculateItemPrice_AppliesCorrectDiscount(PriceLevel priceLevel, double originalPrice, double expectedPrice)
    {
        // Arrange
        customer.PriceLevel = priceLevel;
        var product = new Product {Id = "1", Price = originalPrice};

        // Act
        var price = calculator.CalculateItemPrice(product);

        // Assert
        Assert.Equal(expectedPrice, price);
    }

    [Fact]
    public void CalculateSubtotal_WithMultipleItems_ReturnsCorrectTotal()
    {
        // Arrange
        cartItems.Add("1", 2);
        cartItems.Add("2", 1);
        
        // Act
        var subtotal = calculator.CalculateSubtotal(products);
        
        // Assert
        Assert.Equal(400, subtotal);
    }

    [Fact]
    public void CalculateSubtotal_WithNonExistentProduct_IgnoresProduct()
    {
        // Arrange
        cartItems.Add("1", 1);
        cartItems.Add("999", 1);

        // Act
        var subtotal = calculator.CalculateSubtotal(products);

        // Assert
        Assert.Equal(100, subtotal);
    }

    [Theory]
    [InlineData(100, 10)]
    [InlineData(200, 20)]
    [InlineData(0, 0)]
    public void CalculateTax_ReturnsCorrectAmount(double subtotal, double expectedTax)
    {
        // Act
        var postTax = calculator.CalculateTax(subtotal); 
        
        // Assert
        Assert.Equal(postTax, expectedTax);
    }
}