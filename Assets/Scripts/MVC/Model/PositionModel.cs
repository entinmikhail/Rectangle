using Rectangle.Abstraction;
using UnityEngine;

namespace Rectangle.Model
{
    public class PositionModel : IPositionModel
    {
        public Vector3 CurPosition => _curPosition;
        public Bounds Bounds { get; set; }
        
        private Vector3 _curPosition;
        private Vector3 _size;
        
        public PositionModel(Vector3 curPosition)
        {
            _curPosition = curPosition;
            SpriteRenderer sprite = Resources.Load<GameObject>("Rectangle").GetComponent<SpriteRenderer>();
            _size = sprite.size;
            Bounds = new Bounds(_curPosition, _size);
        }
        
        public void SetPosition(Vector3 newPosition)
        {
            _curPosition = newPosition;
            Bounds = new Bounds(_curPosition, _size);
        }
    }
}