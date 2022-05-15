using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameObject menuButtons, splashScreen;
    // Start is called before the first frame update
    void Start()
    {
        menuButtons = GameObject.Find("MenuButtons");
        menuButtons.SetActive(false);
        splashScreen = GameObject.Find("SplashScreen");
    }

    // Update is called once per frame
    void Update()
    {
        if(splashScreen.active && (Input.GetMouseButtonDown(0) || Input.touchCount > 0)){
            menuButtons.SetActive(true);
            splashScreen.SetActive(false);
        }
    }
}
