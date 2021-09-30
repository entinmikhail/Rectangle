using Rectangle.Controller;
using Rectangle.Core;
using Rectangle.InputManager;
using Rectangle.Model;
using Rectangle.ScriptableObjects;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private UiController _uiController;
    [SerializeField] private GameInfo _gameInfo;
    
    private PlayerController _playerController;
    private LevelController _levelController;
    private LevelModel _levelModel;
    private GameModel _gameModel;
    private InputHandler _input;
    
    
    void Start()
    {
        _levelModel = new LevelModel(_gameInfo.LevelBounds);
        _gameModel = new GameModel();
        _levelController = new LevelController(_levelModel, _gameInfo);
        _input = new InputHandler();
        _playerController = new PlayerController( _levelController, _gameModel, _input, _gameInfo);
        
        _uiController.Init(_gameModel);
        _playerController.OnStart();
    }
    
    void Update()
    {
        _playerController.OnUpdate();
        _levelController.OnUpdate();
        _input.OnUpdate();
    }
}
