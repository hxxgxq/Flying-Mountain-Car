using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarAudioControl : MonoBehaviour
{
    PlayerController playerController;
    private void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }
    private void Update()
    {

    }
    public void PlayAudioClip(AudioClip clip)
    {

        if (clip == null)
            return;
        AudioSource source = (AudioSource)gameObject.GetComponent("AudioSource");
        if (source == null)
            source = (AudioSource)gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.minDistance = 1.0f;
        source.maxDistance = 50;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.transform.position = transform.position;
        source.Play();
    }
    public void Play(string str)

    {
        AudioClip clip = (AudioClip)Resources.Load("Audio/" + str, typeof(AudioClip));//调用Resources方法加载AudioClip资源
        PlayAudioClip(clip);
    }
}