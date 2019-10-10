using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CarSystem : JobComponentSystem
{

    [BurstCompile]
    struct CarJob : IJobForEach<Translation, Rotation, Car>
    {
        [WriteOnly]
        public EntityCommandBuffer.Concurrent CommandBuffer;

        public void Execute(ref Translation translation, ref Rotation rotation, [ReadOnly] ref Car car)
        {
            if (car.iterator < car.speed && !car.end)
            {
                Quaternion rot = rotation.Value;
                List<List<Vector3>> listroad = route.RoadList;
                List<Vector3> road = listroad[car.roadIndice];

                rotation.Value = Quaternion.LookRotation(road[car.positionInRoad + 1] - road[car.positionInRoad], Vector3.up);
                translation.Value += (float3)((float)1 / car.speed) * (road[car.positionInRoad + 1] - road[car.positionInRoad]);
                car.iterator++;
                if(car.iterator>= car.speed - 2 && car.positionInRoad >= road.Count - 2)
                {
                    car.end = true;
                }

                
            }
            else if(!car.end)
            {
                car.iterator = 0;
                car.positionInRoad++;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new CarJob
        {
        };

        return job.Schedule(this, inputDependencies);
    }
}