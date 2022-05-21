using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public void Walk(Animator anim){
        anim.SetBool("isWalking", true);
    }

    public void NotWalk(Animator anim){
        anim.SetBool("isWalking", false);
    }

    public void Attack_1(Animator anim){
        anim.SetTrigger("Attack_1");
    }

    public void Attack_2(Animator anim){
        anim.SetTrigger("Attack_2");
    }

    public void Attack_3(Animator anim){
        anim.SetTrigger("Attack_3");
    }

    public void TakeDamage(Animator anim){

    }

    public void Dead(Animator anim){

    }

    public void ClearAnimationGroupA(Animator anim){

    }

    /***************char special**************/

    public void Defence(Animator anim){
        anim.SetBool("Defence", true);
    }

    public void NoDefence(Animator anim){
        anim.SetBool("Defence", false);
    }

    /*public void ClearCharAnimation(Animator anim){
        anim.SetBool("isWalking", false);
        anim.SetBool("Defence", false);
    }*/
}
