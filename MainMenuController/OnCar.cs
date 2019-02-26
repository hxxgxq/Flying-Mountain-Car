using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCar : MonoBehaviour {
    private AudioSource audioSource;
    private AudioSource backaudioSource;
    private void Start()
    {
        audioSource = GameObject.Find("ButtonDownClip_1").GetComponent<AudioSource>();
        backaudioSource = GameObject.Find("BackButtonClip").GetComponent<AudioSource>();
    }

    public void BackClick()
    {
        SceneMgr.Instance.SwitchScence("GarageDlg");
        backaudioSource.Play();
    }

    public void BackToHome()
    {
        SceneMgr.Instance.SwitchScence("HomeDlg");
        backaudioSource.Play();
    }

    public void GarageClick()
    {
        SceneMgr.Instance.SwitchScence("GarageDlg");
        audioSource.Play();
    }

    public void MaterialClick()
    {
        SceneMgr.Instance.SwitchScence("MaterialDlg");
        audioSource.Play();
    }

    public void ModifyClick()
    {
        SceneMgr.Instance.SwitchScence("ModifyDlg");
        audioSource.Play();
    }

}
