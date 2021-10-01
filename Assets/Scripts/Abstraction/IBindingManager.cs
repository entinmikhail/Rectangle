using UnityEngine;

namespace Rectangle.Abstraction
{
    public interface IBindingManager
    {
        void MoveBindings(IRectangle model);
        void CreateBinding(IRectangle firstModel, IRectangle secondModel);
        void CreateLineToMousePosition(Vector3 position, Vector3 mousePosition);
        void DestroyLineToMousePosition();
        void MoveLineMouse(Vector3 mousePosition);
    }
}