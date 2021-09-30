using UnityEngine;

namespace Rectangle.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Gameplay/GameInfo", fileName = "GameInfo")]
    public class GameInfo : ScriptableObject
    {
        public Vector3 SpriteSize  =>_rectanglePrefab.GetComponent<SpriteRenderer>().size;
        public GameObject RectanglePrefab => _rectanglePrefab;
        public float TimeDelayForDoubleClick => _timeDelayForDoubleClick;
        public Material BindingLineMaterial => _bindingLineMaterial;
        public Bounds LevelBounds => new Bounds(Vector3.zero, _levelBounds);
           
        [SerializeField] private GameObject _rectanglePrefab;
        [SerializeField] private float _timeDelayForDoubleClick;
        [SerializeField] private Material _bindingLineMaterial;
        [SerializeField] private Vector3 _levelBounds;

    }
}