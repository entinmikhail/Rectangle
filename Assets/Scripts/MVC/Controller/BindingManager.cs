using System.Collections.Generic;
using Rectangle.Abstraction;
using Rectangle.Utils;
using UnityEngine;

namespace Rectangle.Controller
{
     public class BindingManager : IBindingManager
    {
        private readonly ILevelModel _levelModel;
        
        private readonly Material _material;
        private LineRenderer _lineToMouse;
        private GameObject _lineGo;
        
        private readonly IDictionary<IRectangleBinding, LineRenderer> _bindingsGo = new Dictionary<IRectangleBinding, LineRenderer>();
        private readonly IDictionary<IRectangle, IList<IRectangleBinding> > _rectangleBindings = new Dictionary<IRectangle, IList<IRectangleBinding>>();

        
        public BindingManager(ILevelModel levelModel, IGameInfo gameInfo)
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
        
        private void CreateBindingLine(IRectangleBinding binding)
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

        private void MoveBindingLine(IRectangleBinding binding)
        {
            _bindingsGo[binding].SetPositions(GetCurBindingPosition(binding).ToArray());
        }

        private void DestroyBindingLine(IRectangleBinding binding)
        {
            Object.Destroy(_bindingsGo[binding].gameObject);
            _bindingsGo.Remove(binding);
        }

        private List<Vector3> GetCurBindingPosition(IRectangleBinding binding)
        {
            var list = new List<Vector3>();
            
            list.Add(binding.FirstModel.PositionModel.CurPosition);
            list.Add(binding.SecondModel.PositionModel.CurPosition);
            
            return list;
        }

        private void OnRectangleAdded(IRectangle model)
        {
            _rectangleBindings.Add(model, new List<IRectangleBinding>());
        }
        
        private void OnRectangleRemoved(IRectangle model)
        {
            var tmp = new Dictionary<IRectangle, IRectangleBinding>();
            
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
            _levelModel.RectangleRemoved += OnRectangleRemoved;
            _levelModel.RectangleAdded += OnRectangleAdded;
            _levelModel.BindingCreated += CreateBindingLine;
            _levelModel.BindingRemoved += DestroyBindingLine;
        }
        
        private void Detach()
        {
            _levelModel.RectangleRemoved -= OnRectangleRemoved;
            _levelModel.RectangleAdded -= OnRectangleAdded;
            _levelModel.BindingCreated -= CreateBindingLine;
            _levelModel.BindingRemoved -= DestroyBindingLine;
        }
        
        public void Dispose()
        {
            Detach();
        }
    }
}