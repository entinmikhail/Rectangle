using Rectangle.Core;
using Rectangle.InputManager;
using Rectangle.Model;
using Rectangle.View;
using UnityEngine;

namespace Rectangle.Controller
{
    public class PlayerController
    {

        private LevelController _levelController;

        private GameObject _rectangle;
        private GameObject _firstGo;
        private GameModel _gameModel;
        private InputHandler _input;
    
        private GameObject _lastSelectedGo;
        private GameObject _firstSelectedGo;
        private GameObject _secondSelectedGo;
    
        private Vector3 _indent;
    
        private bool _isFirstClick = true;
        private bool _isFirstClickBinding = true;
        private bool _isSelected;
        private const float _timeDelay = 0.3f;
        private float _firstClickTime;
        private bool _isMouseLine;

        public PlayerController( LevelController levelController, GameModel gameModel, InputHandler input)
        {
            _levelController = levelController;
            _gameModel = gameModel;
            _input = input;
        }

        public void OnUpdate()
        {
            if (_isMouseLine)
            {
                _levelController.MoveLineMouse(_input.GetMousePosition());
            }
        }
        public void OnStart()
        {
            Attach();
        }
        
        private void OnButtonDrag()
        {
            MoveRectangle();
        }

        private void OnButtonUp()
        {
            ResetRectangleSelection();
        }
        
        private void OnButtonDown()
        {
            if (Time.time - _firstClickTime > _timeDelay) _isFirstClick = true;

            if (_gameModel.IsCreateMode)
            {
                if (_isFirstClick) CreateRectangle();
                else OnDoubleClick();
            }
            else
            {
                CreateBinding();
            }
        }

        private void CreateRectangle()
        {
            _firstSelectedGo = _input.GetHitedObject();
            _lastSelectedGo = _firstSelectedGo;
            _firstClickTime = Time.time;
            _isFirstClick = false;

            SelectOrCreateRectangle();
        }

        private void OnDoubleClick()
        {
            _secondSelectedGo = _input.GetHitedObject();
            _lastSelectedGo = _secondSelectedGo;

            if (_secondSelectedGo != null && _firstSelectedGo == _secondSelectedGo)
            {
                DestroyRectangle();
                _isFirstClick = true;
            }
            else
            {
                SelectOrCreateRectangle();
            }
        }

        private void SelectOrCreateRectangle()
        {
            var mousePosition = _input.GetMousePosition();
        
            if (IsRectangle())
            {
                _rectangle = _lastSelectedGo;
                _isSelected = true;
                _indent = _input.GetIndet();
            }
            else
            {
                if (_lastSelectedGo != null && _lastSelectedGo.CompareTag("UI")) return;
                
                mousePosition.z = 0;
                _levelController.CreateRectangle(mousePosition);
            }
        }

        private void MoveRectangle()
        {
            if (_isSelected && _rectangle != null)
            {
                var mousePosition = _input.GetMousePosition();
                mousePosition.z = 0f;
                
                _levelController.MoveRectangle(_rectangle.GetComponent<LevelObjectView>(), 
                    mousePosition + _indent);   
            }
        }

        private void ResetRectangleSelection()
        {
            if (_isSelected)
            {
                _isSelected = false;
                _rectangle = null;
            }
        }

        private void DestroyRectangle()
        {
            if (IsRectangle())
            {
                _isFirstClick = true;
                _levelController.DestroyRectangle(_lastSelectedGo.GetComponent<LevelObjectView>());
            }
        }
    
        private void CreateBinding()
        {
            if (_isFirstClickBinding)
            {
                if (IsRectangle())
                {
                    _firstGo = _lastSelectedGo;
                    _isFirstClickBinding = false;

                    _levelController.CreateLineToMousePosition(_firstGo.transform.position, _input.GetMousePosition());
                    _isMouseLine = true;
                }
            }
            else
            {
                _isFirstClickBinding = true;
                _levelController.DestroyLineToMousePosition();
                _isMouseLine = false;
            
                if (IsRectangle())
                {
                    var go = _lastSelectedGo;
                    _levelController.CreateBinding(_firstGo.GetComponent<LevelObjectView>(),
                        go.GetComponent<LevelObjectView>());
                }
            }
        }
        
        private bool IsRectangle()
        {
            _lastSelectedGo = _input.GetHitedObject();
        
            if (_lastSelectedGo)
            {
                if(_lastSelectedGo.CompareTag("Rectangle"))
                    return true;
            }

            return false;
        }

        public void Dispose()
        {
            Detach();
        }
        private void Attach()
        {
            _input.ButtonDown += OnButtonDown;
            _input.ButtonDrag += OnButtonDrag;
            _input.ButtonUp += OnButtonUp;
        }

        private void Detach()
        {
            _input.ButtonDown -= OnButtonDown;
            _input.ButtonDrag -= OnButtonDrag;
            _input.ButtonUp -= OnButtonUp;
        }
    }
}

