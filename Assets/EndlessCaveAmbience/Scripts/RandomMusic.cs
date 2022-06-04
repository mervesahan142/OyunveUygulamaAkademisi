using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class RandomMusic : MonoBehaviour
{
    [Tooltip("Immediately start the music when the object is loaded. If false, StartMusic() will need to be called instead.")]
    public bool playOnStart;
    [Tooltip("Randomises the start position of the bed. Avoids repetition when starting.")]
    public bool randomiseStartPosition;

    public AudioSource bed;
    public AudioSource melody;
    public AudioSource perc;
    public AudioSource fx;

    [Tooltip("The mixer containing the master volume group.")]
    public AudioMixer mixer;
    [Tooltip("The name of the exposed paramter used to fade the master volume group. Set this to your own exposed paramter if using with a different mixer.")]
    public string exposedParamName = "CaveMasterVol";

    [Tooltip("Time, in seconds to fade in the music on Start. Set to 0 for immediate playback.")]
    public float fadeInDuration = 3f;
    [Tooltip("Time, in seconds to fade out the music on Stop. Set to 0 for an immediate stop.")]
    public float fadeOutDuration = 3f;

    public AudioClip[] melodyArray;
    public AudioClip[] percArray;
    public AudioClip[] fxArray;

    [Tooltip("Tempo in beats per minute (default: 95), only required if using your own music.")]
    public int bpm = 95;
    [Tooltip("Time signature in beats per bar (default: 4), only required if using your own music.")]
    public int beatsPerBar = 4;
    [Tooltip("How frequently, in whole bars, that cues can be triggered.")]
    public int barsPerTrigger = 2;

    [Range(0, 100)][Tooltip("Percent chance that a melody cue wil play at the trigger point.")]
    public int melodyChance = 60;
    [Range(0, 100)][Tooltip("Percent chance that a percussion cue wil play at the trigger point.")]
    public int percChance = 40;
    [Range(0, 100)][Tooltip("Percent chance that an FX cue wil play at the trigger point.")]
    public int fxChance = 20;

    [Range(0, 1)][Tooltip("Applies an amount of panning variation to generated sounds. 0 applies no panning changes, 1 allows random panning up to the maximumum extents.")]
    public float panVariation;

    float timer;
    float triggerTime;
    bool musicPlaying;

    void Start()
    {
        triggerTime = 60f / bpm * beatsPerBar * barsPerTrigger;
        if (playOnStart == true)
            StartMusic();
    }

    void Update()
    {
        if (musicPlaying == true)
        {
            timer += Time.deltaTime;
            if (timer > triggerTime)
            {
                PlayClips();
            }
        }
    }

    public void StartMusic()
    {
        if (musicPlaying == false)
        {
            StopAllCoroutines();
            timer = 0f;
            mixer.SetFloat(exposedParamName, - 80);
            StartCoroutine(FadeMasterGroup(fadeInDuration, 1));
            if (randomiseStartPosition)
            {
                bed.time = Random.Range(0, bed.clip.length);
            }
            bed.Play();
            musicPlaying = true;
        }
        else { Debug.Log("Cannot start music, it's still playing."); }
    }

    public void StopMusic()
    {
        StartCoroutine(FadeAndStopMusic());
    }

    void PlayClips()
    {
        timer = 0f;
        int melodyRoll = Random.Range(0, 100);
        int percRoll = Random.Range(0, 100);
        int fxRoll = Random.Range(0, 100);

        if (melodyChance > melodyRoll && !melody.isPlaying)
        {
            int melodyIndex = Random.Range(0, melodyArray.Length);
            melody.panStereo = RandomPan();
            melody.clip = melodyArray[melodyIndex];
            melody.Play();
        }

        if (percChance > percRoll && !perc.isPlaying)
        {
            int percIndex = Random.Range(0, percArray.Length);
            perc.panStereo = RandomPan();
            perc.clip = percArray[percIndex];
            perc.Play();
        }

        if (fxChance > fxRoll && !fx.isPlaying)
        {
            int fxIndex = Random.Range(0, fxArray.Length);
            fx.panStereo = RandomPan();
            fx.clip = fxArray[fxIndex];
            fx.Play();
        }
    }

    float RandomPan()
    {
        float panValue = panVariation;

        if (panVariation != 0)
        {
            panValue = Random.Range(-panVariation, panVariation);
        }
       
        return panValue;
    }

    IEnumerator FadeAndStopMusic()
    {
        yield return StartCoroutine(FadeMasterGroup(fadeOutDuration, 0));
        bed.Stop();
        melody.Stop();
        perc.Stop();
        fx.Stop();
        musicPlaying = false;
        StopAllCoroutines();
    }

    IEnumerator FadeMasterGroup(float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat(exposedParamName, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            mixer.SetFloat(exposedParamName, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        mixer.SetFloat(exposedParamName, Mathf.Log10(targetValue) * 20);
        yield break;
    }
}
