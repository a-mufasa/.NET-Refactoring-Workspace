public class CartItem
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }

    public CartItem(string productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}

public class CartItemCollection
{
    private readonly Dictionary<string, CartItem> _items = new();

    public void Add(string productId, int quantity)
    {
        if (_items.TryGetValue(productId, out CartItem existingItem))
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            _items[productId] = new CartItem(productId, quantity);
        }
    }

    public void Update(string productId, int quantity)
    {
        if (quantity <= 0)
        {
            Remove(productId);
        }
        else
        {
            _items[productId] = new CartItem(productId, quantity);
        }
    }

    public void Remove(string productId)
    {
        _items.Remove(productId);
    }

    public IEnumerable<CartItem> GetAll()
    {
        return _items.Values;
    }
}

public class ShoppingCart 
{
    private readonly CartItemCollection items = new CartItemCollection();
    private readonly DiscountCalculator discountCalculator = new DiscountCalculator();
    private readonly PriceCalculator priceCalculator;
    private readonly ShippingCalculator shippingCalculator;
    private readonly LoyaltyPointsCalculator loyaltyPointsCalculator;
    
    private readonly Customer customer;

    public ShoppingCart(Customer customer)
    {
        this.customer = customer;
        this.priceCalculator = new PriceCalculator(customer, items);
        this.shippingCalculator = new ShippingCalculator(customer, items);
        this.loyaltyPointsCalculator = new LoyaltyPointsCalculator(customer, items);
    }

    // Cart management
    public void AddItem(Product product, int quantity)
    {
        if (!product.InStock)
        {
            throw new InvalidOperationException("Product out of stock");
        }
        items.Add(product.Id, quantity);
    }

    public void RemoveItem(string productId)
    {
        items.Remove(productId);
    }

    public void UpdateQuantity(string productId, int quantity)
    {
        if (quantity <= 0)
        {
            RemoveItem(productId);
        }
        else
        {
            items.Update(productId, quantity);
        }
    }

    public void ApplyDiscountCode(string code)
    {
        discountCalculator.ApplyDiscountCode(code);
    }

    // Shipping calculations
    public double CalculateShippingCost(List<Product> products)
    {
        var subTotal = priceCalculator.CalculateSubtotal(products);
        
        return this.shippingCalculator.Calculate(products, subTotal);
    }

    // Loyalty points
    public int CalculateLoyaltyPoints(List<Product> products)
    {
        double subtotal = priceCalculator.CalculateSubtotal(products);
        return this.loyaltyPointsCalculator.Calculate(products, subtotal);
    }

    // Order summary
    public (double Subtotal, double Tax, double Discount, double ShippingCost, double Total, int LoyaltyPoints) GetOrderSummary(List<Product> products)
    {
        double subtotal = priceCalculator.CalculateSubtotal(products);
        double tax = priceCalculator.CalculateTax(subtotal);
        double discount = discountCalculator.CalculateDiscount(subtotal);
        double shippingCost = CalculateShippingCost(products);
        double total = subtotal + tax - discount + shippingCost;
        int loyaltyPoints = CalculateLoyaltyPoints(products);

        return (subtotal, tax, discount, shippingCost, total, loyaltyPoints);
    }
}