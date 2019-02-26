using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnShop : MonoBehaviour {
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GameObject.Find("ButtonDownClip_1").GetComponent<AudioSource>();
    }
    public void BoxClick()
    {
        SceneMgr.Instance.SwitchScence("BoxShop");
    }

    public void PieceClick()
    {
        SceneMgr.Instance.SwitchScence("PieceShop");
        audioSource.Play();
    }

    public void MatClick()
    {
        SceneMgr.Instance.SwitchScence("MaterialShop");
        audioSource.Play();
    }

}
