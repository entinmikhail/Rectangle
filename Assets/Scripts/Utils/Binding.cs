using Rectangle.Abstraction;

namespace Rectangle.Utils
{
    public readonly struct Binding : IRectangleBinding
    {
        public IRectangle FirstModel { get; }
        public IRectangle SecondModel { get; }
        
        public Binding(IRectangle firstModel, IRectangle secondModel)
        {
            FirstModel = firstModel;
            SecondModel = secondModel;
        }
        public IRectangleBinding GetReversBinding()
        {
            return new Binding(SecondModel, FirstModel);
        }
    }
}