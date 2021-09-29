using System;
using System.Collections.Generic;
using Rectangle.Abstraction;
using UnityEngine;

namespace Rectangle.Model
{
    public class LevelModel
    {
        public GameMode GameMode => _gameMode;
        
        private GameMode _gameMode;
        private List<IRectangle> _rectanglesOnMap = new List<IRectangle>();
        private IList<Binding> _rectanglesBindings = new List<Binding>();
        private Bounds _levelBounds = new Bounds(Vector3.zero, new Vector3(16f, 9f, 0));
        
        public event Action<Binding> BindingRemoved;
        public event Action<Binding> BindingCreated;
        
        public IList<Binding>  GetRectanglesBindings() => _rectanglesBindings;
        
        public bool IsCollision(IRectangle model)
        {
            if (!_levelBounds.Contains(model.PositionModel.CurPosition)) return false;
            
            foreach (var rectangle in _rectanglesOnMap)
            {
                if (rectangle == model) continue;
                if (model.PositionModel.Bounds.Intersects(rectangle.PositionModel.Bounds)) return false;
            }
            
            return true;
        }

        public void SetGameState(GameMode newGameMode)
        {
            _gameMode = newGameMode;
        }
        
        public void AddModel(IRectangle model)
        {
            _rectanglesOnMap.Add(model);
        }
        
        public void RemoveRectangleModel(IRectangle model)
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

        private void RemoveAllBindingModel(IRectangle firstModel)
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

    public enum GameMode
    {
        Default,
        Binding
    }
}