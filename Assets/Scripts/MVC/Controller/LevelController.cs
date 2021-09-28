using System.Collections.Generic;
using Rectangle.Abstraction;
using Rectangle.Model;
using Rectangle.View;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Rectangle.Core
{
    public class LevelController : ILevelController
    {
        private IDictionary<ILevelObjectView, IRectangle> _levelObjects = new Dictionary<ILevelObjectView, IRectangle>();
        private IDictionary<Binding, LineRenderer> _bindingsGo = new Dictionary<Binding, LineRenderer>();
        private LevelModel _levelModel;
        private GameObject _rectanglePrefab;
        private Material _material;
        
        public LevelController()
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

        public void CreateRectangle(IRectangle model)
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
                
                _levelObjects.Add(go.GetComponent<LevelObjectView>(), model);
            }
        }

        public void MoveRectangle(ILevelObjectView view, Vector3 newPosition)
        {
            var model = _levelObjects[view];
            var prevPosition = model.PositionModel.CurPosition;
            model.PositionModel.SetPosition(newPosition);
            
            if (!_levelModel.IsCollision(model))
            {
                model.PositionModel.SetPosition(prevPosition);
                return;
            }
            
            view.Transform.position = newPosition;
        }

        public void DestroyRectangle(ILevelObjectView view)
        {
            _levelModel.RemoveRectangleModel(_levelObjects[view]);
            _levelObjects.Remove(view);
            
            Object.Destroy(view.Transform.gameObject);
        }

        public void CreateBinding(ILevelObjectView firstView, ILevelObjectView secondView)
        {
            var binding = new Binding(_levelObjects[firstView], _levelObjects[secondView]);
            _levelModel.CreateBindingModel(binding);
        }
        
        private void DrawRay()
        {
           var allBindings = _levelModel.GetRectanglesBindings();

           foreach (var binding in allBindings)
           {
               MoveBindingLine(binding);
           }
        }

        private void CreateBindingLine(Binding binding)
        {
            var go = new GameObject();
            var line = go.AddComponent<LineRenderer>();
            
            go.name = "BindingLine";
            line.startWidth = 0.2f;
            line.material = _material;
            line.material.color = new Color(Random.value, Random.value, Random.value, 1f);
            line.SetPositions(GetCurBindingPosition(binding).ToArray());
            
            _bindingsGo.Add(binding, line);
        }

        private void MoveBindingLine(Binding binding)
        {
            _bindingsGo[binding].SetPositions(GetCurBindingPosition(binding).ToArray());
        }

        private void DestroyBindingLine(Binding binding)
        {
            Object.Destroy(_bindingsGo[binding].gameObject);
            _bindingsGo.Remove(binding);
        }

        private List<Vector3> GetCurBindingPosition(Binding binding)
        {
            var list = new List<Vector3>();
            list.Add(binding.FirstModel.PositionModel.CurPosition);
            list.Add(binding.SecondModel.PositionModel.CurPosition);
            
            return list;
        }
    }
}