using System;
using System.Collections.Generic;
using Rectangle.Model;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Rectangle.Core
{
   

    public class LevelManager 
    {
        private IDictionary<GameObject, RectangleModel> _levelObjects = new Dictionary<GameObject, RectangleModel>();
        private IDictionary<Binding, GameObject> _bindingsGo = new Dictionary<Binding, GameObject>();
        private LevelModel _levelModel;
        private GameObject _rectanglePrefab;
        private Material _material;

        
        public event Action<RectangleModel> BindingDestoyed;
        public LevelManager()
        {
            _levelModel = new LevelModel();
            _rectanglePrefab = Resources.Load<GameObject>("Rectangle");
            _material = Resources.Load<Material>("Default");
        }

        public void OnInit()
        {
            _levelModel.BindingCreated += CreateBindingLine;
            _levelModel.BindingRemoved += DestroyBindingLine;
        }

        public void OnUpdate()
        {
            DrawRay();
        }

        private void DrawRay()
        {
           var allBindings = _levelModel.GetRectanglesBindings();

           foreach (var binding in allBindings)
           {
               MoveBindingLine(binding);
           }
        }
        
        public void CreateRectangle(RectangleModel model)
        {
            if (_levelModel.IsCollision(model))
            {
                _levelModel.AddModel(model);
                var go = Object.Instantiate(_rectanglePrefab, 
                    model.PositionModel.CurPosition, Quaternion.identity);
                
                if (go.TryGetComponent(out SpriteRenderer result))
                {
                    result.color = new Color(Random.value, Random.value, Random.value, 1f);
                }
                
                _levelObjects.Add(go, model);
            }
        }

        public void CreateBindingLine(Binding binding)
        {
            var go = Object.Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
            var line = go.AddComponent<LineRenderer>();
            
            line.startWidth = 0.2f;
            line.material = _material;
            line.material.color = new Color(Random.value, Random.value, Random.value, 1f);
            
            line.SetPositions(GetCurBintingPosition(binding).ToArray());
            
            _bindingsGo.Add(binding, go);
        }
        
        public void MoveBindingLine(Binding binding)
        {
            var go = _bindingsGo[binding];
            var line = go.GetComponent<LineRenderer>();
            
            line.SetPositions(GetCurBintingPosition(binding).ToArray());
        }

        public void DestroyBindingLine(Binding binding)
        {
            var go = _bindingsGo[binding];
            _bindingsGo.Remove(binding);
            
            Object.Destroy(go);
        }

        public void DestroyRectangle(GameObject go)
        {
            _levelModel.RemoveRectangleModel(_levelObjects[go]);
            
            BindingDestoyed?.Invoke(_levelObjects[go]);
            
            _levelObjects.Remove(go);
            
            Object.Destroy(go);
        }

        public void MoveRectangle(GameObject go, Vector3 newPosition)
        {
            var model = _levelObjects[go];
            var prevPosition = model.PositionModel.CurPosition;
            model.PositionModel.SetPosition(newPosition);
            if (!_levelModel.IsCollision(model))
            {
                model.PositionModel.SetPosition(prevPosition);
                return;
            }
            
            go.transform.position = newPosition;
        }

        public void CreateBinding(GameObject firstGo, GameObject secondGo)
        {
            var binding = new Binding(_levelObjects[firstGo], _levelObjects[secondGo]);
            _levelModel.CreateBindingModel(binding);
        }
        
        private List<Vector3> GetCurBintingPosition(Binding binding)
        {
            var list = new List<Vector3>();
            list.Add(binding.FirstModel.PositionModel.CurPosition);
            list.Add(binding.SecondModel.PositionModel.CurPosition);
            
            return list;
        }
    }

    public class LevelModel
    {
        private List<RectangleModel> _rectanglesOnMap = new List<RectangleModel>();
        
        
        private IList<Binding> _rectanglesBindings = new List<Binding>();
        
        public IList<Binding>  GetRectanglesBindings() => _rectanglesBindings;

        public event Action<Binding> BindingRemoved;
        public event Action<Binding> BindingCreated;
        
        public bool IsCollision(RectangleModel model)
        {
            foreach (var rectangle in _rectanglesOnMap)
            {
                if (rectangle == model) continue;
                if (model.PositionModel.Bounds.Intersects(rectangle.PositionModel.Bounds)) return false;
            }
            return true;
        }

        public void AddModel(RectangleModel model)
        {
            _rectanglesOnMap.Add(model);
        }
        public void RemoveRectangleModel(RectangleModel model)
        {
            RemoveAllBindingModel(model);
            _rectanglesOnMap.Remove(model);
        }

        public void CreateBindingModel(Binding binding)
        {
            if (_rectanglesBindings.Contains(binding) || _rectanglesBindings.Contains(binding.GetReversBinding()))
                return;
            
            _rectanglesBindings.Add(binding);
            BindingCreated?.Invoke(binding);
        }
        
        
        public void RemoveAllBindingModel(RectangleModel firstModel)
        {
            IList<Binding> tmp = new List<Binding>();
            
            foreach (var binding in _rectanglesBindings)
            {
                tmp.Add(binding);
            }
            
            foreach (var binding in tmp)
            {
                if (binding.FirstModel == firstModel || binding.SecondModel == firstModel)
                {
                    BindingRemoved?.Invoke(binding);
                    _rectanglesBindings.Remove(binding);
                }
            }
        }
    }
}