public class PriceCalculator
{
    private const double TAX_RATE = 0.1;
    private readonly Customer customer;
    private readonly CartItemCollection items;

    public PriceCalculator(Customer customer, CartItemCollection items)
    {
        this.customer = customer;
        this.items = items;
    }

    public double CalculateItemPrice(Product product)
    {
        double price = product.Price;
        switch (customer.PriceLevel)
        {
            case PriceLevel.Premium:
                price *= 0.9; // 10% off
                break;
            case PriceLevel.VIP:
                price *= 0.85; // 15% off
                break;
        }
        return price;
    }

    public double CalculateSubtotal(List<Product> products)
    {
        return items.GetAll().Aggregate(0.0, (total, item) =>
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product != null)
            {
                total += CalculateItemPrice(product) * item.Quantity;
            }
            return total;
        });
    }

    public double CalculateTax(double subtotal)
    {
        return subtotal * TAX_RATE;
    }
}