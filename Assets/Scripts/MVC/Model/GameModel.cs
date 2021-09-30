using System;

namespace Rectangle.Model
{
    public class GameModel
    {
        public bool IsCreateMode => _isCreateMode;
        private bool _isCreateMode = true;
        
        public event Action<bool> GameModeChanged;

        public void ChangeGameMode()
        {
            _isCreateMode = !_isCreateMode;
            GameModeChanged?.Invoke(_isCreateMode);
        }
    }
}