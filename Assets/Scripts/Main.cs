using Rectangle.Abstraction;
using Rectangle.Controller;
using Rectangle.Input;
using Rectangle.Model;
using Rectangle.ScriptableObjects;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private GameInfo _gameInfo;
    
    private IPlayerController _playerController;
    private ILevelManager _levelManager;
    private ILevelModel _levelModel;
    private IInputManager _input;
    
    private void Awake()
    {
        _levelModel = new LevelModel(_gameInfo.LevelBounds);
        _levelManager = new LevelManager(_levelModel, _gameInfo);
        _input = new InputManager();
        _playerController = new PlayerController( _levelManager, _input, _gameInfo);
        
        _playerController.Init();
    }
    
    private void Update()
    {
        _playerController.Update();
        _input.OnUpdate();
    }
}
