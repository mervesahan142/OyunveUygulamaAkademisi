using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameObject menuButtons, splashScreen, resumeButton, newGameButton;

    private void Awake() {
        splashScreen = GameObject.Find("SplashScreen");
        if(PlayerPrefs.GetInt("spokeWithFreeKnight_1") == 0){
            GameObject.Find("ResumeButton").SetActive(false);
        }else{
            GameObject.Find("NewGameButton").SetActive(false);
        }
        menuButtons = GameObject.Find("MenuButtons");
        menuButtons.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(splashScreen.active && (Input.GetMouseButtonDown(0) || Input.touchCount > 0)){
            menuButtons.SetActive(true);
            splashScreen.SetActive(false);
        }

    }

    public void NewGame(){
        Application.LoadLevel(1);
    }

    public void Resume(){
        if(PlayerPrefs.GetInt("spokeWithFreeKnight_1") == 0){
            Application.LoadLevel(1);
        }else if(PlayerPrefs.GetInt("spokeWithKnight") == 0){
            Application.LoadLevel(2);
        }else if(PlayerPrefs.GetInt("spokeWithWarrior") == 0){
            Application.LoadLevel(3);
        }else if(PlayerPrefs.GetInt("spokeWithKing") == 0){
            Application.LoadLevel(4);
        }
    }

    public void Exit(){
        Application.Quit();
    }



}
