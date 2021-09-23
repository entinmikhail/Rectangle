using UnityEngine;

namespace Rectangle.View
{
    public class LevelObjectView : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        public Transform Transform => _transform;
    }
}