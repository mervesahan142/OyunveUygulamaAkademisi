using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagerGroup_B : MonoBehaviour
{
    //Grup B NPC'lerin hareketlerinin, animasyonlarının ve özelliklerinin bulunduğu scripttir.

    public float characterMoveSpeed = 2f, fireTime = 0, firePosX = 0, firePosY = 0, xpPoint = 0;
    public int health = 100;
    public GameObject fire;
    public Transform groundDetection;
    public GameObject[] foods = new GameObject[4];

    bool isFriend, isActive, isDead, isTakingDamage, isAttacking, isIdle, isPatroling, isMovingRight = true;
    Animations animations = new Animations();
    float deadTime = 0, halfAttack_1Time = 0;
    GameObject sound;
    
    string gameObjectName;
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
        if(isPatroling || isActive){
            //Dost görüş alanı için Trigger kullanılmıştır. Bu yüzden dost TriggerStay fonksiyonunda algılayabilmsi için rasgele bir konum değiştirme ekleyebilmiştir bu amaç ile Translate kullanılmıştır.
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
        sound.GetComponent<Sounds>().EnemyFire();
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
    }

    void Die(){
        animations.Dead(GetComponent<Animator>());
        sound.GetComponent<Sounds>().Die();
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
        //isActive = false;
    }
}
