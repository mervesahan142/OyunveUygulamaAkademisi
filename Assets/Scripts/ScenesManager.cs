using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            switch(SceneManager.GetActiveScene().name){
                case "Forest":
                    Application.LoadLevel(2);
                    break;
                case "village":
                    if(gameObject.name == "RightDoor"){
                        Application.LoadLevel(3);
                    }else{
                        Application.LoadLevel(1);
                    }
                    break;
                case "Cave":
                    if(gameObject.name == "RightDoor"){
                        Application.LoadLevel(4);
                    }else{
                        Application.LoadLevel(2);
                    }
                    break;
                case "Castle":
                    if(gameObject.name == "RightDoor"){
                        Application.LoadLevel(5);
                    }else{
                        Application.LoadLevel(3);
                    }
                    break;
            }
        }
    }

    public void MainMenu(){
        Application.LoadLevel(0);
    }

    public void ResetAndMainMenu(){
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
        MainMenu();
    }

    public void TryAgain(){
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

    public void GameOver(){
        Application.LoadLevel(6);
    }

}
