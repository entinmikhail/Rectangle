using System.Collections.Generic;
using Rectangle.Abstraction;
using Rectangle.Model;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Rectangle.Controller
{
    public class LevelManager : ILevelManager
    {
        private readonly ILevelModel _levelModel;
        private readonly IBindingManager _bindingManager;
        private readonly GameObject _rectanglePrefab;
        private readonly IGameInfo _gameInfo;
        
        private LineRenderer _lineToMouse;
        private GameObject _lineGo;
        
        private readonly IDictionary<ILevelObjectView, IRectangle> _levelObjects = new Dictionary<ILevelObjectView, IRectangle>();

        public LevelManager(ILevelModel levelModel, IGameInfo gameInfo)
        {
            _levelModel = levelModel;
            _gameInfo = gameInfo;
            _rectanglePrefab = gameInfo.RectanglePrefab;
            _bindingManager = new BindingManager(_levelModel, _gameInfo);
        }
        
        public void CreateRectangle(Vector3 position)
        {
            var model = new RectangleModel(position, _gameInfo);
            
            if (!_levelModel.IsCollision(model))
            {
                _levelModel.AddRectangle(model);
                
                var go = Object.Instantiate(_rectanglePrefab, 
                    model.PositionModel.CurPosition, Quaternion.identity);
                
                if (go.TryGetComponent(out SpriteRenderer result))
                    result.color = new Color(Random.value, Random.value, Random.value, 1f);

                _levelObjects.Add(go.GetComponent<ILevelObjectView>(), model);

                model.PositionChanged += _bindingManager.MoveBindings;
            }
        }

        public void MoveRectangle(ILevelObjectView view, Vector3 newPosition)
        {
            var model = _levelObjects[view];
            var prevPosition = model.PositionModel.CurPosition;
            model.PositionModel.SetPosition(newPosition);
            
            if (_levelModel.IsCollision(model))
            {
                model.PositionModel.SetPosition(prevPosition);
                return;
            }
            
            view.Transform.position = newPosition;
        }
        
        public void DestroyRectangle(ILevelObjectView view)
        {
            _levelObjects[view].PositionChanged -= _bindingManager.MoveBindings;
            _levelModel.RemoveRectangleModel(_levelObjects[view]);
            _levelObjects.Remove(view);
            
            Object.Destroy(view.Transform.gameObject);
        }

        public void CreateBinding(ILevelObjectView firstView, ILevelObjectView secondView)
        {
            _bindingManager.CreateBinding(_levelObjects[firstView], _levelObjects[secondView]);
        }

        public void CreateLineToMousePosition(Vector3 position, Vector3 mousePosition)
        {
            _bindingManager.CreateLineToMousePosition(position, mousePosition);
        }

        public void DestroyLineToMousePosition()
        {
            _bindingManager.DestroyLineToMousePosition();
        }
        
        public void MoveLineMouse(Vector3 mousePosition)
        {
            _bindingManager.MoveLineMouse(mousePosition);
        }
    }
}