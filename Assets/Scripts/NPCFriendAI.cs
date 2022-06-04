using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPCFriendAI : MonoBehaviour
{
    public float attackRateTime = 2;
    bool isActive, isPatroling, isWalking, isIdle, isAttacking;
    Transform friendTransform;
    float attackDistance = 2.5f;
    GameObject hero;
    public string[] friendWords;
    public string[] heroWords;
    int speechController = 0;

    public GameObject speechArea;
    public Text text;

    void Awake(){
        hero = GameObject.Find("Hero");
        //speechArea.SetActive(false);
    }

    void Start()
    {
        //PatrolAI();
    }

    void Update()
    {

    }

    /*void PatrolAI(){
        if(Random.Range(0,2) > 0.5f){
            GetComponent<NPCManagerGroup_A>().Patrol();
            isPatroling = true;
            isIdle = false;
            if(Random.Range(0,3) > 1f){
                GetComponent<NPCManagerGroup_A>().Flip();
            }
        }else{
            GetComponent<NPCManagerGroup_A>().BeIdle();
            isPatroling = false;
            isIdle = true;
        }
        if(!isActive){
            Invoke("PatrolAI", Random.Range(1,4));
        }
    }*/

    void OnCollisionEnter2D(Collision2D other) {
        /*if(!isActive && (other.gameObject.tag == "Player" || other.gameObject.tag == "FriendGroup_A")){
            //CancelInvoke("PatrolAI");
            //PatrolAI();
            GetComponent<NPCManagerGroup_A>().Flip();
        }*/
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "EnemyGroup_A" || other.tag == "EnemyGroup_B"){
            isActive = true;
            GetComponent<NPCManagerGroup_A>().BeActive();
            //CancelInvoke("PatrolAI");
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "EnemyGroup_A" || other.tag == "EnemyGroup_B"){
            isActive = true;
            if(Mathf.Abs(transform.position.x - other.GetComponent<Transform>().position.x) < attackDistance){
                if(!isAttacking) {
                    isAttacking = true;
                    GetComponent<NPCManagerGroup_A>().MakeAttack();
                    Invoke("FinishTheTime", attackRateTime);
                }
            }else{
                GetComponent<NPCManagerGroup_A>().BeActive();
            }
            if(transform.localScale.x > 0){
                if(transform.position.x > other.gameObject.GetComponent<Transform>().position.x){
                    GetComponent<NPCManagerGroup_A>().Flip();
                }
            }else{
                if(transform.position.x < other.gameObject.GetComponent<Transform>().position.x){
                    GetComponent<NPCManagerGroup_A>().Flip();
                }
            }
        }
    }

    void FinishTheTime(){
        isAttacking = false;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "EnemyGroup_A" || other.tag == "EnemyGroup_B"){
            isActive = false;
            friendTransform = null;
            GetComponent<NPCManagerGroup_A>().BeNotActive();
            //Invoke("PatrolAI",Random.Range(0,5));
        }
    }

    public void Speak(){
        GetComponent<NPCManagerGroup_A>().Speak();
        speechArea.SetActive(true);
        ManageSpeak();
    }

    bool isSpeakingFriend = true;
    void ManageSpeak(){
        if(isSpeakingFriend){
            text.color = Color.red;
            text.text = friendWords[speechController];
            isSpeakingFriend = false;
        }else{
            text.color = Color.white;
            text.text = heroWords[speechController];
            isSpeakingFriend = true;
            speechController++;
        }
        if(friendWords.Length > speechController){
            Invoke("ManageSpeak", 1);
        }else{
            FinishSpeech();
        }
    }

    void FinishSpeech(){
        GetComponent<NPCManagerGroup_A>().FinishSpeech();
        hero.GetComponent<MainCharacterController>().FinishSpeech(gameObject.name);
        speechArea.SetActive(false);
    }

    
}
