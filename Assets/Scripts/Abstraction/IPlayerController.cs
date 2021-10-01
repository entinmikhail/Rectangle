namespace Rectangle.Abstraction
{
    public interface IPlayerController : IController
    {
        
    }

    public interface IController
    {
        void Init();
        void Update();
        void Dispose();
    }
}