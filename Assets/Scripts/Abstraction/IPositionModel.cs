using System;
using UnityEngine;

namespace Rectangle.Abstraction
{
    public interface IRectangle
    {
        IPositionModel PositionModel { get; }
        event Action<IRectangle> CreatedRectangle;
        event Action<IRectangle> RemovedRectangle;
        event Action<IRectangle, IRectangle> BindedRectangle;
        public void Destroy();
        public void CreateBinding(IRectangle rectangleModel);
    }

    public interface IPositionModel
    {
        Vector3 CurPosition { get; }
        public Bounds Bounds { get; set; }
        
        void SetPosition(Vector3 newPosition);
    }
}