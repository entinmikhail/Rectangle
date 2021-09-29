using Rectangle.Core;
using Rectangle.Model;
using Rectangle.View;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private Button _changeModeButton;
    
    private LevelController _levelController;
    private GameObject _rectangle;
    private GameObject _firstGo;
    private Camera _camera;
    
    private RaycastHit2D _hit;
    
    private bool isFirstClick = true;
    private bool _isSelected;
    private bool gameMode = true;
    
    void Start()
    {
        _levelController = new LevelController();
        _levelController.OnInit();
        _changeModeButton.onClick.AddListener(ChangeGameMode);
    }
    
    void Update()
    {
        _levelController.OnUpdate();
        
        if (Input.GetButtonDown("Fire1")) SelectOrCreateRectangle();
        
        if (Input.GetButton("Fire1")) MoveRectangle();
        
        if (Input.GetButtonUp("Fire1")) ResetRectangleSelection();

        if (Input.GetButtonDown("Fire2")) DestroyRectangle();

        if (Input.GetButtonDown("Fire3")) CreateBinding();
    }

    void ChangeGameMode()
    {
        Debug.Log(_levelController.LevelModel.GameMode);
        if (gameMode)
        {
            _levelController.LevelModel.SetGameState(GameMode.Binding);
            gameMode = false;
        }
        else
        {
            _levelController.LevelModel.SetGameState(GameMode.Default);
            gameMode = true;
        }
        
    }
    
    private void SelectOrCreateRectangle()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (IsRectangle())
        {
            _rectangle = _hit.transform.gameObject;
            _isSelected = true;
            return;
        }

        mousePosition.z = 0;
        _levelController.CreateRectangle(mousePosition);
    }

    private void MoveRectangle()
    {
        if (_isSelected && _rectangle != null )
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            _levelController.MoveRectangle(_rectangle.GetComponent<LevelObjectView>(), mousePosition);   
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

    private void CreateBinding()
    {
        if (isFirstClick)
        {
            if (IsRectangle())
            {
                _firstGo = _hit.transform.gameObject;
                isFirstClick = false;
            }
        }
        else
        {
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
