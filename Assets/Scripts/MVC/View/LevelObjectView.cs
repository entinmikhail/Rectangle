using Rectangle.Abstraction;
using UnityEngine;

namespace Rectangle.View
{
    public class LevelObjectView : MonoBehaviour, ILevelObjectView
    {
        [SerializeField] private Transform _transform;
        public Transform Transform => _transform;
        public string Tag => tag;
    }
}