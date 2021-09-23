using Rectangle.Core;
using Rectangle.Model;
using UnityEngine;

public class Main : MonoBehaviour
{
    private LevelManager _levelManager;
    private Camera _camera;
    
    private RaycastHit2D _hit;

    private bool isFirstClick = true;
    private GameObject _firstGo;
    void Start()
    {
        _levelManager = new LevelManager();
    }
    
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
        _levelManager.OnUpdate();
        
        if (Input.GetButtonDown("Fire1"))
        {
            mousePosition.z = 0;
            _levelManager.CreateRectangle(new RectangleModel(mousePosition));
        }

        if (Input.GetButton("Fire1"))
        {
            if (_hit)
            {
                if(_hit.collider.CompareTag("Rectangle"))
                {
                    mousePosition.z = 0f;
                    _levelManager.MoveRectangle(_hit.transform.gameObject, mousePosition);
                }
            }
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            if (_hit)
            {
                if(_hit.collider.CompareTag("Rectangle"))
                {
                    _levelManager.DestroyRectangle(_hit.transform.gameObject);
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
                        Debug.Log("first");
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
                        Debug.Log("second");
                        _levelManager.CreateBinding(_firstGo, go);
                        isFirstClick = true;
                    }
                }
            }
        }
    }
}
