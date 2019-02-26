using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPropLevel : MonoBehaviour {
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
        audioSource.Play();
        PlayerPrefs.SetInt("CurrentScene",3);
        SceneMgr.Instance.SwitchScence("Loading");
    }
    public void Level2Click()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("CurrentScene", 4);
        SceneMgr.Instance.SwitchScence("Loading");
    }
}
