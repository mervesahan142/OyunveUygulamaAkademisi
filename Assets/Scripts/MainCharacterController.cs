using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    Animations anim1 = new Animations();
    bool isMoveRight, isMoveLeft, isAnimationFinished = true, isGrounded, isDodging;
    public float characterMoveSpeed = 2f, characterDodgeSpeed = 4f;
    public int attackLevel = 1, attackDamage;
    int health = 100;
    Animator anim;
    BoxCollider2D rangeCollider;
    
    void Awake()
    {
        //attackLevel = PlayerPrefs.GetInt("attackLevel");
        switch(attackLevel){
            case 1:
                attackDamage = 50;
                break;
            case 2:
                attackDamage = 75;
                break;
            case 3:
                attackDamage = 100;
                break;
            default:
                break;
        }
        var colliders = GetComponents<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rangeCollider =  colliders[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Defence
        if(Input.GetKey("q")){
            anim1.Defence(GetComponent<Animator>());
        }else if(Input.GetKeyUp("q")){
            anim1.NoDefence(GetComponent<Animator>());
        }
        //walking
        if(!Input.GetKey("q")){
            if(Input.GetKey("d") || isMoveRight){
                transform.localScale = new Vector3(5,5,1);
                transform.position += new Vector3(characterMoveSpeed * Time.deltaTime,0,0);
                anim1.Walk(GetComponent<Animator>());
            }else if(Input.GetKey("a") || isMoveLeft){
                transform.localScale = new Vector3(-5,5,1);
                transform.position -= new Vector3(characterMoveSpeed * Time.deltaTime,0,0);
                anim1.Walk(GetComponent<Animator>());
            }else{
                anim1.NotWalk(GetComponent<Animator>());
            }
        }
        else{
            anim1.NotWalk(GetComponent<Animator>());
        }
        //attack
        if(Input.GetKeyDown("e") && !Input.GetKey("q") && isAnimationFinished){
            switch(attackLevel){
                case 1:
                    anim1.Attack_1(GetComponent<Animator>());
                    Invoke("FinishAnimation",0.5f);
                    break;
                case 2:
                    anim1.Attack_2(GetComponent<Animator>());
                    Invoke("FinishAnimation",0.75f);
                    break;
                case 3:
                    anim1.Attack_3(GetComponent<Animator>());
                    Invoke("FinishAnimation",0.85f);
                    break;
                default:
                    break;
            }
            /*anim.SetBool("isWalking", false);
            isAnimationFinished = false;
            switch(attackLevel){
                case 1:
                    anim.SetTrigger("Attack_1");
                    Invoke("FinishAnimation",0.5f);
                    break;
                case 2:
                    anim.SetTrigger("Attack_2");
                    Invoke("FinishAnimation",0.75f);
                    break;
                case 3:
                    anim.SetTrigger("Attack_3");
                    Invoke("FinishAnimation",0.85f);
                    break;
                default:
                    break;
            }*/
        }
        //Jump
        if(Input.GetKeyDown("w") && isGrounded/*anim.GetBool("isGrounded");*/){
            anim.SetBool("isGrounded", false);
            anim.SetTrigger("Jump");
        }
        //Dodge
        if(Input.GetKeyDown("s") && isAnimationFinished){
            isAnimationFinished = false;
            isDodging = true;
            anim.SetTrigger("Dodge");
            Debug.Log(transform.localScale.x);
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

    void FinishAnimation(){
        isAnimationFinished = true;
        isDodging = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.IsTouching(rangeCollider) && collider.gameObject.tag == "Enemy")
        {
            collider.GetComponent<NPCManagerGroup_A>().health = collider.GetComponent<NPCManagerGroup_A>().health - attackDamage;
            Debug.Log("Can : " + collider.GetComponent<NPCManagerGroup_A>().health);
        }
    }

    /*--------------BUTTONS------------------*/
    public void MoveRight(){
        isMoveRight = true;
        isMoveLeft = false;
    }

    public void MoveLeft(){
        isMoveRight = false;
        isMoveLeft = true;
    }

    public void StopMoving(){
        isMoveRight = false;
        isMoveLeft = false;
    }
}
