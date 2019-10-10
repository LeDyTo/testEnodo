using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
public class CarAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new Car { speed = 1, currentPosition = new Vector3(), roadIndice = 0, positionInRoad = 0, iterator = 0, end = false };
        dstManager.AddComponentData(entity, data);
    }
}