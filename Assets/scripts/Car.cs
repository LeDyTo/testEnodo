using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[Serializable]
public struct Car : IComponentData
{
    public int speed;
    public Vector3 currentPosition;
    public int roadIndice;
    public int positionInRoad;
    public int iterator;
    public bool end;
}
