using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagerGroup_B : MonoBehaviour
{
    public float characterMoveSpeed = 2f, fireTime = 0, firePosX = 0, firePosY = 0;
    public int health = 100;
    public GameObject fire;
    public Transform groundDetection;

    bool isFriend, isActive, isDead, isTakingDamage, isAttacking, isIdle, isPatroling, isMovingRight = true;
    Animations animations = new Animations();
    float deadTime = 0, halfAttack_1Time = 0;
    
    void Awake() {
        if(gameObject.name == "FreeKnight_1" || gameObject.name == "FreeKnight_2" || gameObject.name == "HeavyBandit" || gameObject.name == "King" || gameObject.name == "Knight" || gameObject.name == "LightBandit" || gameObject.name == "Warrior"){
            isFriend = true;
            //Debug.Log(gameObject.name + " is Friend");
        }
        foreach(AnimationClip clip in GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if(clip.name == gameObject.name + "_Dead"){
                deadTime = clip.length;
            }else if(clip.name == gameObject.name + "_Attack_1"){
                halfAttack_1Time = clip.length / 2;
            }
        }
        
    }

    void Start()
    {
        /*if(!isFriend)
            InvokeRepeating("MakeAttack",3f,3f);*/
    }
    
    void Update()
    {
        if(isPatroling || isActive){
            if(!isTakingDamage && !isAttacking && !isDead && !isIdle && Physics2D.Raycast(groundDetection.position, Vector2.down, 0).collider){
                animations.Walk(GetComponent<Animator>());
                if(isMovingRight){
                    transform.position += new Vector3(characterMoveSpeed * Time.deltaTime, 0, 0);
                }else{
                    transform.position -= new Vector3(characterMoveSpeed * Time.deltaTime, 0, 0);
                }
                if(Physics2D.Raycast(groundDetection.position, Vector2.down, 0).collider == false){
                    if(isPatroling){
                        Flip();
                    }else if(isActive){
                        BeIdle();
                    }
                }
            }else{
                animations.NotWalk(GetComponent<Animator>());
            }
        }else{
            animations.NotWalk(GetComponent<Animator>());
        }
    }

    public void BeIdle(){
        InitialValues();
        isIdle = true;
        animations.NotWalk(GetComponent<Animator>());
    }

    public void BeActive(){
        InitialValues();
        isActive = true;
    }

    public void BeNotActive(){
        InitialValues();
        isActive = false;
    }

    public void Patrol(){
        InitialValues();
        isPatroling = true;
    }

    /*void Jump(){
        GetComponent<Rigidbody2D>().AddForce(transform.up * Time.deltaTime * 100, ForceMode2D.Impulse);
    }*/

    public void Flip(){
        if(isMovingRight){
            isMovingRight = false;
        }else{
            isMovingRight = true;
        }
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void MakeAttack(){
        if(!isDead){
            InitialValues();
            isAttacking = true;
            animations.Attack_1(GetComponent<Animator>());
            Invoke("Fire",fireTime);
        }
    }

    void Fire(){
        if(transform.localScale.x < 0){
            Instantiate(fire, new Vector3(transform.position.x - firePosX, transform.position.y + firePosY, 0),  Quaternion.identity).transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }else{
            Instantiate(fire, new Vector3(transform.position.x + firePosX, transform.position.y + firePosY, 0),  Quaternion.identity).transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }
    }

    public void TakeDamage(int attackDamage, float attackTime, bool isByChar){
        if(isByChar){//attack by char
            if(!isDead && attackDamage >= 25){
                health = health - attackDamage;
                if(health > 0){
                    Invoke("ShowDamageAnim", 0.25f);
                }else{
                    isDead = true;
                    Invoke("Die", deadTime);
                }
            }
            if(!isDead && attackDamage >= 35){
                health = health - attackDamage;
                if(health > 0){
                    Invoke("ShowDamageAnim",0.5f);
                }else{
                    isDead = true;
                    Invoke("Die", deadTime);
                }
            }
            if(!isDead && attackDamage >= 45){
                health = health - attackDamage;
                if(health > 0){
                    Invoke("ShowDamageAnim",1f);
                }else{
                    isDead = true;
                    Invoke("Die", deadTime);
                }
            }
        }else{//attack by friend
            if(!isDead){
                health = health - attackDamage;
                if(health > 0){
                    Invoke("ShowDamageAnim", attackTime);
                }else{
                    isDead = true;
                    Invoke("Die", deadTime);
                }
            }
        }
        
    }

    void ShowDamageAnim(){
        animations.TakeDamage(GetComponent<Animator>());
    }

    void Die(){
        
        animations.Dead(GetComponent<Animator>());
        foreach(AnimationClip clip in GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if(clip.name == gameObject.name + "_Dead"){
                Invoke("WaitDeadAnimation",clip.length + 0.1f);
                break;
            }
        }
    }

    void WaitDeadAnimation(){
        gameObject.SetActive(false);
    }

    void InitialValues(){
        isTakingDamage = false;
        isAttacking = false;
        isIdle = false;
        isPatroling = false;
        //isActive = false;
    }
}
