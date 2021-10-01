using System;
using Rectangle.Abstraction;
using UnityEngine;

namespace Rectangle.Model
{
    public class PositionModel : IPositionModel
    {
        private readonly Vector3 _size;

        public Vector3 CurPosition { get; private set; }
        public Bounds Bounds { get; set; }
        
        public event Action PositionChanged;
        
        public PositionModel(Vector3 curPosition, IGameInfo gameInfo)
        {
            CurPosition = curPosition;
            _size = gameInfo.SpriteSize;
            Bounds = new Bounds(CurPosition, _size);
        }
        
        public void SetPosition(Vector3 newPosition)
        {
            CurPosition = newPosition;
            Bounds = new Bounds(CurPosition, _size);
            PositionChanged?.Invoke();
        }
    }
}