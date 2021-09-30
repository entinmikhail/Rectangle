using System;
using System.Collections.Generic;
using Rectangle.Abstraction;
using Rectangle.ScriptableObjects;
using UnityEngine;

namespace Rectangle.Model
{
    public class RectangleModel : IRectangle
    {
        public IPositionModel PositionModel { get; }

        private List<IRectangle> _rectangles = new List<IRectangle>();
        
        public event Action<IRectangle> CreatedRectangle;
        public event Action<IRectangle> RemovedRectangle;
        public event Action<IRectangle, IRectangle> BindedRectangle;
        
        public RectangleModel(Vector3 curPosition, GameInfo gameInfo)
        {
            PositionModel = new PositionModel(curPosition, gameInfo);
            CreatedRectangle?.Invoke(this);
        }

        public void Destroy()
        {
            RemovedRectangle?.Invoke(this);
        }

        public void CreateBinding(IRectangle rectangleModel)
        {
            if(_rectangles.Contains(rectangleModel)) return;
            
            _rectangles.Add(rectangleModel);
            rectangleModel.CreateBinding(this);
            
            BindedRectangle?.Invoke(this, rectangleModel);
        }
    }
}

