using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPlants : MonoBehaviour
{
    public float fireTime = 0;
    
    public GameObject fire;
    Animations animations = new Animations();
    float firePosX = 0, firePosY = 0, deadTime = 0;
    
    void Start()
    {
        if(gameObject.name == "Plant_1"){
            firePosY = 0.9f;
        }else if(gameObject.name == "Plant_2"){
            firePosX = 0.82f; firePosY = -0.22f;
        }
        InvokeRepeating("MakeAttack",3f,3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeAttack(){
        animations.Attack_1(GetComponent<Animator>());
        Invoke("Fire",fireTime);
    }

    void Fire(){
        if(transform.localScale.x < 0){
            Instantiate(fire, new Vector3(transform.position.x - firePosX, transform.position.y + firePosY, 0),  Quaternion.identity).transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }else{
            Instantiate(fire, new Vector3(transform.position.x + firePosX, transform.position.y + firePosY, 0),  Quaternion.identity).transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }
    }
}
