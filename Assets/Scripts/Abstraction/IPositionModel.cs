using UnityEngine;

namespace Abstraction
{

    public interface IRectangle
    {

    }


    public interface IPositionModel
    {
        Vector3 CurPosition { get; }
        
        void SetPosition(Vector3 newPosition);
    }
}