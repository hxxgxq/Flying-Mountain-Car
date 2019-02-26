using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Winning : MonoBehaviour {

    public GameObject ShowRank;
    Text rank;

    void Start()
    {
        rank = ShowRank.GetComponent<Text>();
        rank.text = "第 " + GameObject.Find("Target").GetComponent<Win>().rank.ToString() + " 名";        
    }

    public void Restart()
    {
        SceneMgr.Instance.SwitchScence("Loading");
    }

    public void Back()
    {
        SceneMgr.IsJump = true;
        SceneMgr.Instance.SwitchScence("Return");
    }

}
