using Xunit;
using Moq;

public class OrderManagerTests
{
    [Fact]
    public void WriteOutSmallOrders_FiltersAndWritesCorrectOrders()
    {
        // Arrange
        var mockOrderStore = new Mock<IOrderStore>();
        var mockOrderWriter = new Mock<IOrderWriter>();
        var mockSmallOrderFilter = new Mock<IOrderFilter>();
        var mockLargeOrderFilter = new Mock<IOrderFilter>();

        var orders = new List<Order>
        {
            new Order { Price = 20.0, Size = 5, Symbol = "TShirt" },
            new Order { Price = 100.0, Size = 150, Symbol = "Sport Equipment" }
        };

        var filteredOrders = new List<Order>
        {
            new Order { Price = 20.0, Size = 5, Symbol = "TShirt" }
        };

        mockOrderStore.Setup(store => store.GetOrders()).Returns(orders);
        mockSmallOrderFilter.Setup(filter => filter.FilterAndSortOrders(orders)).Returns(filteredOrders);

        var orderManager = new OrderManager(
            mockOrderStore.Object,
            mockOrderWriter.Object,
            mockSmallOrderFilter.Object,
            mockLargeOrderFilter.Object
        );

        // Act
        orderManager.WriteOutSmallOrders();

        // Assert
        mockOrderWriter.Verify(writer => writer.WriteOrders(filteredOrders), Times.Once);
    }

    [Fact]
    public void WriteOutLargeOrders_FiltersAndWritesCorrectOrders()
    {
        // Arrange
        var mockOrderStore = new Mock<IOrderStore>();
        var mockOrderWriter = new Mock<IOrderWriter>();
        var mockSmallOrderFilter = new Mock<IOrderFilter>();
        var mockLargeOrderFilter = new Mock<IOrderFilter>();

        var orders = new List<Order>
        {
            new Order { Price = 20.0, Size = 5, Symbol = "TShirt" },
            new Order { Price = 100.0, Size = 150, Symbol = "Sport Equipment" }
        };

        var filteredOrders = new List<Order>
        {
            new Order { Price = 100.0, Size = 150, Symbol = "Sport Equipment" }
        };

        mockOrderStore.Setup(store => store.GetOrders()).Returns(orders);
        mockLargeOrderFilter.Setup(filter => filter.FilterAndSortOrders(orders)).Returns(filteredOrders);

        var orderManager = new OrderManager(
            mockOrderStore.Object,
            mockOrderWriter.Object,
            mockSmallOrderFilter.Object,
            mockLargeOrderFilter.Object
        );

        // Act
        orderManager.WriteOutLargeOrders();

        // Assert
        mockOrderWriter.Verify(writer => writer.WriteOrders(filteredOrders), Times.Once);
    }
}
