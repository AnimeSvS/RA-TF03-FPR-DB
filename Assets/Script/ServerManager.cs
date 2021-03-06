using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//asignar 
using System;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{


    //campo de  asginacion de archivo Json 

    [SerializeField] private string jsonURL;

    [SerializeField] private ItemButtonManager itemButtonManager;
    [SerializeField] private GameObject buttonsContainer;
    [Serializable]
    //lectura de archivo Json segun la estructura
    public struct Items
    {
        [Serializable]
        public struct Item
        {
            public string Name;
            public string Description;
            public string URLBundleModel; //link de dscarga assetbundle (modelo3D)
            public string URLImageModel;//link de dscarga de imagen
        }   
        //creacion de un array para items ya que son varios
        public Item[] items;
    }

    public Items newItemsCollection = new Items();



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetJsonData());

        GameManager.instance.OnItemsMenu += CreateButtons;
    }

    private void CreateButtons()
    {
        foreach (var item in newItemsCollection.items)
        {
            ItemButtonManager itemButton;
            itemButton = Instantiate(itemButtonManager, buttonsContainer.transform);
            itemButton.name = item.Name;
            itemButton.ItemName = item.Name;
            itemButton.ItemDescription = item.Description;
            itemButton.URLBundleModel = item.URLBundleModel;

            //llamar a la coorutina
            StartCoroutine(GetBundleImage(item.URLImageModel, itemButton));




        }
        GameManager.instance.OnItemsMenu -= CreateButtons;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //creacion de una funcion el cual descargara el archivo json y asi asignara a la estructura creada
    IEnumerator GetJsonData()
    {
        UnityWebRequest serverRequest = UnityWebRequest.Get(jsonURL);
        yield return serverRequest.SendWebRequest();
        //si el resultado de requeste fue exitoso , asigna toda la informacion el archivo json asignada
        if (serverRequest.result == UnityWebRequest.Result.Success)
        {
            newItemsCollection = JsonUtility.FromJson<Items>(serverRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error!!!!!!!");
        }
    }
    IEnumerator GetBundleImage(string urlImage, ItemButtonManager button)
    {
        UnityWebRequest serverRequest = UnityWebRequest.Get(urlImage);
        serverRequest.downloadHandler = new DownloadHandlerTexture();
        yield return serverRequest.SendWebRequest();
        //si el resultado de requeste fue exitoso , asigna toda la informacion el archivo json asignada
        if (serverRequest.result == UnityWebRequest.Result.Success)
        {
            button.ImageBundle.texture = ((DownloadHandlerTexture)serverRequest.downloadHandler).texture;
        }
        else
        {
            Debug.Log("Error!!!!!!!");
        }
    }
}
