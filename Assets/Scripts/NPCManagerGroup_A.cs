using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagerGroup_A : MonoBehaviour
{
    public float characterMoveSpeed = 2f, attackRange = 0.5f;
    public int[] attackDamage;
    public Transform attackPoint, groundDetection;
    public LayerMask attackLayers;

    public int health = 100;
    bool isFriend, isActive, isDead, isTakingDamage, isAttacking, isIdle, isPatroling, isMovingRight = true;
    //for friends bool
    bool isSpeaking;
    Animations animations = new Animations();
    float deadTime = 0, halfAttack_1Time = 0;

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);    
    }
    
    void Awake() {
        if(gameObject.name == "FreeKnight_1" || gameObject.name == "FreeKnight_2" || gameObject.name == "HeavyBandit" || gameObject.name == "King" || gameObject.name == "Knight" || gameObject.name == "LightBandit" || gameObject.name == "Warrior"){
            isFriend = true;
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
       
    }
    
    void Update()
    {
        if(!isSpeaking){
            transform.Translate(new Vector3(Random.Range(-0.001f,0.001f),0,0));
            /*if(isFriend){
                if(isActive){

                }else{

                }
            }else{*/
                if(isPatroling || isActive){
                    if(!isTakingDamage && !isAttacking && !isDead && !isIdle /*&& Physics2D.Raycast(groundDetection.position, Vector2.down, 0).collider*/){
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
            //}
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

    //For Friends-------
    public void Speak(){
        isSpeaking = true;
    }

    public void FinishSpeech(){
        isSpeaking = false;
    }
    //------------------

    public void Flip(){
        if(isMovingRight){
            isMovingRight = false;
        }else{
            isMovingRight = true;
        }
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void MakeAttack(){
        InitialValues();
        isAttacking = true;
        if(!isDead){
            int whichAttack = (int)Random.Range(0, attackDamage.Length);
            switch(whichAttack){
                case 0:
                    animations.Attack_1(GetComponent<Animator>());
                    break;
                case 1:
                    animations.Attack_2(GetComponent<Animator>());
                    break;
            }
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackLayers);
            foreach(Collider2D hit in hits){
                if(hit.tag == "Player"){
                    hit.GetComponent<MainCharacterController>().TakeDamage(attackDamage[whichAttack], halfAttack_1Time, false);
                }else if(hit.tag == "FriendGroup_A" || hit.tag == "EnemyGroup_A"){
                    hit.GetComponent<NPCManagerGroup_A>().TakeDamage(attackDamage[whichAttack], halfAttack_1Time, false);
                }else if(hit.tag == "FriendGroup_B" || hit.tag == "EnemyGroup_B"){
                    hit.GetComponent<NPCManagerGroup_B>().TakeDamage(attackDamage[whichAttack], halfAttack_1Time, false);
                }
            }
        }
    }

    public void TakeDamage(int attackDamage, float attackTime, bool isByChar){
        InitialValues();
        isTakingDamage = true;
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
        }else{//attack by friend or enemy
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
        isTakingDamage = false;
    }

    void Die(){
        InitialValues();
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
