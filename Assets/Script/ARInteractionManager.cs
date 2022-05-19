using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//referenciar el evento del sistema
using UnityEngine.EventSystems;
//refrenciar el AR
using UnityEngine.XR.ARFoundation;
//referenciar el subsistem
using UnityEngine.XR.ARSubsystems;

public class ARInteractionManager : MonoBehaviour
{
    //creacion de campos
    [SerializeField] private Camera aRCamera;
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // denifir campos del modelo, punto y seleccion de items
    private GameObject aRPointer;
    private GameObject item3DModel;
    private GameObject itemSelect;

    // boleanos para saber la posion del modelo 3D
    private bool isInitialPosition;
    private bool isOverUI;
    private bool isOver3DModel;


    private Vector2 initialTouchPos;


    //propiedad de Item3DModel
    public GameObject Item3DModel
    {
        set
        {
            //cuando sea asignado el item 3D, tomara la posicion del punto(circulo guia)
            item3DModel = value;
            item3DModel.transform.position = aRPointer.transform.position;
            item3DModel.transform.parent = aRPointer.transform;
            isInitialPosition = true;
        }
    }
    // Se llama al inicio antes de la primera actualización del cuadro.
    void Start()
    {
        aRPointer = transform.GetChild(0).gameObject;
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        GameManager.instance.OnMainMenu += SetItemPosition;
    }


    // Update is called once per frame
    void Update()
    {

        //condicionales de actualizacion por modelo 3D e interacciones
        if (isInitialPosition)
        {
            Vector2 middlePointScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            aRRaycastManager.Raycast(middlePointScreen, hits, TrackableType.Planes);

            if (hits.Count>0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
                aRPointer.SetActive(true); 
                isInitialPosition = false;

            }
        }
        //si no hay toques mantiene su posicion , pero si se toca una ves mostrara los botones asignados
        if (Input.touchCount>0)
        {
            Touch touchOne = Input.GetTouch(0);
            if(touchOne.phase == TouchPhase.Began)
            {
                var touchPositon = touchOne.position;
                isOverUI = isTapOverUI(touchPositon);
                isOver3DModel = isTapOver3DModel(touchPositon);
            }
            if (touchOne.phase == TouchPhase.Moved)
            {
                if (aRRaycastManager.Raycast(touchOne.position, hits, TrackableType.Planes))
                {
                    //para mover el modelo 3D en tiempo real
                    Pose hitPose = hits[0].pose;
                    if (!isOverUI && isOver3DModel)
                    {
                        transform.position = hitPose.position;
                    }
                }

            }
            //si se mantiene presionado 2 toques, se podra girar en un angulo de 360°
            if (Input.touchCount == 2)
            {
                Touch touchTwo = Input.GetTouch(1);
                if(touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
                {
                    initialTouchPos = touchTwo.position - touchOne.position;
                }
                if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
                {
                    Vector2 currentTouchPos = touchTwo.position - touchOne.position;
                    float angle = Vector2.SignedAngle(initialTouchPos, currentTouchPos);
                    item3DModel.transform.rotation = Quaternion.Euler(0, item3DModel.transform.eulerAngles.y - angle, 0);
                    initialTouchPos = currentTouchPos;
                }
            }
            
            if (isOver3DModel && item3DModel == null && !isOverUI)
            {
                GameManager.instance.ARPosition();
                item3DModel = itemSelect;
                itemSelect = null;
                aRPointer.SetActive(true);
                transform.position = item3DModel.transform.position;
                item3DModel.transform.parent = aRPointer.transform;
            }
        }
    }

    //condicionales para la posicion del item
    private bool isTapOver3DModel(Vector2 touchPositon)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPositon);
        if( Physics.Raycast(ray, out RaycastHit hit3DModel))
        {
            if (hit3DModel.collider.CompareTag("Item"))
            {
                itemSelect = hit3DModel.transform.gameObject;
                return true;
            }
        }
        return false;
    }

    //Mantiene la posicion del modelo 3D con ayuda de la nube de puntos
    private bool isTapOverUI(Vector2 touchPositon)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position =  new Vector2(touchPositon.x, touchPositon.y);
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);

        return result.Count > 0;
    }

    //asigna las posiciones de cada item dependiendo al cirgulo guia
    private void SetItemPosition()
    {
        if (item3DModel != null)
        {
            item3DModel.transform.parent = null;
            aRPointer.SetActive(false);
            item3DModel = null;
        }
    }

    //borra el modelo 3D seleccionado, con el boton delete
    public void DeleteItem()
    {
        Destroy(item3DModel);
        aRPointer.SetActive(false);
        GameManager.instance.OnMainMenu += SetItemPosition;
    }

}
