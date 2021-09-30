using System;
using UnityEngine;

namespace Rectangle.InputManager
{
    public class InputHandler 
    {
        public event Action ButtonDown;
        public event Action ButtonDrag;
        public event Action ButtonUp;

        public void OnUpdate()
        {
            InputUpdate();
        }

        public Vector3 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public GameObject GetHitedObject()
        {
            var hit  = Physics2D.Raycast(GetMousePosition(), Vector2.zero);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        public Vector3 GetIndet()
        {
            var hit = Physics2D.Raycast(GetMousePosition(), Vector2.zero);
            var hitPoint = new Vector3(hit.point.x, hit.point.y, 0);
            return hit.transform.position - hitPoint;
        }
    
        private void InputUpdate()
        {
            if (Input.GetButtonDown("Fire1")) ButtonDown?.Invoke();

            if (Input.GetButton("Fire1")) ButtonDrag?.Invoke();

            if (Input.GetButtonUp("Fire1")) ButtonUp?.Invoke();
        }
    }
}