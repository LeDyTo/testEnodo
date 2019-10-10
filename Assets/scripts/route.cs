using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class route : MonoBehaviour
{
    private static List<List<Vector3>> roadList;

    public static List<List<Vector3>> RoadList
    {
        get => roadList;
        set => roadList = value;
    }
}
