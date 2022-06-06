using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainCharacterController : MonoBehaviour
{
    /*
        Oyuncunun karakteri kontrol ettiği scripttir.
    */

    public float characterMoveSpeed = 2f, characterDodgeSpeed = 2f ,attackRange = 0.5f;
    public int attackLevel = 1, enduranceLevel = 1;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    Animations anim = new Animations();
    
    bool isSpeaking, isMovingRight, isMovingLeft, isAttacking, isGuarding, isJumping, isDodging, isGrounded, isTakingDamage, isDead, isAnimationFinished = true;
    int health = 100, attackDamage = 1;
    int spokeWithFreeKnight_1, spokeWithKnight, spokeWithWarrior, spokeWithKing;

    GameObject speechButton, friendGameObject, sound;
    Image healthBar;

    //Attack menzilini tanımlamak için kullanılmıştır.
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
    void Awake()
    {
        //Oyun kapanıp tekrar açıldığında karakteer ile ilgili bir veri kaybı olamaması için PlayerPrefs kullanılmıştır.
        spokeWithFreeKnight_1 = PlayerPrefs.GetInt("spokeWithFreeKnight_1");
        spokeWithKnight = PlayerPrefs.GetInt("spokeWithKnight");
        spokeWithWarrior = PlayerPrefs.GetInt("spokeWithWarrior");
        spokeWithKing = PlayerPrefs.GetInt("spokeWithKing");

        attackLevel = PlayerPrefs.GetInt("attackLevel");
        enduranceLevel = PlayerPrefs.GetInt("enduranceLevel");
        characterMoveSpeed = PlayerPrefs.GetInt("moveSpeedLevel") + 1;
        characterDodgeSpeed = PlayerPrefs.GetInt("dodgeSpeedLevel") * 2;
        attackRange = PlayerPrefs.GetInt("attackRangeLevel") / 3f;

        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        speechButton = GameObject.Find("SpeechButton");
        speechButton.SetActive(false);

        sound = GameObject.Find("Sound");

        //attack leveline göre karakterin attack hasarı atanmıştır.
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
        //Periyodik olarak yütüme sesini duymak için kullanılmıştır. WalkSound fonksiyonunun içerisindeki bir koşul ile yürüdüğünü anlayıp ses verektedir.
        InvokeRepeating("WalkSound", 0.5f, 0.5f);
    }

    void WalkSound(){
        if((isMovingRight || isMovingLeft) && isGrounded){
            sound.GetComponent<Sounds>().HeroWalk();
        }
    }

    void Update()
    {
        if(!isSpeaking){
            //Düşman görüş alanı için Trigger kullanılmıştır. Bu yüzden düşman TriggerStay fonksiyonunda algılayabilmsi için rasgele bir konum değiştirme ekleyebilmiştir bu amaç ile Translate kullanılmıştır..
            transform.Translate(new Vector3(Random.Range(-0.0001f,0.0001f),0,0));
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
                    AttackSound();
                    //attack to enemy
                    //Karakter attak yaptığında ve bir düşman menzil içindeyse o düşmanın canını azaltmak için kullanılmıştır.
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
                    foreach(Collider2D enemy in hitEnemies){
                        if(Mathf.Abs(enemy.transform.position.x - transform.position.x) < attackRange * 6){
                            if(enemy.tag == "EnemyGroup_A"){
                                //Düşmanın canını azaltmak için düşman script'deki TakeDamage fonksiyonunu çalıştırmaktadır.
                                enemy.GetComponent<NPCManagerGroup_A>().TakeDamage(attackDamage,0,true);
                            }else if(enemy.tag == "EnemyGroup_B"){
                                enemy.GetComponent<NPCManagerGroup_B>().TakeDamage(attackDamage,0,true);
                            }
                        }
                    }
                    isTakingDamage = false;
                }
                //Jump
                if(isJumping && anim.GetGrounded(GetComponent<Animator>())){
                    isGrounded = false;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 7.5f), ForceMode2D.Impulse);
                    anim.Jump(GetComponent<Animator>());
                    sound.GetComponent<Sounds>().HeroJump();
                }
                //Dodge
                if(isDodging && isAnimationFinished){
                    isAnimationFinished = false;
                    anim.Dodge(GetComponent<Animator>());
                    sound.GetComponent<Sounds>().HeroDodge();
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

    void LateUpdate() {
        Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        //Aşağıya düştüğünde karakterin ölmesi için yazılmıştır.
        if(!isDead && transform.position.y < -10){
            Die();
        }
    }

    int soundCount = 1;
    void AttackSound(){
        //Saldırı sesi bir tanedir ama animasyonda ard ardadır. Bu yüzen attackLevel sayısına kadar sesi çalıştırması gerekmektedir.
        sound.GetComponent<Sounds>().HeroAttack();
        if(attackLevel == soundCount){
            soundCount = 1;
        }else{
            Invoke("AttackSound",0.5f);
            soundCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "EnemyFire"){
            if(isGuarding){
                anim.Repel(GetComponent<Animator>());
                sound.GetComponent<Sounds>().HeroShieldGuard();
            }else{
                TakeDamage(other.gameObject.GetComponent<Fire>().damage, 0, false);
                sound.GetComponent<Sounds>().HeroHit();
            }
        }
        if(other.gameObject.tag == "Ground"){
            isGrounded = true;
            anim.Ground(GetComponent<Animator>());
        }
        //Düşmanlardan düşen yiyecelkleri toplayarak can kazanabilir. O yüzden yiyeceğin türüne göre can doldurması için kullanılmıştır.
        switch(other.gameObject.tag){
            case "Apple":
                health = health + 20;
                if(health > 100){
                    health = 100;
                }
                healthBar.rectTransform.sizeDelta = new Vector2(2.7f * health, 27.2155f);
                Destroy(other.gameObject);
                sound.GetComponent<Sounds>().HeroHealth();
                break;
            case "Cheese":
                health = health + 25;
                if(health > 100){
                    health = 100;
                }
                healthBar.rectTransform.sizeDelta = new Vector2(2.7f * health, 27.2155f);
                Destroy(other.gameObject);
                sound.GetComponent<Sounds>().HeroHealth();
                break;
            case "Meat":
                health = health + 40;
                if(health > 100){
                    health = 100;
                }
                healthBar.rectTransform.sizeDelta = new Vector2(2.7f * health, 27.2155f);
                Destroy(other.gameObject);
                sound.GetComponent<Sounds>().HeroHealth();
                break;
            case "Drink":
                health = health + 10;
                if(health > 100){
                    health = 100;
                }
                healthBar.rectTransform.sizeDelta = new Vector2(2.7f * health, 27.2155f);
                Destroy(other.gameObject);
                sound.GetComponent<Sounds>().HeroHealth();
                break;
        }
    }

    void OnCollisionExit2D(Collision2D other){
        if(other.gameObject.tag == "Ground"){
            isGrounded = false;
            anim.NotGround(GetComponent<Animator>());
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        //Dost olan npcler ile konuşma amacıyla kullanılmıştır.
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
        //Düşmandan gelen hasarı almak için kullanılmıştır.
        if(!isDead && !isTakingDamage){
            Invoke("ShowDamageAnim", attackTime);
            enemyAttackDamage = (int)attackDamage / enduranceLevel;
            isTakingDamage = true;
        }
    }

    void ShowDamageAnim(){
        if(isTakingDamage){
            if(isGuarding){
                anim.Repel(GetComponent<Animator>());
            }else{
                health = health - enemyAttackDamage;
                healthBar.rectTransform.sizeDelta = new Vector2(2.7f * health, 27.2155f);
                if(health > 0){
                    anim.TakeDamage(GetComponent<Animator>());
                }else{
                    Die();
                }
            }
            isTakingDamage = false;
        }
    }

    void Die(){
        isDead = true;
        anim.Dead(GetComponent<Animator>());
        sound.GetComponent<Sounds>().Die();
        Invoke("GameOver", 1.0f);
    }

    void GameOver(){
        //Oyun bitti ekranına gönderir
        Camera.main.GetComponent<ScenesManager>().GameOver();
    }

    public void UpdateSkills(){
        //Toplanan puanları kullanarak yeteneği gelişmektedir gelişen yetenekleri kaydetmektedir.
        attackLevel = PlayerPrefs.GetInt("attackLevel");
        enduranceLevel = PlayerPrefs.GetInt("enduranceLevel");
        characterMoveSpeed = PlayerPrefs.GetInt("moveSpeedLevel") + 1;
        characterDodgeSpeed = PlayerPrefs.GetInt("dodgeSpeedLevel") * 2;
        attackRange = PlayerPrefs.GetInt("attackRangeLevel") / 2f;
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
                PlayerPrefs.SetInt("spokeWithFreeKnight_1",spokeWithFreeKnight_1);
                break;
            case "Knight":
                spokeWithKnight = 1;
                PlayerPrefs.SetInt("spokeWithKnight",spokeWithFreeKnight_1);
                break;
            case "Warrior":
                spokeWithWarrior = 1;
                PlayerPrefs.SetInt("spokeWithWarrior",spokeWithFreeKnight_1);
                break;
            case "King":
                spokeWithKing = 1;
                PlayerPrefs.SetInt("spokeWithKing",spokeWithFreeKnight_1);
                break;
        }
        speechButton.SetActive(false);
    }

    //Buttonlar Basma ve Basmayı bıraktığı anı algılamaktadır ve bu algılamaya göre durm yönetimi yapmaktadır.

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
