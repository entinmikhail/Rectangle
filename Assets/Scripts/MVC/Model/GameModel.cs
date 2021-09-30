using System;

namespace Rectangle.Model
{
    public class GameModel
    {
        public event Action<bool> GameModeChanged;

        public bool IsCreateMode => _isCreateMode;
        private bool _isCreateMode = true;

        public void ChangeGameMode()
        {
            _isCreateMode = !_isCreateMode;
            GameModeChanged?.Invoke(_isCreateMode);
        }
    }
}