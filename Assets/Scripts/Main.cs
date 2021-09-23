using System.Collections;
using System.Collections.Generic;
using Rectangle.Core;
using Rectangle.Model;
using UnityEngine;

public class Main : MonoBehaviour
{
    private LevelManager _levelManager;
    private Camera _camera;
    void Start()
    {
        _levelManager = new LevelManager();
        
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log(Input.mousePosition);
            var vect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vect.z = 0;
            _levelManager.CreateRectangle(new RectangleModel(vect));
        }
    }
}
