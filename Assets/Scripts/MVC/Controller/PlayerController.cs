using Rectangle.Abstraction;
using UnityEngine;

namespace Rectangle.Controller
{
    public class PlayerController : IPlayerController
    {
        private readonly ILevelManager _levelManager;
        private readonly IInputManager _input;

        private ILevelObjectView _lastSelectedView;
        private ILevelObjectView _firstSelectedView;
        private ILevelObjectView _secondSelectedView;
        private ILevelObjectView _lastSelectedViewOnRightClick;
        private ILevelObjectView _firstSelectedViewOnRightClick;

        
        private readonly float _timeDelayForDoubleClick;
        private Vector3 _indent;
        private bool _isFirstClick = true;
        private bool _isFirstClickBinding = true;
        private bool _isSelected;
        private float _firstClickTime;
        private bool _isMouseLine;

        public PlayerController(ILevelManager levelManager, IInputManager input, IGameInfo gameInfo)
        {
            _timeDelayForDoubleClick = gameInfo.TimeDelayForDoubleClick;
            _levelManager = levelManager;
            _input = input;
        }

        public void Update()
        {
            if (_isMouseLine)
            {
                _levelManager.MoveLineMouse(_input.GetMousePosition());
            }
        }
        public void Init()
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
            if (Time.time - _firstClickTime > _timeDelayForDoubleClick)
            {
                _isFirstClick = true;
            }
            
            if (_isFirstClick) 
                CreateRectangle();
            else
                OnDoubleClick();
            
        }

        private void OnRightButtonDown()
        {
            CreateBinding();
        }
        

        private void CreateRectangle()
        {
            _firstSelectedView = _input.GetHitedObject();
            _lastSelectedView = _firstSelectedView;
            _firstClickTime = Time.time;
            
            _isFirstClick = false;

            SelectOrCreateRectangle();
        }

        private void OnDoubleClick()
        {
            _secondSelectedView = _input.GetHitedObject();
            _lastSelectedView = _secondSelectedView;
            
            _isSelected = false;
            
            if (_secondSelectedView != null && _firstSelectedView == _secondSelectedView)
            {
                DestroyRectangle();
            }
            else
            {
                CreateRectangle();
            }
        }

        private void SelectOrCreateRectangle()
        {
            _lastSelectedView = _input.GetHitedObject();
            
            if (IsRectangle(_lastSelectedView))
            {
                _indent = _input.GetIndent();
                
                _isSelected = true;
            }
            else
            {
                var mousePosition = _input.GetMousePosition();
                mousePosition.z = 0;
                _levelManager.CreateRectangle(mousePosition);
            }
        }

        private void MoveRectangle()
        {
            if (_isSelected && _lastSelectedView != null)
            {
                var mousePosition = _input.GetMousePosition();
                mousePosition.z = 0f;
                
                _levelManager.MoveRectangle(_lastSelectedView, 
                    mousePosition + _indent);   
            }
        }

        private void ResetRectangleSelection()
        {
            if (_isSelected)
            {
                _isSelected = false;
                _lastSelectedView = null;
            }
        }

        private void DestroyRectangle()
        {
            _lastSelectedView = _input.GetHitedObject();

            if (_isMouseLine)
            {
                _levelManager.DestroyLineToMousePosition();
                
                _isMouseLine = false;
                _isFirstClickBinding = true;
            }

            if (IsRectangle(_lastSelectedView))
            {
                _levelManager.DestroyRectangle(_lastSelectedView);
                
                _isFirstClick = true;
            }
        }
    
        private void CreateBinding()
        {
            if (_isFirstClickBinding)
            {
                SelectFirstRectangleForBind();
            }
            else
            {
                SelectSecondRectangleAndBind();
            }
        }
        
        private void SelectFirstRectangleForBind()
        {
            _lastSelectedViewOnRightClick = _input.GetHitedObject();
            
            if (IsRectangle(_lastSelectedViewOnRightClick))
            {
                _firstSelectedViewOnRightClick = _lastSelectedViewOnRightClick;
                _levelManager.CreateLineToMousePosition(_firstSelectedViewOnRightClick.Transform.position,
                    _input.GetMousePosition());
                
                _isFirstClickBinding = false;
                _isMouseLine = true;
            }
        }
        
        private void SelectSecondRectangleAndBind()
        {
            _lastSelectedViewOnRightClick = _input.GetHitedObject();
            _levelManager.DestroyLineToMousePosition();
            
            _isMouseLine = false;
            _isFirstClickBinding = true;

            if (IsRectangle(_lastSelectedViewOnRightClick))
            {
                var view = _lastSelectedViewOnRightClick;
                _levelManager.CreateBinding(_firstSelectedViewOnRightClick, view);
            }
        }

        private static bool IsRectangle(ILevelObjectView view)
        {
            if (view == null) return false;
            return view.Tag == "Rectangle";
        }

        public void Dispose()
        {
            Detach();
        }
        private void Attach()
        {
            _input.RightButtonDown += OnRightButtonDown;
            _input.ButtonDown += OnButtonDown;
            _input.ButtonDrag += OnButtonDrag;
            _input.ButtonUp += OnButtonUp;
        }

        private void Detach()
        {
            _input.RightButtonDown -= OnRightButtonDown;
            _input.ButtonDown -= OnButtonDown;
            _input.ButtonDrag -= OnButtonDrag;
            _input.ButtonUp -= OnButtonUp;
        }
    }
}

