using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Ana menüdeki durumları yöterek ana menüden sahnelere geçişi yöneten scripttir.

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
        //reset
        if(!PlayerPrefs.HasKey("workOneTime")){
            PlayerPrefs.SetInt("workOneTime", 1);

            PlayerPrefs.SetInt("spokeWithFreeKnight_1", 0);
            PlayerPrefs.SetInt("spokeWithKnight", 0);
            PlayerPrefs.SetInt("spokeWithWarrior", 0);
            PlayerPrefs.SetInt("spokeWithKing", 0);

            PlayerPrefs.SetInt("level", 1);
            PlayerPrefs.SetInt("levelXp", 0);
            PlayerPrefs.SetInt("skillPointValue", 0);
            PlayerPrefs.SetInt("skillRightValue", 0);

            PlayerPrefs.SetInt("attackLevel", 1);
            PlayerPrefs.SetInt("enduranceLevel", 1);
            PlayerPrefs.SetInt("moveSpeedLevel", 1);
            PlayerPrefs.SetInt("dodgeSpeedLevel", 1);
            PlayerPrefs.SetInt("attackRangeLevel", 1);
        }
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
        if(PlayerPrefs.GetInt("spokeWithKing") == 1){
            Application.LoadLevel(4);
        }else if(PlayerPrefs.GetInt("spokeWithWarrior") == 1){
            Application.LoadLevel(3);
        }else if(PlayerPrefs.GetInt("spokeWithKnight") == 1){
            Application.LoadLevel(2);
        }else if(PlayerPrefs.GetInt("spokeWithFreeKnight_1") == 1){
            Application.LoadLevel(1);
        }
    }

    public void Exit(){
        Application.Quit();
    }



}
