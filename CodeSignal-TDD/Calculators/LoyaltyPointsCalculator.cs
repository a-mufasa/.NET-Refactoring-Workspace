public class LoyaltyPointsCalculator
{
    private readonly Customer customer;
    private readonly CartItemCollection items;

    public LoyaltyPointsCalculator(Customer customer, CartItemCollection items)
    {
        this.customer = customer;
        this.items = items;
    }

    public int Calculate(List<Product> products, double subtotal)
    {
        int points = (int)(subtotal / 10); // Base points

        // Bonus points for electronics
        points += items.GetAll().Aggregate(0, (bonus, item) =>
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product?.Category == "electronics")
            {
                return bonus + (item.Quantity * 5);
            }
            return bonus;
        });

        // VIP multiplier
        if (customer.PriceLevel == PriceLevel.VIP)
        {
            points *= 2;
        }

        return points;
    }
}