using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

//Script System du véhicule
public class CarSystem : JobComponentSystem
{

    [BurstCompile]  //Le burstcompile génère des erreur à cause de la liste de vector3, qui n'es tpas pris en charge, car pas blittable. Cette erreur n'est pas critique, c'est seulement pour le debug
    struct CarJob : IJobForEach<Translation, Rotation, Car>
    {
        [WriteOnly]
        public EntityCommandBuffer.Concurrent CommandBuffer;

        public void Execute(ref Translation translation, ref Rotation rotation, [ReadOnly] ref Car car)
        {
            //le véhicule se déplace entre 2 point de la route, l'iteratoire indique ou en est le véhicule entre ces points
            if (car.iterator < car.speed && !car.end)
            {
                Quaternion rot = rotation.Value;

                //On charge la liste de route et on accède à la route indiquée dans les data du véhicule
                List<List<Vector3>> listroad = route.RoadList;
                List<Vector3> road = listroad[car.roadIndice];

                //on applique la rotation et la translation correspondantes. La direction de la translation correspond 
                //au vecteur entre le point duquel il part et sa destination. Sa valeur est 1/vitesse, pour appliquer la
                //vitesse du véhicule
                rotation.Value = Quaternion.LookRotation(road[car.positionInRoad + 1] - road[car.positionInRoad], Vector3.up);
                translation.Value += (float3)((float)1 / car.speed) * (road[car.positionInRoad + 1] - road[car.positionInRoad]);


                car.iterator++;

                //si on arrive à la fin de la route, on indique dans la data end du véhicule
                if(car.iterator>= car.speed - 2 && car.positionInRoad >= road.Count - 2)
                {
                    car.end = true;
                }

                
            }

            //si le véhicule est arrivé au point de destination de la route, on met à jour la destination au prochain point de la route
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