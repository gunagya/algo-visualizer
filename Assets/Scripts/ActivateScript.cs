using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateScript : MonoBehaviour
{
    public GameObject driverObject;
    public GameObject infoCanvas;

    GameObject mainMenuCanvas;

    public void loadDriver()
    {
        driverObject.SetActive(true);
        infoCanvas.SetActive(true);

        mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        mainMenuCanvas.SetActive(false);
    }
}
