using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//referenciar el UI (text , image)
using UnityEngine.UI;

public class ItemButtonManager : MonoBehaviour
{
  
    // Crear atributos de objetos privados
    private string itemName;
    private string itemDescription;
    private Sprite itemImage;
    private GameObject item3DModel;
    private ARInteractionManager interactionManager;

    //campo de link de descarga
    private string urlBundleModel;
    private RawImage imageBundle;


    //Creacion de propiedades para acceder a las clases
    public string ItemName
    {
        //consigue los datos asignados por cada valor
        set
        {
            itemName = value;
        }
    }
    //llamada de obejetos privados a publicas
    // => Operador lambda = es una subrutina definida que no está enlazada a un identificador.
    public string ItemDescription { set => itemDescription = value; }
    public Sprite ItemImage { set => itemImage = value; }


    public GameObject Item3DModel { set => item3DModel = value; }
    public string URLBundleModel { set => urlBundleModel = value; }
    public RawImage ImageBundle { get => imageBundle; set=> imageBundle = value; }

    void Start()
    {
        //Al crear el boton se asignaran los valor dados
        transform.GetChild(0).GetComponent<Text>().text = itemName;
        //transform.GetChild(1).GetComponent<RawImage>().texture = itemImage.texture;

        //funcion para crear la imagen 
        imageBundle = transform.GetChild(1).GetComponent<RawImage>();
        transform.GetChild(2).GetComponent<Text>().text = itemDescription;


        //Dar valor a boton con eventos
        var button = GetComponent<Button>();
        //al hacer click en el boton llamara al evento del GameManager - ARPosition
        button.onClick.AddListener(GameManager.instance.ARPosition);
        //al hacer click en el boton llamara a la funcion Create3DModel
        button.onClick.AddListener(Create3DModel);


        interactionManager = FindObjectOfType<ARInteractionManager>();
    }

    private void Create3DModel()
    {
        //al crear tendra que asignaar los modelos 3D 
        //interactionManager.Item3DModel = Instantiate(item3DModel);
        StartCoroutine(DownLoadAssetBundle(urlBundleModel));

    }
  IEnumerator DownLoadAssetBundle(string urlAssetBundle)
    {
        UnityWebRequest serverRequest = UnityWebRequestAssetBundle.GetAssetBundle(urlAssetBundle);
        yield return serverRequest.SendWebRequest();
        if (serverRequest.result == UnityWebRequest.Result.Success)
        {
            AssetBundle model3D = DownloadHandlerAssetBundle.GetContent(serverRequest);
            if (model3D != null)
            {
                interactionManager.Item3DModel = Instantiate(model3D.LoadAsset(model3D.GetAllAssetNames()[0]) as GameObject);
            }
            else
            {
                Debug.Log("AssetBundle no valido!! >:v");
            }
        }
        else
        {
            Debug.Log("Error!!!!");
        }
    }

}
