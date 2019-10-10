using System.Collections.Generic;
using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class RoadsManager : MonoBehaviour
{
    private bool drawRoads;
    private List<Vector3> Points;
    private Vector3 roadDir;
    private Ray road;
    private float roadLength;
    private LineRenderer line;
    [SerializeField] GameObject car;
    private Entity carEntity;
    private EntityManager entityManager;
    private int roadCounter;
    private int pointRoadCounter;
    private Entity instance;
    public List<List<Vector3>> roadlist = new List<List<Vector3>>();
    private int nbcar = 0;

    void Start()
    {
        drawRoads = false;
        Points = new List<Vector3>();
        carEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(car, World.Active);
        entityManager = World.Active.EntityManager;
        roadCounter = 0;
        pointRoadCounter = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && !drawRoads)
        {
            drawRoads = true;
        }

        if (Input.GetKeyDown(KeyCode.F) && drawRoads)
        {
            roadlist.Add(Points);
            roadCounter++;


            Points = new List<Vector3>();
            drawRoads = false;
        }
        if(roadCounter>=1)
        {
            SpawnCar();
        }

    }

    private void OnMouseDown()
    {
        if (drawRoads)
        {
           Vector3 Point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            
            if (Points.Count >= 1)
            {
                Debug.DrawLine(Points[Points.Count - 1], Point, Color.green, Mathf.Infinity);
            }
            pointRoadCounter++;
            Points.Add(Point);
        }
    }

    private void SpawnCar()
    {
        Debug.Log("spawncar");
        if (Random.Range(0f, 100f)< 100f )
        {
            nbcar++;
            Debug.Log("nombre de voiture présentes : " + nbcar);

            var instance = entityManager.Instantiate(carEntity);
            int ind = Random.Range(0, roadlist.Count);
            int vit = Random.Range(10, 100);
            route.RoadList = roadlist;
            entityManager.SetComponentData(instance, new Car { speed = vit, currentPosition = roadlist[ind][0], roadIndice = ind, positionInRoad = 0, iterator = 0, end = false });
            entityManager.SetComponentData(instance, new Translation { Value = roadlist[ind][0] });
        }
    }
}