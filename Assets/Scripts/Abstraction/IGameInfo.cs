using UnityEngine;

namespace Rectangle.Abstraction
{
    public interface IGameInfo
    {
        Vector3 SpriteSize { get; }
        GameObject RectanglePrefab { get; }
        float TimeDelayForDoubleClick { get; }
        Material BindingLineMaterial { get; }
        Bounds LevelBounds { get; }
    }
}