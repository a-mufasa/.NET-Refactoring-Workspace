using Xunit;

public class ShoppingCartTests : IDisposable
{
    private ShoppingCart _cart;
    private readonly Customer _regularCustomer;
    private readonly Customer _premiumCustomer;
    private readonly Customer _vipCustomer;
    private readonly List<Product> _products;

    public ShoppingCartTests()
    {
        // Setup test fixtures - equivalent to beforeEach in Jest
        _regularCustomer = new Customer
        {
            Id = "1",
            Name = "John Doe",
            Email = "john@example.com",
            Address = "123 Main St",
            LoyaltyPoints = 100,
            PriceLevel = PriceLevel.Regular
        };

        _premiumCustomer = new Customer
        {
            Id = "2",
            Name = "John Doe",
            Email = "john@example.com",
            Address = "123 Main St",
            LoyaltyPoints = 100,
            PriceLevel = PriceLevel.Premium
        };

        _vipCustomer = new Customer
        {
            Id = "3",
            Name = "John Doe",
            Email = "john@example.com",
            Address = "123 Main St",
            LoyaltyPoints = 100,
            PriceLevel = PriceLevel.VIP
        };

        _products = new List<Product>
        {
            new Product
            {
                Id = "p1",
                Name = "Laptop",
                Price = 1000,
                Weight = 2.5,
                Category = "electronics",
                InStock = true
            },
            new Product
            {
                Id = "p2",
                Name = "Book",
                Price = 20,
                Weight = 0.5,
                Category = "books",
                InStock = true
            },
            new Product
            {
                Id = "p3",
                Name = "Smartphone",
                Price = 800,
                Weight = 0.3,
                Category = "electronics",
                InStock = true
            }
        };
    }

    public void Dispose()
    {
        // Cleanup after each test if needed
    }

    public class CartManagementTests : ShoppingCartTests
    {
        [Fact]
        public void ShouldInitializeWithEmptyCart()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);

            // Act
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(0, summary.Subtotal);
        }

        [Fact]
        public void ShouldAddItemsToCart()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var laptop = _products[0];
            var book = _products[1];

            // Act
            _cart.AddItem(laptop, 1);
            _cart.AddItem(book, 2);
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(1040, summary.Subtotal); // 1000 + (2 * 20)
        }

        [Fact]
        public void ShouldAddMultipleQuantitiesOfSameItem()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var laptop = _products[0];

            // Act
            _cart.AddItem(laptop, 1);
            _cart.AddItem(laptop, 2);
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(3000, summary.Subtotal); // 3 * 1000
        }

        [Fact]
        public void ShouldUpdateQuantity()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var laptop = _products[0];
            _cart.AddItem(laptop, 1);

            // Act
            _cart.UpdateQuantity(laptop.Id, 2);
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(2000, summary.Subtotal);
        }

        [Fact]
        public void ShouldRemoveItemsWhenQuantitySetToZero()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var laptop = _products[0];
            _cart.AddItem(laptop, 1);

            // Act
            _cart.UpdateQuantity(laptop.Id, 0);
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(0, summary.Subtotal);
        }

        [Fact]
        public void ShouldThrowErrorForOutOfStockItems()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var outOfStockProduct = new Product
            {
                Id = _products[0].Id,
                Name = _products[0].Name,
                Price = _products[0].Price,
                Weight = _products[0].Weight,
                Category = _products[0].Category,
                InStock = false
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => 
                _cart.AddItem(outOfStockProduct, 1));
            Assert.Equal("Product out of stock", exception.Message);
        }
    }

    public class PriceCalculationsTests : ShoppingCartTests
    {
        [Fact]
        public void ShouldApplyPremiumCustomerDiscount()
        {
            // Arrange
            _cart = new ShoppingCart(_premiumCustomer);
            var laptop = _products[0];

            // Act
            _cart.AddItem(laptop, 1);
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(900, summary.Subtotal); // 10% off 1000
        }

        [Fact]
        public void ShouldApplyVipDiscount()
        {
            // Arrange
            _cart = new ShoppingCart(_vipCustomer);
            var laptop = _products[0];

            // Act
            _cart.AddItem(laptop, 1);
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(850, summary.Subtotal); // 15% off 1000
        }

        [Fact]
        public void ShouldCalculateTaxCorrectly()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var laptop = _products[0];

            // Act
            _cart.AddItem(laptop, 1);
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(100, summary.Tax); // 10% of 1000
        }
    }

    public class DiscountCodeTests : ShoppingCartTests
    {
        public DiscountCodeTests()
        {
            _cart = new ShoppingCart(_regularCustomer);
            _cart.AddItem(_products[0], 1); // $1000 laptop
        }

        [Fact]
        public void ShouldApplySummer20DiscountCode()
        {
            // Act
            _cart.ApplyDiscountCode("SUMMER20");
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(200, summary.Discount); // 20% of 1000
        }

        [Fact]
        public void ShouldAllowUpdatingDiscountCode()
        {
            // Act
            _cart.ApplyDiscountCode("SAVE10");
            _cart.ApplyDiscountCode("SUMMER20");
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(200, summary.Discount); // Should use latest discount
        }
    }

    public class ShippingCalculationsTests : ShoppingCartTests
    {
        [Fact]
        public void ShouldCalculateBaseShippingCost()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var book = _products[1]; // 0.5kg, $20

            // Act
            _cart.AddItem(book, 1);
            var shipping = _cart.CalculateShippingCost(_products);

            // Assert
            Assert.Equal(5.05, shipping); // Base $5 + (0.5kg * $0.1)
        }

        [Fact]
        public void ShouldProvideShippingOver100()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var laptop = _products[0]; // $1000

            // Act
            _cart.AddItem(laptop, 1);
            var shipping = _cart.CalculateShippingCost(_products);

            // Assert
            Assert.Equal(0, shipping);
        }

        [Fact]
        public void ShouldProvideShippingForVipCustomers()
        {
            // Arrange
            _cart = new ShoppingCart(_vipCustomer);
            var book = _products[1]; // Low price item

            // Act
            _cart.AddItem(book, 1);
            var shipping = _cart.CalculateShippingCost(_products);

            // Assert
            Assert.Equal(0, shipping);
        }
    }

    public class LoyaltyPointsTests : ShoppingCartTests
    {
        [Fact]
        public void ShouldCalculatePointsForMultipleItems()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var book = _products[1]; // $20 each

            // Act
            _cart.AddItem(book, 5); // $100 worth of books
            var points = _cart.CalculateLoyaltyPoints(_products);

            // Assert
            Assert.Equal(10, points); // 10 points for $100
        }

        [Fact]
        public void ShouldAddBonusPointsForElectronics()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var laptop = _products[0];
            var smartphone = _products[2];

            // Act
            _cart.AddItem(laptop, 1);
            _cart.AddItem(smartphone, 1);
            var points = _cart.CalculateLoyaltyPoints(_products);

            // Assert
            Assert.Equal(190, points); // 180 base points + (2 * 5) electronics bonus
        }

        [Fact]
        public void ShouldDoublePointsForVipCustomers()
        {
            // Arrange
            _cart = new ShoppingCart(_vipCustomer);
            var laptop = _products[0];

            // Act
            _cart.AddItem(laptop, 1);
            var points = _cart.CalculateLoyaltyPoints(_products);

            // Assert
            Assert.Equal(180, points); // (85 points + 5 bonus) * 2 for VIP
        }
    }

    public class OrderSummaryTests : ShoppingCartTests
    {
        [Fact]
        public void ShouldGenerateCompleteOrderSummary()
        {
            // Arrange
            _cart = new ShoppingCart(_regularCustomer);
            var laptop = _products[0];
            _cart.AddItem(laptop, 1);
            _cart.ApplyDiscountCode("SAVE10");

            // Act
            var summary = _cart.GetOrderSummary(_products);

            // Assert
            Assert.Equal(1000, summary.Subtotal);
            Assert.Equal(100, summary.Tax);
            Assert.Equal(100, summary.Discount);
            Assert.Equal(0, summary.ShippingCost);
            Assert.Equal(1000, summary.Total);
            Assert.Equal(105, summary.LoyaltyPoints);
        }

        [Fact]
        public void ShouldCalculateComplexOrderCorrectly()
        {
            // Arrange
            _cart = new ShoppingCart(_premiumCustomer);
            var laptop = _products[0];
            var book = _products[1];
            _cart.AddItem(laptop, 2); // 2 laptops
            _cart.AddItem(book, 3); // 3 books
            _cart.ApplyDiscountCode("SUMMER20");

            // Act
            var summary = _cart.GetOrderSummary(_products);

            // Calculate expected values
            var expectedSubtotal = (2 * 1000 * 0.9) + (3 * 20 * 0.9); // Premium gets 10% off

            // Assert
            Assert.Equal(expectedSubtotal, summary.Subtotal);
            Assert.Equal(expectedSubtotal * 0.1, summary.Tax);
            Assert.Equal(expectedSubtotal * 0.2, summary.Discount); // SUMMER20 gives 20% off
            Assert.Equal(0, summary.ShippingCost); // Free shipping over $100
            Assert.Equal(
                expectedSubtotal + (expectedSubtotal * 0.1) - (expectedSubtotal * 0.2),
                summary.Total
            );
            Assert.Equal(
                (int)(expectedSubtotal / 10) + (2 * 5), // Base points + electronics bonus
                summary.LoyaltyPoints
            );
        }
    }
}