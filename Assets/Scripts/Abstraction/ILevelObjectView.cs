using UnityEngine;

namespace Rectangle.Abstraction
{
    public interface ILevelObjectView
    { 
        Transform Transform { get; }
        string Tag { get; }
    }
}