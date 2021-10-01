using System;
using UnityEngine;

namespace Rectangle.Abstraction
{
    public interface IInputManager
    {
        public event Action ButtonDown;
        public event Action ButtonDrag;
        public event Action ButtonUp;
        public event Action RightButtonDown;
        void OnUpdate();
        Vector3 GetMousePosition();
        ILevelObjectView GetHitedObject();
        Vector3 GetIndent();
    }
}