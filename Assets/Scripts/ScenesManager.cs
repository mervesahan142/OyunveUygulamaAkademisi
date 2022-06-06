using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    //Sahneler arasındaki geçişleri ve kayıt işlemlerini sağlayan scripttir.

    // Start is called before the first frame update
    void Start()
    {
         Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        //Gri tuşuna basıldığında ana menüye döner
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Karakter Trigger'a giriş yaptığında ileri sahneye veya geri sahneye gidebilmektedir. Bu fonksiyon, bu yüzden hangi sahneye giriş yapmak istediğini düzenlemektedir.
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
        //Ana menüye geçiş yapar
        Application.LoadLevel(0);
    }

    public void ResetAndMainMenu(){
        //Tüm kayıtları resetlemektedir.
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
        //Oyun bitti ekranndan en son kaldığı yerden başlatmak amacıyla yazılmıştır.
        if(PlayerPrefs.GetInt("spokeWithKing") == 1){
            Application.LoadLevel(4);
        }else if(PlayerPrefs.GetInt("spokeWithWarrior") == 1){
            Application.LoadLevel(3);
        }else if(PlayerPrefs.GetInt("spokeWithKnight") == 1){
            Application.LoadLevel(2);
        }else if(PlayerPrefs.GetInt("spokeWithFreeKnight_1") == 1){
            Application.LoadLevel(1);
        }else{
             Application.LoadLevel(1);
        }
    }

    public void GameOver(){
        //Oyun bitti ekranına giriş yapmaktadır.
        Application.LoadLevel(6);
    }

}
