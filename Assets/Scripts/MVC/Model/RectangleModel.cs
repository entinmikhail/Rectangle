using System;
using System.Collections.Generic;
using Abstraction;
using UnityEngine;

namespace Rectangle.Model
{
    public class RectangleModel : IRectangle
    {
        public PositionModel PositionModel;
        private FigureInfo _figureInfo;
        
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

    public class FigureInfo : ScriptableObject
    {
        public float Length => _length;
        [SerializeField] private float _length;
    }
    
    public class PositionModel : IPositionModel
    {
        public Vector3 CurPosition => _curPosition;
        private Vector3 _curPosition;
        private Vector3 _firstPoint;
        private Vector3 _secondPoint;
        private FigureInfo _figureInfo;

        public Bounds Bounds;
        public PositionModel(Vector3 curPosition)
        {
            var height = 3 / 2;
            
            _curPosition = curPosition;
            
            _firstPoint = new Vector3(curPosition.x - height, curPosition.y + height, 0);
            _secondPoint = new Vector3(curPosition.x + height, curPosition.y - height, 0);
            
            Bounds = new Bounds(_curPosition, _firstPoint);
        }
        
        public void SetPosition(Vector3 newPosition)
        {
            _curPosition = newPosition;
        }
    }
}

