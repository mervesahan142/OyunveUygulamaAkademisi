using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float fireSpeed = 2, liveTime = 5;
    public int damage = 10;
    Animator anim;
    float deadTime = 0;
    bool isBlocked;
    void Awake(){
        anim = GetComponent<Animator>();
        foreach(AnimationClip clip in GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if(clip.name == gameObject.name.Replace("(Clone)","") + "_PutOutFire"){
                deadTime = clip.length;
            }
        }
    }

    void Start()
    {
        Invoke("PutOutFire", liveTime);
    }

    void Update()
    {
        if(!isBlocked){
            if(transform.localScale.x < 0){
                transform.localPosition -= new Vector3(fireSpeed * Time.deltaTime, 0f, 0f);
            }else{
                transform.localPosition += new Vector3(fireSpeed * Time.deltaTime, 0f, 0f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        PutOutFire();
        /*if(other.gameObject.tag != "EnemyGroup_A" && other.gameObject.tag != "EnemyGroup_B"){
            
        }*/
    }

    void PutOutFire(){
        isBlocked = true;
        anim.SetTrigger("PutOut");
        Invoke("DestroyGameObject", deadTime);
    }

    void DestroyGameObject(){
        Destroy (gameObject);
    }
}
