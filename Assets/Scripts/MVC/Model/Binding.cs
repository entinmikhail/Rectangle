using Rectangle.Abstraction;

namespace Rectangle.Model
{
    public struct Binding
    {
        public IRectangle FirstModel;
        public IRectangle SecondModel;

        public Binding(IRectangle firstModel, IRectangle secondModel)
        {
            FirstModel = firstModel;
            SecondModel = secondModel;
        }
        
        public Binding GetReversBinding()
        {
            return new Binding(SecondModel, FirstModel);
        }
    }
}