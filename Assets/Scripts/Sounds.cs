using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sounds : MonoBehaviour
{   
    //Oyun seslerini yöneten scripttir.

    public AudioClip[] audioClips;
    public AudioClip[] musicClips;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        switch(SceneManager.GetActiveScene().name){
            case "Forest":
                audioSource.clip = musicClips[0];
                break;
            case "village":
                audioSource.clip = musicClips[1];
                break;
            case "Cave":
                //audioSource.clip = musicClips[0];
                break;
            case "Castle":
                audioSource.clip = musicClips[2];
                break;
            default:
                audioSource.clip = musicClips[0];
                break;
        }
        audioSource.volume = 0.5f;
        MusicPlay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicPlay(){
        audioSource.Play();
    }

    public void MusicPause(){
        audioSource.Pause();
    }

    public void Click(){
        audioSource.PlayOneShot(audioClips[1],1.0f);
    }

    public void Attack(){
        audioSource.PlayOneShot(audioClips[12],1.0f);
    }

    public void Die(){
        audioSource.PlayOneShot(audioClips[2],1.0f);
    }

    /*----------HERO----------*/
    public void HeroAttack(){
        audioSource.PlayOneShot(audioClips[13],1.0f);
    }

    public void HeroDodge(){
        audioSource.PlayOneShot(audioClips[4],1.0f);
    }

    public void HeroJump(){
        audioSource.PlayOneShot(audioClips[7],1.0f);
    }

    public void HeroWalk(){
        audioSource.PlayOneShot(audioClips[11],1.0f);
    }

    public void HeroShieldGuard(){
        audioSource.PlayOneShot(audioClips[10],1.0f);
    }

    public void HeroHit(){
        audioSource.PlayOneShot(audioClips[8],1.0f);
    }

    public void HeroHealth(){
        audioSource.PlayOneShot(audioClips[6],1.0f);
    }
    /*---------------------*/

    public void EnemyDie(){
        audioSource.PlayOneShot(audioClips[3],1.0f);
    }

    public void EnemyFire(){
        audioSource.PlayOneShot(audioClips[5],1.0f);
    }

    
}
