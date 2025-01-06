public interface IOrderWriter
{
    void WriteOrders(IEnumerable<Order> orders);
}

public interface IOrderStore
{
    List<Order> GetOrders();
}

public class OrderManager
{
    private readonly IOrderStore _orderStore;
    private readonly IOrderWriter _orderWriter;
    private readonly IOrderFilter _smallOrderFilter;
    private readonly IOrderFilter _largeOrderFilter;

    public OrderManager(
        IOrderStore orderStore,
        IOrderWriter orderWriter,
        IOrderFilter smallOrderFilter,
        IOrderFilter largeOrderFilter)
    {
        _orderStore = orderStore;
        _orderWriter = orderWriter;
        _smallOrderFilter = smallOrderFilter;
        _largeOrderFilter = largeOrderFilter;
    }

    public void WriteOutSmallOrders()
    {
        var orders = _orderStore.GetOrders();
        var filteredOrders = _smallOrderFilter.FilterAndSortOrders(orders);
        _orderWriter.WriteOrders(filteredOrders);
    }

    public void WriteOutLargeOrders()
    {
        var orders = _orderStore.GetOrders();
        var filteredOrders = _largeOrderFilter.FilterAndSortOrders(orders);
        _orderWriter.WriteOrders(filteredOrders);
    }
}
