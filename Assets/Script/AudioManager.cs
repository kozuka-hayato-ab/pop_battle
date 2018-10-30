using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>{
    private AudioSource AudioSourceBGM;
    private AudioSource AudioSourceSE;
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioClip[] seClips;

    private new void Awake()
    {
        base.Awake();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        AudioSourceBGM = audioSources[0];
        AudioSourceSE = audioSources[1];
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeBGM(int playBGMindex)
    {
        AudioSourceBGM.clip = bgmClips[playBGMindex];
        AudioSourceBGM.Play();
    }

    public void StopBGM()
    {
        AudioSourceBGM.Stop();
    }

    public void PlaySEClip(AudioClip clip,float soundScale)
    {
        AudioSourceSE.PlayOneShot(clip, soundScale);
    }

    public void StopAllsound()
    {
        AudioSourceBGM.Stop();
        AudioSourceSE.Stop();
    }
}
