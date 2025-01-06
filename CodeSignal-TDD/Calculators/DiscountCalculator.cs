public class DiscountCalculator
{
    private string discountCode;
    private readonly Dictionary<string, double> DISCOUNT_RATES = new()
    {
        { "SAVE10", 0.1 },
        { "SUMMER20", 0.2 },
        { "VIP15", 0.15 }
    };

    public void ApplyDiscountCode(string code)
    {
        if (!DISCOUNT_RATES.ContainsKey(code))
        {
            throw new ArgumentException("Invalid discount code");
        }
        discountCode = code;
    }

    public double CalculateDiscount(double subtotal)
    {
        if (string.IsNullOrEmpty(discountCode)) return 0;
        return subtotal * DISCOUNT_RATES[discountCode];
    }
}