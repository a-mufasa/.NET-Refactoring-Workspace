public interface IOrderFilter
{
    IEnumerable<Order> FilterAndSortOrders(IEnumerable<Order> orders);
}

public class LargeOrderFilter : IOrderFilter
{
    private readonly int _filterSize;

    public LargeOrderFilter(int filterSize = 100)
    {
        _filterSize = filterSize;
    }

    public IEnumerable<Order> FilterAndSortOrders(IEnumerable<Order> orders)
    {
        return orders
            .Where(order => order.Size > _filterSize)
            .OrderBy(order => order.Price);
    }
}

public class SmallOrderFilter : IOrderFilter
{
    private readonly int _filterSize;

    public SmallOrderFilter(int filterSize = 10)
    {
        _filterSize = filterSize;
    }

    public IEnumerable<Order> FilterAndSortOrders(IEnumerable<Order> orders)
    {
        return orders
            .Where(order => order.Size <= _filterSize)
            .OrderBy(order => order.Price);
    }
}