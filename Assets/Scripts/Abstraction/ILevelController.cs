﻿using UnityEngine;

namespace Rectangle.Abstraction
{
    public interface ILevelController
    {
        void OnUpdate();
        void CreateRectangle(Vector3 position);
        void MoveRectangle(ILevelObjectView go, Vector3 newPosition);
        void DestroyRectangle(ILevelObjectView go);
        void CreateBinding(ILevelObjectView firstGo, ILevelObjectView secondGo);
    }
}