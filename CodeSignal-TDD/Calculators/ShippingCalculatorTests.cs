using Xunit;

public class ShippingCalculatorTests
{
    private readonly List<Product> products;
    private readonly Customer customer;
    private readonly CartItemCollection cartItems;
    private readonly ShippingCalculator calculator;

    public ShippingCalculatorTests()
    {
        // Arrange
        products = new List<Product>
        {
            new Product { Id = "1", Weight = 2.0 },
            new Product { Id = "2", Weight = 3.0 }
        };
        customer = new Customer { PriceLevel = PriceLevel.Regular };
        cartItems = new CartItemCollection();
        calculator = new ShippingCalculator(customer, cartItems);
    }

    [Fact]
    public void Calculate_VIPCustomer_ReturnsZero()
    {
        // Arrange
        customer.PriceLevel = PriceLevel.VIP;
        
        // Act
        var cost = calculator.Calculate(products, 50);

        // Assert
        Assert.Equal(0, cost);
    }

    [Fact]
    public void Calculate_SubtotalOver100_ReturnsZero()
    {
        // Act
        var cost = calculator.Calculate(products, 150);

        // Assert
        Assert.Equal(0, cost);
    }

    [Fact]
    public void Calculate_StandardShipping_ReturnsCorrectCost()
    {
        // Arrange
        cartItems.Add("1", 2);
        
        // Act
        var cost = calculator.Calculate(products, 50);

        // Assert
        Assert.Equal(5.4, cost); // BASE_RATE(5) + (2 weight * 2 quantity * 0.1 rate)
    }

    [Fact]
    public void Calculate_NonExistentProduct_IgnoresWeight()
    {
        // Arrange
        cartItems.Add("999", 1);
        
        // Act
        var cost = calculator.Calculate(products, 50);

        // Assert
        Assert.Equal(5.0, cost); // Just BASE_RATE
    }
}