public class Customer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public int LoyaltyPoints { get; set; }
    public PriceLevel PriceLevel { get; set; }
}

public enum PriceLevel
{
    Regular,
    Premium,
    VIP
}