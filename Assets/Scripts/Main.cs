using Rectangle.Core;
using Rectangle.Model;
using Rectangle.View;
using UnityEngine;

public class Main : MonoBehaviour
{
    private LevelController _levelController;
    private Camera _camera;
    
    private RaycastHit2D _hit;
    private GameObject _rectangle;
    private bool isFirstClick = true;
    private GameObject _firstGo;

    private bool _isSelected;
    
    void Start()
    {
        _levelController = new LevelController();
        _levelController.OnInit();
    }
    
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
        _levelController.OnUpdate();
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (_hit)
            {
                if(_hit.collider.CompareTag("Rectangle"))
                {
                    _rectangle = _hit.transform.gameObject;
                    _isSelected = true;
                }
            }
            
            mousePosition.z = 0;
            _levelController.CreateRectangle(new RectangleModel(mousePosition));
        }

        if (Input.GetButton("Fire1"))
        {
            if (_rectangle != null && _isSelected)
            {
                mousePosition.z = 0f;
                _levelController.MoveRectangle(_rectangle.GetComponent<LevelObjectView>(), mousePosition);   
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (_isSelected)
            {
                _isSelected = false;
                _rectangle = null;
            }
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            if (_hit)
            {
                if(_hit.collider.CompareTag("Rectangle"))
                {
                    isFirstClick = true;
                    _levelController.DestroyRectangle(_hit.transform.gameObject.GetComponent<LevelObjectView>());
                }
            }
        }
        
        if (Input.GetButtonDown("Fire3"))
        {
            if (isFirstClick)
            {
                if (_hit)
                {
                    if(_hit.collider.CompareTag("Rectangle"))
                    {
                        _firstGo = _hit.transform.gameObject;
                        isFirstClick = false;
                    }
                }
            }
            else
            {
                if (_hit)
                {
                    if(_hit.collider.CompareTag("Rectangle"))
                    {
                        var go = _hit.transform.gameObject;
                        _levelController.CreateBinding(_firstGo.GetComponent<LevelObjectView>(), go.GetComponent<LevelObjectView>());
                        isFirstClick = true;
                    }
                }
            }
        }
    }
}
