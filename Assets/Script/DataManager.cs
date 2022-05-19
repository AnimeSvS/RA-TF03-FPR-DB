using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //definir los campos privados
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private ItemButtonManager itemButtonManager;

   //funcion Start
    void Start()
    {
        //vincular la funcion cuando el boton sea llamado
        GameManager.instance.OnItemsMenu += CreateButtons;
    }

    //funcion CreateButtons
    private void CreateButtons()
    {
        foreach (var item in items)
        {
            //variables
            ItemButtonManager itemButton;
            itemButton = Instantiate(itemButtonManager, buttonContainer.transform);
            itemButton.ItemName = item.ItemName;
            itemButton.ItemDescription = item.ItemDescription;
            itemButton.ItemImage = item.ItemImage;
            itemButton.Item3DModel = item.Item3DModel;
            itemButton.name = item.ItemName;
        }
        //Al terminar se desvinculara del evento OnItemsMenu
        GameManager.instance.OnItemsMenu -= CreateButtons;

    }
}
