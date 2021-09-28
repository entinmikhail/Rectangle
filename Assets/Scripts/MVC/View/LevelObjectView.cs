using Rectangle.Abstraction;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rectangle.View
{
    public class LevelObjectView : MonoBehaviour, ILevelObjectView, IPointerClickHandler
    {
        [SerializeField] private Transform _transform;
        public Transform Transform => _transform;
        
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            
        }
    }
}