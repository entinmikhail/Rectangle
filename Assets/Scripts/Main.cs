using Rectangle.Core;
using Rectangle.Model;
using Rectangle.View;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private Button _changeModeButton;
    [SerializeField] private Text _changeModeButtonText;
    
    private LevelController _levelController;
    private GameObject _rectangle;
    private GameObject _firstGo;
    private Material _material;
    private Camera _camera;

    private LineRenderer _lineToMouse;
    private GameObject _lineGo;
    
    private RaycastHit2D _hit;
    private RaycastHit2D _firstHit;
    private RaycastHit2D _secondHit;


    private Vector3 _indent;
    
    private bool isFirstClick = true;
    private bool _isSelected;
    private bool _isCreateMode = true;
    
    
    
    private bool _isFirst = true;
    private float _timeDelay = 0.3f;
    private float _firstClickTime;
    
    void Start()
    {
        _levelController = new LevelController();
        _levelController.OnInit();
        _changeModeButton.onClick.AddListener(ChangeGameMode);
        _material = Resources.Load<Material>("Default");
    }
    
    void Update()
    {
        _levelController.OnUpdate();
        UpdateLineMouse();
        InputUpdate();
    }

    private void InputUpdate()
    {
        if (Input.GetButtonDown("Fire1")) OnClick();

        if (Input.GetButton("Fire1")) MoveRectangle();

        if (Input.GetButtonUp("Fire1")) ResetRectangleSelection();
    }

    private void OnClick()
    {
        if (Time.time - _firstClickTime > _timeDelay)
            _isFirst = true;

        if (_isCreateMode)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_isFirst)
            {
                CreateRectangle(mousePosition);
            }
            else
            {
                DestroyRectangle(mousePosition);
            }
        }

        else
        {
            CreateBinding();
        }
    }

    private void CreateRectangle(Vector3 mousePosition)
    {
        _firstHit = Physics2D.Raycast(mousePosition, Vector2.zero);
        _hit = _firstHit;
        _firstClickTime = Time.time;
        _isFirst = false;

        SelectOrCreateRectangle();
    }

    private void DestroyRectangle(Vector3 mousePosition)
    {
        _secondHit = Physics2D.Raycast(mousePosition, Vector2.zero);
        _hit = _secondHit;

        if (_secondHit.transform != null && _firstHit.transform == _secondHit.transform)
        {
            DestroyRectangle();
            _isFirst = true;
        }
        else
        {
            SelectOrCreateRectangle();
        }
    }

    private void SetIndent()
    {
        var hitPoint = new Vector3(_hit.point.x, _hit.point.y, 0);
        _indent = _hit.transform.position - hitPoint;
    }

    void ChangeGameMode()
    {
        _isCreateMode = !_isCreateMode;
        
        if (_lineGo != null)
        {
            Destroy(_lineGo); 
        }
        
        if (_isCreateMode)
        {
            _changeModeButtonText.text = "CreateMode";
        }
        else
        {
            _changeModeButtonText.text = "BindingMode";
        }
    }
    
    private void SelectOrCreateRectangle()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (IsRectangle())
        {
            _rectangle = _hit.transform.gameObject;
            _isSelected = true;
            SetIndent();
        }
        else
        {
            if (_hit.collider != null && _hit.collider.CompareTag("UI"))
            {
                return;
            }
                
            
            mousePosition.z = 0;
            _levelController.CreateRectangle(mousePosition);
        }
    }

    private void MoveRectangle()
    {
        if (_isSelected && _rectangle != null)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            _levelController.MoveRectangle(_rectangle.GetComponent<LevelObjectView>(), mousePosition + _indent);   
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
            isFirstClick = true;
            _levelController.DestroyRectangle(_hit.transform.gameObject.GetComponent<LevelObjectView>());
        }
    }

    private void UpdateLineMouse()
    {
        if (_lineToMouse != null)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            _lineToMouse.SetPosition(1, mousePosition);
        }
    }
    private void CreateBinding()
    {
        
        
        if (isFirstClick)
        {
            if (IsRectangle())
            {
                _lineGo = new GameObject();
                _lineToMouse = _lineGo.AddComponent<LineRenderer>();

                _firstGo = _hit.transform.gameObject;
                isFirstClick = false;
                
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                
                _lineGo.name = "tmp";
                _lineToMouse.startWidth = 0.2f;
                _lineToMouse.material = _material;
                _lineToMouse.material.color = new Color(Random.value, Random.value, Random.value, 1f);
                _lineToMouse.SetPositions(new Vector3[2]{_firstGo.transform.position, mousePosition});
            }
        }
        else
        {
            if (_lineGo != null)
            {
                Destroy(_lineGo); 
            }
            
            if (IsRectangle())
            {
                var go = _hit.transform.gameObject;
                _levelController.CreateBinding(_firstGo.GetComponent<LevelObjectView>(), go.GetComponent<LevelObjectView>());
                isFirstClick = true;
            }
        }
    }
    
    private bool IsRectangle()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
        if (_hit)
        {
            if(_hit.collider.CompareTag("Rectangle"))
                return true;
        }

        return false;
    }
}
