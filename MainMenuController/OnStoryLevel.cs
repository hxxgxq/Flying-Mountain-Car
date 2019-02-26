using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnStoryLevel : MonoBehaviour {
    private AudioSource audioSource;
    private AudioSource backaudioSource;
    private void Start()
    {
        audioSource = GameObject.Find("ButtonDownClip_1").GetComponent<AudioSource>();
        backaudioSource = GameObject.Find("BackButtonClip").GetComponent<AudioSource>();
    }
    public void BackClick()
    {
        SceneMgr.Instance.SwitchScence("ModelDlg");
        backaudioSource.Play();
    }

    public void LevelClick()
    {
        SceneMgr.Instance.SwitchScence("Loading");
        audioSource.Play();
    }
}
