public class ShippingCalculator
{
    private const double BASE_RATE = 5;
    private const double WEIGHT_RATE = 0.1;
  
    private readonly Customer customer;
    private readonly CartItemCollection items;

    public ShippingCalculator(Customer customer, CartItemCollection items)
    {
        this.customer = customer;
        this.items = items;
    }
  
    public double Calculate(List<Product> products, double subtotal)
    {
        if (customer.PriceLevel == PriceLevel.VIP || subtotal > 100)
        {
            return 0;
        }
  
        double totalWeight = items.GetAll().Aggregate(0.0, (weight, item) =>
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            return weight + ((product?.Weight ?? 0) * item.Quantity);
        });
  
        return BASE_RATE + (totalWeight * WEIGHT_RATE);
    }
}