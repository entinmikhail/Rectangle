namespace Rectangle.Abstraction
{
    public interface IRectangleBinding
    {
        IRectangle FirstModel { get; }
        IRectangle SecondModel { get; }
        IRectangleBinding GetReversBinding();
    }
}