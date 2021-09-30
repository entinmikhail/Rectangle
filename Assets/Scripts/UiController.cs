using Rectangle.Model;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    
    [SerializeField] private Button _changeModeButton;
    [SerializeField] private Text _changeModeButtonText;

    private GameModel _gameModel;
    
    void Start()
    {
        _changeModeButton.onClick.AddListener(ChangeGameMode);
        
    }
    
    public void Init(GameModel gameModel)
    {
        _gameModel = gameModel;
        ChangeText();
    }
    
    private void ChangeGameMode()
    {
        _gameModel.ChangeGameMode();
        ChangeText();
    }

    private void ChangeText()
    {
        if (_gameModel.IsCreateMode)
        {
            _changeModeButtonText.text = "Create";
        }
        else
        {
            _changeModeButtonText.text = "Bind";
        }
    }
}
