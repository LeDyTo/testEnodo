using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

//Component Data des véhicules
[Serializable]
public struct Car : IComponentData
{
    public int speed;               //vitesse du véhicule
    public Vector3 currentPosition; //position de départ du véhicule
    public int roadIndice;          //indice de la route sur laquelle roule le véhicule
    public int positionInRoad;      //indice indiquant l'emplacement courant du véhicule sur la route 
    public int iterator;            //itérateur permettant de gérer l'évolution du véhicule, notamment sa vitesse
    public bool end;                //data temporaire permettant d'indiquer quand le véhicule est au bout de la route
}
