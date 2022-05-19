using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//referenciar
using System;

public class GameManager : MonoBehaviour
{
    // estados de la aplicacion 
    public event Action OnMainMenu;
    public event Action OnItemsMenu;
    public event Action OnARPosition;
    //----------------------------------

    //crear un patron singleton el cual sera accesible globalmente, sera instanciada una sola vez
    //El propósito de este patrón es evitar que sea creado más de un objeto por clase.
    public static GameManager instance;
    //----------------------------------

    //Awake - se utiliza cuando se inicializa un GameObject activo que contiene el script cuando se carga una escena
    //solo puede ser llamado una vez
    private void Awake()
    {
        //se crea una condicional donde se asegura la existencia de una instancia en GAMEMANAGER
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    //-------------------------------------

    //metodo de inicio de la aplicacion
    void Start()
    {
        //cuando la aplicacion se inicie, solo llamara al mainMenu
        MainMenu();
    }
    //-----------------------------------------

    //funciones para llamar a los eventos
    public void MainMenu()
    {
        // ? = constatar que existe algo que esta suscrito al evento
        OnMainMenu?.Invoke();
        Debug.Log("Menu Inicial Activado!");
    }
    public void ItemsMenu()
    {
        OnItemsMenu?.Invoke();
        Debug.Log("Items del menu activados");
    }
    public void ARPosition()
    {
        OnARPosition?.Invoke();
        Debug.Log("Posicion de AR activada!");
    }
    //-----------------------------------------
}