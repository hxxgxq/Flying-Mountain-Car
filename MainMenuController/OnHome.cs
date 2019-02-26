using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHome : MonoBehaviour {
    private AudioSource audioSource;
    private AudioSource startaudioSource;
    private void Start()
    {
        audioSource = GameObject.Find("ButtonDownClip_1").GetComponent<AudioSource>();
        startaudioSource = GameObject.Find("StartClip").GetComponent<AudioSource>();
    }
    public void GameStartClick()
    {
        SceneMgr.Instance.SwitchScence("ModelDlg");
        startaudioSource.Play();
    }

    public void CarsClick()
    {
        SceneMgr.Instance.SwitchScence("GarageDlg");
        audioSource.Play();
    }

}
