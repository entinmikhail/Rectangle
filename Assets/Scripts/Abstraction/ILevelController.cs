using UnityEngine;

namespace Rectangle.Abstraction
{
    public interface ILevelController
    {
        void OnInit();
        void OnUpdate();
        void CreateRectangle(IRectangle model);
        void MoveRectangle(ILevelObjectView go, Vector3 newPosition);
        void DestroyRectangle(ILevelObjectView go);
        void CreateBinding(ILevelObjectView firstGo, ILevelObjectView secondGo);
    }
}