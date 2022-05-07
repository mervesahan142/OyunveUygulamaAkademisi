using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    Button conversationButton;
    Image conversationArea;
    Text conversationText;

    //ConversationManager cm = new ConversationManager();
    string[] playerWords = new string[] {"Hello there","I look for Masimo. Do you know him?"};
    string[] mayorWords = new string[] {"Hey... Welcome to our village adventurer.","Ye, he must be at his home."};
    int playerMayorSpeechCounter = 0;

    private void Awake() {
        conversationButton = GameObject.Find("ConversationButton").GetComponent<Button>();
        conversationButton.gameObject.SetActive(false);
        conversationText = GameObject.Find("ConversationText").GetComponent<Text>();
        conversationArea = GameObject.Find("ConversationArea").GetComponent<Image>();
        conversationArea.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            //cm.ActiveButton();
            conversationButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player"){
            //cm.PasiveButton();
            conversationButton.gameObject.SetActive(false);
        }
    }

    public void Speak(){
        //cm.Speak(this.gameObject.name);
        if(playerMayorSpeechCounter < 2){
            conversationArea.gameObject.SetActive(true);
            conversationButton.gameObject.SetActive(false);
            //player
            conversationText.color = Color.white;
            conversationText.text = playerWords[playerMayorSpeechCounter];
            Invoke("SpeakMayor",2f);
        }else{
            conversationArea.gameObject.SetActive(false);
            conversationButton.gameObject.SetActive(true);
            playerMayorSpeechCounter = 0;
        }
    }

    private void SpeakMayor(){
        //Mayor
        conversationText.color = Color.green;
        conversationText.text = mayorWords[playerMayorSpeechCounter];
        playerMayorSpeechCounter++;
        Invoke("Speak",2f);
    }
    
}
