using Rectangle.Model;
using Rectangle.View;
using UnityEngine;

namespace Rectangle.Controller
{
    public class RectangleController
    {
        private LevelObjectView _view;
        private RectangleModel _rectangle;

        public RectangleController(LevelObjectView view, RectangleModel rectangle)
        {
            _view = view;
            _rectangle = new RectangleModel(view.Transform.position);
        }

        public void OnStart()
        {
        }

        public void OnUpdate()
        {
            
        }
    }
}