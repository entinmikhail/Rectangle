using System;
using System.Collections.Generic;
using Rectangle.Abstraction;
using UnityEngine;

namespace Rectangle.Model
{
    public class LevelModel : ILevelModel
    {
        private readonly List<IRectangle> _rectanglesOnMap = new List<IRectangle>();
        private readonly IList<IRectangleBinding> _rectanglesBindings = new List<IRectangleBinding>();

        private Bounds _levelBounds;

        public event Action<IRectangle> RectangleRemoved;
        public event Action<IRectangle> RectangleAdded;
        public event Action<IRectangleBinding> BindingRemoved;
        public event Action<IRectangleBinding> BindingCreated;

        public LevelModel(Bounds levelBounds)
        {
            _levelBounds = levelBounds;
        }

        public bool IsCollision(IRectangle model)
        {
            if (!_levelBounds.Contains(model.PositionModel.CurPosition)) return true;
            
            foreach (var rectangle in _rectanglesOnMap)
            {
                if (rectangle == model) continue;
                if (model.PositionModel.Bounds.Intersects(rectangle.PositionModel.Bounds)) return true;
            }
            
            return false;
        }

        public void AddRectangle(IRectangle model)
        {
            _rectanglesOnMap.Add(model);

            RectangleAdded?.Invoke(model);
        }

        public void RemoveRectangleModel(IRectangle model)
        {
            RemoveAllBindingModel(model);
            _rectanglesOnMap.Remove(model);

            RectangleRemoved?.Invoke(model);
        }

        public void CreateBindingModel(IRectangleBinding binding)
        {
            if (_rectanglesBindings.Contains(binding) || _rectanglesBindings.Contains(binding.GetReversBinding()))
                return;

            _rectanglesBindings.Add(binding);

            BindingCreated?.Invoke(binding);
        }

        private void RemoveAllBindingModel(IRectangle firstModel)
        {
            IList<IRectangleBinding> tmp = new List<IRectangleBinding>();

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