using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagerGroup_A : MonoBehaviour
{
    //Grup A NPC'lerin hareketlerinin, animasyonlarının ve özelliklerinin bulunduğu scripttir.

    public float characterMoveSpeed = 2f, attackRange = 0.5f, xpPoint = 0;
    public int[] attackDamage;
    public Transform attackPoint, groundDetection;
    public LayerMask attackLayers;

    public int health = 100;
    public GameObject[] foods = new GameObject[4];

    bool isFriend, isActive, isDead, isTakingDamage, isAttacking, isIdle, isPatroling, isMovingRight = true;
    //for friends bool
    bool isSpeaking;
    Animations animations = new Animations();
    float deadTime = 0, halfAttack_1Time = 0;
    GameObject sound;

    string gameObjectName;
    //Saldırı menzilini göstermektedir.
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);    
    }
    
    void Awake() {
        sound = GameObject.Find("Sound");

        gameObjectName = gameObject.name.Substring(0,gameObject.name.Length - 4);
        if(gameObjectName == "FreeKnight_1" || gameObjectName == "FreeKnight_2" || gameObjectName == "HeavyBandit" || gameObjectName == "King" || gameObjectName == "Knight" || gameObjectName == "LightBandit" || gameObjectName == "Warrior"){
            isFriend = true;
        }
        foreach(AnimationClip clip in GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if(clip.name == gameObjectName + "_Dead"){
                deadTime = clip.length;
            }else if(clip.name == gameObjectName + "_Attack_1"){
                halfAttack_1Time = clip.length / 2;
            }
        }
    }

    void Start()
    {
       
    }
    
    void Update()
    {
        //Düşman ile dost bu scripti kullanmaktadır. O yüzden isSpeaking durumu vardır.
        if(!isSpeaking){
            //Dost görüş alanı için Trigger kullanılmıştır. Bu yüzden dost TriggerStay fonksiyonunda algılayabilmsi için rasgele bir konum değiştirme ekleyebilmiştir bu amaç ile Translate kullanılmıştır.
            transform.Translate(new Vector3(Random.Range(-0.001f,0.001f),0,0));
            if(isPatroling || isActive){
                if(!isTakingDamage && !isAttacking && !isDead && !isIdle){
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
                case 2:
                    animations.Attack_3(GetComponent<Animator>());
                    break;
            }
            sound.GetComponent<Sounds>().Attack();
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
                    CancelInvoke("ShowDamageAnim");
                    Invoke("Die", deadTime);
                }
            }
            if(!isDead && attackDamage >= 35){
                health = health - attackDamage;
                if(health > 0){
                    Invoke("ShowDamageAnim", 0.5f);
                }else{
                    isDead = true;
                    CancelInvoke("ShowDamageAnim");
                    Invoke("Die", deadTime);
                }
            }
            if(!isDead && attackDamage >= 45){
                health = health - attackDamage;
                if(health > 0){
                    Invoke("ShowDamageAnim", 1f);
                }else{
                    isDead = true;
                    CancelInvoke("ShowDamageAnim");
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
                    if(isByChar){
                        Camera.main.GetComponent<SkillManagerandUI>().EarnXp((int)xpPoint);
                    }
                }
            }
        }
        
    }

    void ShowDamageAnim(){
        animations.TakeDamage(GetComponent<Animator>());
        sound.GetComponent<Sounds>().Attack();
        isTakingDamage = false;
    }

    void Die(){
        InitialValues();
        animations.Dead(GetComponent<Animator>());
        if(gameObject.name.Contains("emon")){
            sound.GetComponent<Sounds>().EnemyDie();
        }else{
            sound.GetComponent<Sounds>().Die();
        }
        Camera.main.GetComponent<SkillManagerandUI>().EarnXp((int)xpPoint);
        foreach(AnimationClip clip in GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if(clip.name == gameObjectName + "_Dead"){
                Invoke("WaitDeadAnimation",clip.length + 0.1f);
                break;
            }
        }
        CreateFood();
    }

    //Düşman veya dost öldüğün rasgele yiyecek üretmektedir.
    void CreateFood(){
        float state = Random.Range(0.0f,1.0f);
        Debug.Log("state" + state);
        if(state > 0.5f){
            if(state < 0.6f){
                Instantiate(foods[0], new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
            }else if(state < 0.7f){
                Instantiate(foods[1], new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
            }else if(state < 0.8f){
                Instantiate(foods[2], new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
            }else if(state < 1f){ 
                Instantiate(foods[3], new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
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
    }
    
}
