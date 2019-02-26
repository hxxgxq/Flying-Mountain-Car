using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OnClickMenu : MonoBehaviour {

    public GameObject[] menu;

    void Start()
    {
        Time.timeScale = 1;
        if (SceneMgr.IsJump == false)
            SceneMgr.Instance.SwitchScence("HomeDlg");
        else
            SceneMgr.Instance.SwitchScence("ModelDlg");
        if (PlayerPrefs.GetInt("IsFirstEnterGame", 0) == 0)
        {

            PlayerPrefs.SetInt("Money", 1000);
            PlayerPrefs.SetInt("Diamond", 100);
            PlayerPrefs.SetInt("IsFirstEnterGame", 1);
        }
        


    }
    public void HomeClick()
    {
        SceneMgr.Instance.SwitchScence("HomeDlg");
    }
    public void TaskClick()
    {
        SceneMgr.Instance.SwitchScence("TaskDlg");
    }
    public void AchClick()
    {
        SceneMgr.Instance.SwitchScence("AchievementDlg");
    }
    public void ShopClick()
    {
        SceneMgr.Instance.SwitchScence("MaterialShop");
    }
    public void SetClick()
    {
        SceneMgr.Instance.SwitchScence("SetDlg");
    }

}
