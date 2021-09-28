namespace Rectangle.Model
{
    public struct Binding
    {
        public RectangleModel FirstModel;
        public RectangleModel SecondModel;

        public Binding(RectangleModel firstModel, RectangleModel secondModel)
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