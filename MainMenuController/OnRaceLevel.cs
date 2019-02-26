using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRaceLevel : MonoBehaviour {
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

    public void Level1Click()
    {
        PlayerPrefs.SetInt("CurrentScene", 1);
        SceneMgr.Instance.SwitchScence("Loading");
        audioSource.Play();
    }
    public void Level2Click()
    {
        PlayerPrefs.SetInt("CurrentScene", 2);
        SceneMgr.Instance.SwitchScence("Loading");
        audioSource.Play();
    }
}
