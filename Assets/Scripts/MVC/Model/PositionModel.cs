using Rectangle.Abstraction;
using Rectangle.ScriptableObjects;
using UnityEngine;

namespace Rectangle.Model
{
    public class PositionModel : IPositionModel
    {
        public Vector3 CurPosition => _curPosition;
        public Bounds Bounds { get; set; }
        
        private Vector3 _curPosition;
        private Vector3 _size;
        
        public PositionModel(Vector3 curPosition, GameInfo gameInfo)
        {
            _curPosition = curPosition;
            _size = gameInfo.SpriteSize;
            Bounds = new Bounds(_curPosition, _size);
        }
        
        public void SetPosition(Vector3 newPosition)
        {
            _curPosition = newPosition;
            Bounds = new Bounds(_curPosition, _size);
        }
    }
}