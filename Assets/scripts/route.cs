using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contient la variable globale de la liste des routes
public class route : MonoBehaviour
{
    private static List<List<Vector3>> roadList;

    public static List<List<Vector3>> RoadList
    {
        get => roadList;
        set => roadList = value;
    }
}
