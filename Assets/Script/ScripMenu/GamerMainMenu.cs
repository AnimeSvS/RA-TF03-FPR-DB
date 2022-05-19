using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//paquete para pasar escenas



public class GamerMainMenu : MonoBehaviour
{
    public void SceneInicio()
    {
        SceneManager.LoadScene("Inicio");
    }
 
    public void Close()
    {
        Application.Quit();
    }
}
