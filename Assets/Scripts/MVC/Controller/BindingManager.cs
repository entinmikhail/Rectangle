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
        private IDictionary<>
        
        public BindingManager(LevelModel levelModel, GameInfo gameInfo)
        {
            _levelModel = levelModel;
            _material = gameInfo.BindingLineMaterial;
            
            Attach();
        }

        public void MoveBinding(IRectangle model)
        {
             IDictionary<Binding, LineRenderer> tmp = new Dictionary<Binding, LineRenderer>();

            foreach (var bLineRenderer in _bindingsGo)
            {
                if (bLineRenderer.Key.FirstModel == model || bLineRenderer.Key.SecondModel == model)
                {
                    tmp.Add(bLineRenderer);
                }
            }

            foreach (var binding in tmp)
            {
                MoveBindingLine(binding.Key);
            }
        }

        public void CreateBinding(IRectangle firstModel, IRectangle secondModel)
        {
            if (firstModel == secondModel) return;

            var binding = new Binding(firstModel, secondModel);
            _levelModel.CreateBindingModel(binding);
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

        public void MoveAllBindingLine()
        {
           var allBindings = _levelModel.GetRectanglesBindings();

           foreach (var binding in allBindings)
           {
               MoveBindingLine(binding);
           }
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
        
        private void Attach()
        {
            _levelModel.BindingCreated += CreateBindingLine;
            _levelModel.BindingRemoved += DestroyBindingLine;
        }
        
        private void Detach()
        {
            _levelModel.BindingCreated -= CreateBindingLine;
            _levelModel.BindingRemoved -= DestroyBindingLine;
        }
        
        public void Dispose()
        {
            Detach();
        }
    }
}