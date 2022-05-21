using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagerGroup_A : MonoBehaviour
{
    bool isFriend;
    Animations animartions = new Animations();
    public int health = 100;
    
    void Awake() {
        if(gameObject.name == "FreeKnight_1" || gameObject.name == "FreeKnight_2" || gameObject.name == "HeavyBandit" || gameObject.name == "King" || gameObject.name == "Knight" || gameObject.name == "LightBandit" || gameObject.name == "Warrior"){
            isFriend = true;
            Debug.Log(gameObject.name + " is Friend");
        }
    }

    void Start()
    {
        animartions.Walk(GetComponent<Animator>());
    }

    
    void Update()
    {

    }
}
