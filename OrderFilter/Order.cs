public class Order
{
    public double Price { get; set; }
    public int Size { get; set; }
    public string Symbol { get; set; }

    public static int ToNumber(string input)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }
        return 0; // Default value if parsing fails
    }
}