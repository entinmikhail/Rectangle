using UnityEngine;

namespace Rectangle.Abstraction
{
    public interface ILevelManager
    {
        void CreateRectangle(Vector3 position);
        void MoveRectangle(ILevelObjectView go, Vector3 newPosition);
        void DestroyRectangle(ILevelObjectView go);
        void CreateBinding(ILevelObjectView firstGo, ILevelObjectView secondGo);
        void CreateLineToMousePosition(Vector3 position, Vector3 mousePosition);
        void DestroyLineToMousePosition();
        void MoveLineMouse(Vector3 mousePosition);
    }
}