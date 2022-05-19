using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Permite la visualizacion del Item al dar click 
[CreateAssetMenu]

//La clase Item esta vinculada a ScriptableObject
public class Item : ScriptableObject
{
    //Definir la informacion con variables
    //Sprite representa a objetos 2D,3D
    public string ItemName;
    public Sprite ItemImage;
    public string ItemDescription;
    public GameObject Item3DModel;
}
 