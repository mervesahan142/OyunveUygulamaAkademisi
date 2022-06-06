using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEnemyAIGroup_B : MonoBehaviour
{
    //Grup B NPC'leri hareketlerini ve animasyonlarını yöneten script.

    public float attackRateTime = 2;
    bool isActive, isPatroling, isWalking, isIdle, isAttacking;
    Transform friendTransform;
    float attackDistance = 10;

    void Start()
    {
        PatrolAI();
    }

    void Update()
    {

    }

    void PatrolAI(){
        //Platform üzerinde rasgele hareket ettirtmektedir.
        float state = Random.Range(0,2);
        if(state > 0.5f){
            GetComponent<NPCManagerGroup_B>().Patrol();
            isPatroling = true;
            isIdle = false;
            if(Random.Range(0,3) > 1f){
                GetComponent<NPCManagerGroup_B>().Flip();
            }
        }else{
            GetComponent<NPCManagerGroup_B>().BeIdle();
            isPatroling = false;
            isIdle = true;
        }
        if(!isActive){
            Invoke("PatrolAI", Random.Range(1,4));
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(!isActive && (other.gameObject.tag == "EnemyGroup_A" || other.gameObject.tag == "EnemyGroup_B")){
            CancelInvoke("PatrolAI");
            PatrolAI();
            GetComponent<NPCManagerGroup_B>().Flip();
        }
        if(other.gameObject.tag == "Apple" || other.gameObject.tag == "Cheese" || other.gameObject.tag == "Meat" || other.gameObject.tag == "Drink"){
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Karakter veya dost grubu A görüş alanına girdiğinde saldırmayı yönetmektedir. 
        if(other.tag == "Player" || other.tag == "FriendGroup_A"){
            isActive = true;
            GetComponent<NPCManagerGroup_B>().BeActive();
            CancelInvoke("PatrolAI");
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        //Karakter veya dost grubu A görüş alanına girdiğinde takibi sağlamaktadır.
        if(other.tag == "Player" || other.tag == "FriendGroup_A"){
            isActive = true;
            if(Mathf.Abs(transform.position.x - other.GetComponent<Transform>().position.x) < attackDistance){
                if(!isAttacking) {
                    isAttacking = true;
                    GetComponent<NPCManagerGroup_B>().MakeAttack();
                    Invoke("FinishTheTime", attackRateTime);
                }
            }else{
                GetComponent<NPCManagerGroup_B>().BeActive();
            }
            if(transform.localScale.x > 0){
                if(transform.position.x > other.gameObject.GetComponent<Transform>().position.x){
                    GetComponent<NPCManagerGroup_B>().Flip();
                }
            }else{
                if(transform.position.x < other.gameObject.GetComponent<Transform>().position.x){
                    GetComponent<NPCManagerGroup_B>().Flip();
                }
            }
        }
    }

    void FinishTheTime(){
        isAttacking = false;
    }

    private void OnTriggerExit2D(Collider2D other) {
        //Karakter veya dost grubu A görüş alanından çıktığında tekrar devriye durumuna döndürür.
        if(other.tag == "Player" || other.tag == "FriendGroup_A"){
            isActive = false;
            friendTransform = null;
            GetComponent<NPCManagerGroup_B>().BeNotActive();
            Invoke("PatrolAI",Random.Range(0,5));
        }
    }
}
