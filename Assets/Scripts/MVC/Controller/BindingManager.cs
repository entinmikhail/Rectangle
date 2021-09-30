using System.Collections.Generic;
using Rectangle.Abstraction;
using Rectangle.Model;
using Rectangle.ScriptableObjects;
using UnityEngine;

namespace Rectangle.Controller
{
     public class BindingManager
    {
        private LevelModel _levelModel;
        private Material _material;
        private LineRenderer _lineToMouse;
        private GameObject _lineGo;
        
        private IDictionary<Binding, LineRenderer> _bindingsGo = new Dictionary<Binding, LineRenderer>();
        private IDictionary<IRectangle, IList<Binding> > _rectangleBindings = new Dictionary<IRectangle, IList<Binding>>();

        
        public BindingManager(LevelModel levelModel, GameInfo gameInfo)
        {
            _levelModel = levelModel;
            _material = gameInfo.BindingLineMaterial;
            
            Attach();
        }

        public void MoveBindings(IRectangle model)
        {
            foreach (var binding in _rectangleBindings[model])
            {
                MoveBindingLine(binding);
            }
        }

        public void CreateBinding(IRectangle firstModel, IRectangle secondModel)
        {
            if (firstModel == secondModel) return;
            
            var firstBinding = new Binding(firstModel, secondModel);
            var secondBinding = firstBinding.GetReversBinding();
            
            if (_rectangleBindings[firstModel].Contains(firstBinding)
                || _rectangleBindings[firstModel].Contains(secondBinding)) return;

            _rectangleBindings[firstModel].Add(firstBinding);
            _rectangleBindings[secondModel].Add(firstBinding);
            _levelModel.CreateBindingModel(firstBinding);
        }

        public void CreateLineToMousePosition(Vector3 position, Vector3 mousePosition)
        {
            _lineToMouse = CreateLineRenderer();
            _lineToMouse.SetPositions(new []{position, mousePosition});
        }

        public void DestroyLineToMousePosition()
        {
            Object.Destroy(_lineToMouse.gameObject);
        }
        
        public void MoveLineMouse(Vector3 mousePosition)
        {
            if (_lineToMouse != null)
            {
                mousePosition.z = 0f;
                _lineToMouse.SetPosition(1, mousePosition);
            }
        }
        
        private void CreateBindingLine(Binding binding)
        {
            var line = CreateLineRenderer();
            line.material.color = _lineToMouse.material.color;
            line.SetPositions(GetCurBindingPosition(binding).ToArray());
            
            _bindingsGo.Add(binding, line);
        }
        
        private LineRenderer CreateLineRenderer()
        {
            var go = new GameObject();
            var line = go.AddComponent<LineRenderer>();

            go.name = "BindingLine";
            line.startWidth = 0.2f;
            line.material = _material;
            line.material.color = new Color(Random.value, Random.value, Random.value, 1f);
            return line;
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

        private void OnRectangleAdded(IRectangle model)
        {
            _rectangleBindings.Add(model, new List<Binding>());
        }
        
        private void OnRectangleRemoved(IRectangle model)
        {
            var tmp = new Dictionary<IRectangle, Binding>();
            
            foreach (var binding in _rectangleBindings[model])
            {
                if (binding.FirstModel == model)
                    tmp.Add(binding.SecondModel, binding);
                else
                    tmp.Add(binding.FirstModel, binding);
            }
            _rectangleBindings.Remove(model);
            
            foreach (var rectangleModel in tmp)
            {
                _rectangleBindings[rectangleModel.Key].Remove(rectangleModel.Value);
            }
        }
        
        private void Attach()
        {
            _levelModel.RectanglRemoved += OnRectangleRemoved;
            _levelModel.RectanglAdded += OnRectangleAdded;
            _levelModel.BindingCreated += CreateBindingLine;
            _levelModel.BindingRemoved += DestroyBindingLine;
        }
        
        private void Detach()
        {
            _levelModel.RectanglRemoved -= OnRectangleRemoved;
            _levelModel.RectanglAdded -= OnRectangleAdded;
            _levelModel.BindingCreated -= CreateBindingLine;
            _levelModel.BindingRemoved -= DestroyBindingLine;
        }
        
        public void Dispose()
        {
            Detach();
        }
    }
}