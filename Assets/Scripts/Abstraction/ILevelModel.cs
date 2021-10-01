using System;

namespace Rectangle.Abstraction
{
    public interface ILevelModel
    {
        event Action<IRectangle> RectangleRemoved;
        event Action<IRectangle> RectangleAdded;
        event Action<IRectangleBinding> BindingRemoved;
        event Action<IRectangleBinding> BindingCreated;
        bool IsCollision(IRectangle model);
        void AddRectangle(IRectangle model);
        void RemoveRectangleModel(IRectangle model);
        void CreateBindingModel(IRectangleBinding binding);
    }
}