using System.Collections.Generic;
using Rectangle.Controller;
using Rectangle.Model;
using Rectangle.View;
using UnityEngine;

namespace Rectangle.Core
{
    public class LevelManager 
    {
        
        private Dictionary<RectangleController, LevelObjectView> _levelObject = new Dictionary<RectangleController, LevelObjectView>();
        private LevelModel _levelModel;
        private GameObject _rectanglePrefab;

        public LevelManager()
        {
            _levelModel = new LevelModel();
            _rectanglePrefab = Resources.Load<GameObject>("Rectangle");

        }

        public void CreateRectangle(RectangleModel model)
        {
            if (_levelModel.CreateNewRectangleModel(model))
            {
                var go = Object.Instantiate(_rectanglePrefab, model.PositionModel.CurPosition, Quaternion.identity);
                if (!go.TryGetComponent(out SpriteRenderer result))
                {
                    
                }
                result.color = new Color(Random.value, Random.value, Random.value, 1);
            }
        }
    }

    public class LevelModel
    {
        private Dictionary<RectangleModel, RectangleModel> _rectanglesBindings = new Dictionary<RectangleModel, RectangleModel>();
        private List<RectangleModel> _rectangles = new List<RectangleModel>();

        public bool CreateNewRectangleModel(RectangleModel model)
        {
            foreach (var rectangle in _rectangles)
            {
                if (rectangle == model) continue;
                if (model.PositionModel.Bounds.Intersects(rectangle.PositionModel.Bounds)) return false;
            }
            _rectangles.Add(model);
            return true;
        }
    }
}