using System;
using Rectangle.Abstraction;
using UnityEngine;

namespace Rectangle.Input
{
    public class InputManager : IInputManager
    {
        private readonly Camera _camera;
        
        public event Action ButtonDown;
        public event Action ButtonDrag;
        public event Action ButtonUp;
        public event Action RightButtonDown;

        public InputManager()
        {
            _camera = Camera.main;
        }

        public void OnUpdate()
        {
            InputUpdate();
        }

        public Vector3 GetMousePosition()
        {
            return _camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        }

        public ILevelObjectView GetHitedObject()
        {
            var hit  = Physics2D.Raycast(GetMousePosition(), Vector2.zero);
            
            return hit.collider != null ? hit.collider.gameObject.GetComponent<ILevelObjectView>() : null;
        }

        public Vector3 GetIndent()
        {
            var hit = Physics2D.Raycast(GetMousePosition(), Vector2.zero);
            var hitPoint = new Vector3(hit.point.x, hit.point.y, 0);
            
            return hit.transform.position - hitPoint;
        }
    
        private void InputUpdate()
        {
            if (UnityEngine.Input.GetButtonDown("Fire1")) ButtonDown?.Invoke();
            
            if (UnityEngine.Input.GetButton("Fire1")) ButtonDrag?.Invoke();
            
            if (UnityEngine.Input.GetButtonUp("Fire1")) ButtonUp?.Invoke();
            
            if (UnityEngine.Input.GetButtonUp("Fire2")) RightButtonDown?.Invoke();
        }
    }
}