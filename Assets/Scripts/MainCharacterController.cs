using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainCharacterController : MonoBehaviour
{
    public float characterMoveSpeed = 2f, characterDodgeSpeed = 4f ,attackRange = 0.5f;
    public int attackLevel = 1;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    Animations anim = new Animations();
    bool isSpeaking, isMovingRight, isMovingLeft, isAttacking, isGuarding, isJumping, isDodging, isGrounded, isTakingDamage, isDead, isAnimationFinished = true;
    int health = 100, attackDamage = 1;
    int spokeWithFreeKnight_1, spokeWithKnight, spokeWithWarrior, spokeWithKing;

    GameObject speechButton, friendGameObject;

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);  
    }
    
    void Awake()
    {
        /*spokeWithFreeKnight_1 = PlayerPrefs.GetInt("spokeWithFreeKnight_1");
        spokeWithKnight = PlayerPrefs.GetInt("spokeWithKnight");
        spokeWithWarrior = PlayerPrefs.GetInt("spokeWithWarrior");
        spokeWithKing = PlayerPrefs.GetInt("spokeWithKing");

        attackLevel = PlayerPrefs.GetInt("attackLevel");*/

        speechButton = GameObject.Find("SpeechButton");
        speechButton.SetActive(false);

        switch(attackLevel){
            case 1:
                attackDamage = 25;
                break;
            case 2:
                attackDamage = 35;
                break;
            case 3:
                attackDamage = 45;
                break;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(!isSpeaking){
            transform.Translate(new Vector3(Random.Range(-0.01f,0.01f),0,0));
            if(!isTakingDamage && !isDead){
                //Defence
                if(isGuarding){
                    anim.Defence(GetComponent<Animator>());
                }else{
                    anim.NoDefence(GetComponent<Animator>());
                }
                //walking
                if(!isGuarding){
                    if(Input.GetKey("d") || isMovingRight){
                        transform.localScale = new Vector3(5,5,1);
                        transform.position += new Vector3(characterMoveSpeed * Time.deltaTime,0,0);
                        anim.Walk(GetComponent<Animator>());
                    }else if(Input.GetKey("a") || isMovingLeft){
                        transform.localScale = new Vector3(-5,5,1);
                        transform.position -= new Vector3(characterMoveSpeed * Time.deltaTime,0,0);
                        anim.Walk(GetComponent<Animator>());
                    }else{
                        anim.NotWalk(GetComponent<Animator>());
                    }
                }
                else{
                    anim.NotWalk(GetComponent<Animator>());
                }
                //attack
                if((isAttacking && !isGuarding) && isAnimationFinished){
                    isAnimationFinished = false;
                    switch(attackLevel){
                        case 1:
                            anim.Attack_1(GetComponent<Animator>());
                            Invoke("FinishAnimation",0.5f);
                            break;
                        case 2:
                            anim.Attack_2(GetComponent<Animator>());
                            Invoke("FinishAnimation",0.75f);
                            break;
                        case 3:
                            anim.Attack_3(GetComponent<Animator>());
                            Invoke("FinishAnimation",0.85f);
                            break;
                        default:
                            break;
                    }
                    //attack to enemy
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
                    foreach(Collider2D enemy in hitEnemies){
                        if(enemy.tag == "EnemyGroup_A"){
                            enemy.GetComponent<NPCManagerGroup_A>().TakeDamage(attackDamage,0,true);
                        }else if(enemy.tag == "EnemyGroup_B"){
                            enemy.GetComponent<NPCManagerGroup_B>().TakeDamage(attackDamage,0,true);
                        }
                        
                    }
                    isTakingDamage = false;
                }
                //Jump
                if(isJumping && anim.GetGrounded(GetComponent<Animator>())){
                    isGrounded = false;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
                    anim.Jump(GetComponent<Animator>());
                }
                //Dodge
                if(isDodging && isAnimationFinished){
                    isAnimationFinished = false;
                    anim.Dodge(GetComponent<Animator>());
                    Invoke("FinishAnimation",0.66f);
                }
                if(isDodging){
                    if(transform.localScale.x > 0){
                        transform.position += new Vector3(characterDodgeSpeed * Time.deltaTime,0,0);
                    }else{
                        transform.position -= new Vector3(characterDodgeSpeed * Time.deltaTime,0,0);
                    }
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "EnemyFire"){
            health = health - other.gameObject.GetComponent<Fire>().damage;
            if(isGuarding){
                anim.Repel(GetComponent<Animator>());
            }else{
                anim.TakeDamage(GetComponent<Animator>());
            }
        }
        if(other.gameObject.tag == "Ground"){
            isGrounded = true;
            anim.Ground(GetComponent<Animator>());
        }
    }

    void OnCollisionExit2D(Collision2D other){
        if(other.gameObject.tag == "Ground"){
            isGrounded = false;
            anim.NotGround(GetComponent<Animator>());
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "FriendGroup_A"){
            switch(other.name){
                case "FreeKnight_1":
                    if(spokeWithFreeKnight_1 == 0){
                        speechButton.SetActive(true);
                        friendGameObject = other.gameObject;
                    }
                    break;
                case "Knight":
                    if(spokeWithKnight == 0){
                        speechButton.SetActive(true);
                        friendGameObject = other.gameObject;
                    }
                    break;
                case "Warrior":
                    if(spokeWithWarrior == 0){
                        speechButton.SetActive(true);
                        friendGameObject = other.gameObject;
                    }
                    break;
                case "King":
                    if(spokeWithKing == 0){
                        speechButton.SetActive(true);
                        friendGameObject = other.gameObject;
                    }
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "FriendGroup_A"){
            speechButton.SetActive(false);
        }
    }

    void FinishAnimation(){
        isAnimationFinished = true;
        isDodging = false;
        isAttacking = false;
    }
    
    int enemyAttackDamage = 0;
    public void TakeDamage(int attackDamage, float attackTime, bool isByChar){
        if(!isDead && !isTakingDamage){
            Invoke("ShowDamageAnim", attackTime);
            enemyAttackDamage = attackDamage;
            isTakingDamage = true;
        }
    }

    void ShowDamageAnim(){
        if(isTakingDamage){
            if(isGuarding){
                anim.Repel(GetComponent<Animator>());
            }else{
                health = health - enemyAttackDamage;
                Debug.Log(health);
                if(health > 0){
                    anim.TakeDamage(GetComponent<Animator>());
                }else{
                    anim.Dead(GetComponent<Animator>());
                    isDead = true;
                }
            }
            isTakingDamage = false;
        }
    }

    void Die(){
        isDead = true;
        anim.Dead(GetComponent<Animator>());
    }

    /*--------------BUTTONS------------------*/
    public void Speak(){
        isSpeaking = true;
        friendGameObject.GetComponent<NPCFriendAI>().Speak();
        speechButton.SetActive(false);
    }

    public void FinishSpeech(string friendName){
        isSpeaking = false;
        switch(friendName){
            case "FreeKnight_1":
                spokeWithFreeKnight_1 = 1;
                //PlayerPrefs.SetInt("spokeWithFreeKnight_1",1);
                break;
            case "Knight":
                spokeWithKnight = 1;
                //PlayerPrefs.SetInt("Knight",1);
                break;
            case "Warrior":
                spokeWithWarrior = 1;
                //PlayerPrefs.SetInt("Warrior",1);
                break;
            case "King":
                spokeWithKing = 1;
                //PlayerPrefs.SetInt("King",1);
                break;
        }
        speechButton.SetActive(false);
    }

    public void RightButtonDown(){
        isMovingRight = true;
        isMovingLeft = false;
    }

    public void LeftButtonDown(){
        isMovingRight = false;
        isMovingLeft = true;
    }

    public void HorizontalButtonsUp(){
        isMovingRight = false;
        isMovingLeft = false;
    }

    public void GuardButtonDown(){
        isGuarding = true;
    }

    public void GuardButtonUp(){
        isGuarding = false;
    }

    public void AttackButtonDown(){
        isAttacking = true;
    }

    public void AttackButtonUp(){
        isAttacking = false;
    }

    public void JumpButtonDown(){
        isJumping = true;
    }

    public void JumpButtonUp(){
        isJumping = false;
    }

    public void DodgeButtonDown(){
        isDodging = true;
    }

    public void DodgeButtonUp(){
        isDodging = false;
    }
}
