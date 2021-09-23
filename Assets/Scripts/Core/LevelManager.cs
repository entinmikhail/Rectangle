using System.Collections.Generic;
using Rectangle.Model;
using UnityEngine;

namespace Rectangle.Core
{
    public class LevelManager 
    {
        
        private Dictionary<GameObject, RectangleModel> _levelObjects = new Dictionary<GameObject, RectangleModel>();
        private LevelModel _levelModel;
        private GameObject _rectanglePrefab;

        public LevelManager()
        {
            _levelModel = new LevelModel();
            _rectanglePrefab = Resources.Load<GameObject>("Rectangle");
        }

        public void OnUpdate()
        {
            DrawRay();
        }

        private void DrawRay()
        {
           var allBindings = _levelModel.GetRectanglesBindings();

           foreach (var bindings in allBindings)
           {
               foreach (var binding in bindings.Value)
               {
                   Debug.DrawLine(binding.PositionModel.CurPosition, bindings.Key.PositionModel.CurPosition, Color.red, Time.deltaTime);
               }
           }
        }
        
        public void CreateRectangle(RectangleModel model)
        {
            if (_levelModel.IsCollision(model))
            {
                _levelModel.AddModel(model);
                var go = Object.Instantiate(_rectanglePrefab, model.PositionModel.CurPosition, Quaternion.identity);
                
                if (go.TryGetComponent(out SpriteRenderer result))
                {
                    result.color = new Color(Random.value, Random.value, Random.value, 1);
                }
                
                _levelObjects.Add( go, model);
            }
        }

        public void DestroyRectangle(GameObject go)
        {
            _levelModel.RemoveRectangleModel(_levelObjects[go]);
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
            _levelModel.CreateBindingModel(_levelObjects[firstGo], _levelObjects[secondGo]);
        }
    }

    public class LevelModel
    {
        private List<RectangleModel> _rectanglesOnMap = new List<RectangleModel>();

        private Dictionary<RectangleModel, List<RectangleModel>> _rectanglesBindings =
            new Dictionary<RectangleModel, List<RectangleModel>>();
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

        public void CreateBindingModel(RectangleModel firstModel, RectangleModel secondModel)
        {
            
            
            if (!_rectanglesBindings.ContainsKey(firstModel))
            {
                _rectanglesBindings.Add(firstModel, new List<RectangleModel>());
            }
            _rectanglesBindings[firstModel].Add(secondModel);
        }
        
        public void RemoveAllBindingModel(RectangleModel firstModel)
        {
            _rectanglesBindings.Remove(firstModel);
        }

        public Dictionary<RectangleModel, List<RectangleModel>> GetRectanglesBindings() => _rectanglesBindings;
    }
}