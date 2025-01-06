using Xunit;

public class LoyaltyPointsCalculatorTests
{
    private readonly List<Product> products;
    private readonly Customer customer;
    private readonly CartItemCollection cartItems;
    private readonly LoyaltyPointsCalculator calculator;

    public LoyaltyPointsCalculatorTests()
    {
        // Arrange
        products = new List<Product>
        {
            new Product { Id = "1", Category = "electronics", Price = 100 },
            new Product { Id = "2", Category = "books", Price = 50 }
        };
        customer = new Customer { PriceLevel = PriceLevel.Regular };
        cartItems = new CartItemCollection();
        calculator = new LoyaltyPointsCalculator(customer, cartItems);
    }

    [Fact]
    public void Calculate_WithRegularCustomer_ReturnsBasePoints()
    {
        // Act
        var points = calculator.Calculate(products, 100);

        // Assert
        Assert.Equal(10, points); // $100 = 10 base points
    }

    [Fact]
    public void Calculate_WithVIPCustomer_DoublesPoints()
    {
        // Arrange
        customer.PriceLevel = PriceLevel.VIP;

        // Act
        var points = calculator.Calculate(products, 100);

        // Assert
        Assert.Equal(20, points); // ($100 = 10 points) * 2 VIP multiplier
    }

    [Fact]
    public void Calculate_WithElectronics_AddsBonusPoints()
    {
        // Arrange
        cartItems.Add("1", 2);  // Electronics item

        // Act
        var points = calculator.Calculate(products, 200);

        // Assert
        Assert.Equal(30, points); // (200/10 = 20 base points) + (2 items * 5 bonus points)
    }

    [Fact]
    public void Calculate_WithElectronicsAndVIP_AppliesBothBonuses()
    {
        // Arrange
        customer.PriceLevel = PriceLevel.VIP;
        cartItems.Add("1", 2); // Electronics item

        // Act
        var points = calculator.Calculate(products, 200);

        // Assert
        Assert.Equal(60, points); // ((200/10 = 20 base points) + (2 items * 5 bonus points)) * 2 VIP multiplier
    }
}