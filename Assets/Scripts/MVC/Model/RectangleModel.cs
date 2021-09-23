using System;
using System.Collections.Generic;
using Abstraction;
using UnityEngine;

namespace Rectangle.Model
{
    public class RectangleModel : IRectangle
    {
        public PositionModel PositionModel;

        private List<RectangleModel> _rectangles = new List<RectangleModel>();
        
        public event Action<RectangleModel> CreatedRectangle;
        public event Action<RectangleModel> RemovedRectangle;
        public event Action<RectangleModel, RectangleModel> BindedRectangle;
        
        public RectangleModel(Vector3 curPosition)
        {
            PositionModel = new PositionModel(curPosition);
            CreatedRectangle?.Invoke(this);
        }

        public void Destroy()
        {
            RemovedRectangle?.Invoke(this);
        }

        public void CreateBinding(RectangleModel rectangleModel)
        {
            if(_rectangles.Contains(rectangleModel)) return;
            
            _rectangles.Add(rectangleModel);
            rectangleModel.CreateBinding(this);
            BindedRectangle?.Invoke(this, rectangleModel);
        }

        public bool IsColision(RectangleModel rectangle)
        {
           return rectangle.PositionModel.Bounds.Intersects(rectangle.PositionModel.Bounds);
        }
        
        
    }
    
    public class PositionModel : IPositionModel
    {
        public Vector3 CurPosition => _curPosition;
        public Bounds Bounds;
        
        private Vector3 _curPosition;
        private Vector3 _size = new Vector3(2, 1, 0);
        
        public PositionModel(Vector3 curPosition)
        {
            _curPosition = curPosition;
            Bounds = new Bounds(_curPosition, _size);
        }
        
        public void SetPosition(Vector3 newPosition)
        {
            
            _curPosition = newPosition;
            Bounds = new Bounds(_curPosition, _size);
        }
    }
}

