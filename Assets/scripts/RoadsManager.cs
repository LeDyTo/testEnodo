using System.Collections.Generic;
using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

//Main script qui gère un peu tout, comme l'affichage et la création de route, ainsi que la création de véhicules
public class RoadsManager : MonoBehaviour
{
    private bool drawRoads;                                         //booléen indiquant si on est en mode dessin ou non
    private List<Vector3> Points;                                   //liste des points de la route en cours de création


    [SerializeField] GameObject car;                                //prefab des véhicules
    private Entity carEntity;                                       //et entity correspondant
    

    private EntityManager entityManager;                            
    private int roadCounter;                                        //entier pour savoir combien de route sont dessinées (pour le moment utilisé uniquement pour dire quand faire apparaitre des véhicules)
    public List<List<Vector3>> roadlist = new List<List<Vector3>>();//liste composée de l'ensemble des routes
    private int nbcar = 0;                                          //simple compteur de véhicules pour savoir combien ont été invoqués

    void Start()
    {
        drawRoads = false;
        Points = new List<Vector3>();
        carEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(car, World.Active); //conversion du prefab Gameobject en sont équivalant Entity
        entityManager = World.Active.EntityManager;
        roadCounter = 0;
    }

    void Update()
    {
        //A l'appuie de la touche D, on entre en mode dessin de route, et en appuyant sur F, on indique qu'une route est terminée
        if (Input.GetKeyDown(KeyCode.D) && !drawRoads)
        {
            drawRoads = true;
        }

        if (Input.GetKeyDown(KeyCode.F) && drawRoads)
        {
            //lorsqu'une route est terminée, on l'ajoute à la liste des routes, et on réinitialise la route en création 
            roadlist.Add(Points);
            roadCounter++;
            Points = new List<Vector3>();
            drawRoads = false;
        }

        //Lorsqu'au moins 1 route est créée, on peut invoquer des véhicules
        if(roadCounter>=1)
        {
            SpawnCar();
        }

    }

    private void OnMouseDown()
    {
        //en mode route, en cliquant sur la souris, on ajoute ce point à la route, et on le relie au point précédent, pour "dessiner" la route
        if (drawRoads)
        {
           Vector3 Point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            
            if (Points.Count >= 1)
            {
                Debug.DrawLine(Points[Points.Count - 1], Point, Color.green, Mathf.Infinity);
            }
            Points.Add(Point);
        }
    }

    private void SpawnCar()
    {
        //On créé des véhicules aléatoirement
        if (Random.Range(0f, 100f)< 1f )
        {
            nbcar++;
            Debug.Log("nombre de voiture présentes : " + nbcar);

            
            var instance = entityManager.Instantiate(carEntity);    //instance d'entity du prefab véhicule
            int ind = Random.Range(0, roadlist.Count);              //Le véhicule se voit attribuer une route aléatoirement parmi la liste de routes
            int vit = Random.Range(10, 100);                        //On lui attribut également une vitesse aléatoire
            route.RoadList = roadlist;                              //on met à jour la class globale contenant la liste de route

            //on met à jour les data du véhicule créé
            entityManager.SetComponentData(instance, new Car { speed = vit, currentPosition = roadlist[ind][0], roadIndice = ind, positionInRoad = 0, iterator = 0, end = false });
            entityManager.SetComponentData(instance, new Translation { Value = roadlist[ind][0] });
        }
    }
}