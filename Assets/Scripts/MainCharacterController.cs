using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    bool isMoveRight, isMoveLeft;
    public float charakterSpeed = 2f;
    Animator anim;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("d") || isMoveRight){
            transform.localScale = new Vector3(3,3,1);
            transform.position += new Vector3(charakterSpeed * Time.deltaTime,0,0);
            anim.SetInteger("Movement",1);
        }else if(Input.GetKey("a") || isMoveLeft){
            transform.localScale = new Vector3(-3,3,1);
            transform.position -= new Vector3(charakterSpeed * Time.deltaTime,0,0);
            anim.SetInteger("Movement",1);
        }
        else{
            anim.SetInteger("Movement",0);
        }
    }

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
