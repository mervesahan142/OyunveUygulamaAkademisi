using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    /*
        B Grubu düşmanların üretebildiği, kendine ait olan Ateş'in çalışma scripti
    */

    public float fireSpeed = 2, liveTime = 5;
    public int damage = 10;
    Animator anim;
    float deadTime = 0;
    bool isBlocked;

    void Awake(){
        anim = GetComponent<Animator>();
        //Ateşin ölüm zamanını almak için kullanılmıştır.
        foreach(AnimationClip clip in GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if(clip.name == gameObject.name.Replace("(Clone)","") + "_PutOutFire"){
                deadTime = clip.length;
            }
        }
    }

    void Start()
    {
        //Ateşi yok etmeden önce bir yaşam zamanı verilmiştir.
        Invoke("PutOutFire", liveTime);
    }

    void Update()
    {
        //Herhangi bir engel ile karşılaşmadığı sürece doğrusal bir hareket ettir.
        if(!isBlocked){
            if(transform.localScale.x < 0){
                transform.localPosition -= new Vector3(fireSpeed * Time.deltaTime, 0f, 0f);
            }else{
                transform.localPosition += new Vector3(fireSpeed * Time.deltaTime, 0f, 0f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //Herhangi bir engel ile karşılaştığında kendini yok et
        PutOutFire();
    }

    void PutOutFire(){
        isBlocked = true;
        anim.SetTrigger("PutOut");
        //Bir yok olma animasyonu varsa yok olana kadar Ateş'i yok ettirmemek için kullanılmıştır.
        Invoke("DestroyGameObject", deadTime);
    }

    void DestroyGameObject(){
        Destroy (gameObject);
    }
}
