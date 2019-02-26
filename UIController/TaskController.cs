using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TaskController : MonoBehaviour {
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GameObject.Find("GetMoenyClip").GetComponent<AudioSource>();

        PlayerPrefs.SetInt("Task_01", PlayerPrefs.GetInt("WinGameTimes", 0));
        PlayerPrefs.SetInt("Task_04", PlayerPrefs.GetInt("WinRunTimes", 0));
        PlayerPrefs.SetInt("Task_05", PlayerPrefs.GetInt("WinPropTimes", 0));
        PlayerPrefs.SetInt("Task_06", PlayerPrefs.GetInt("WinQuickTimes", 0));
        PlayerPrefs.SetFloat("Task_02", PlayerPrefs.GetFloat("excursionPath", 0));
        PlayerPrefs.SetFloat("Task_03", PlayerPrefs.GetFloat("DashTime", 0));
        PlayerPrefs.SetFloat("Task_07", PlayerPrefs.GetFloat("FlyTime", 0));

    }
     
    public void RecieveReward_01()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money")+500);
        GameObject.Find("Button_01").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Task_01_isRewarded", 1);
        audioSource.Play();
    }
    public void RecieveReward_02()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 1000);
        GameObject.Find("Button_02").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Task_02_isRewarded", 1);
        audioSource.Play();
    }
    public void RecieveReward_03()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 1000);
        GameObject.Find("Button_03").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Task_03_isRewarded", 1);
        audioSource.Play();
    }
    public void RecieveReward_04()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 500);
        GameObject.Find("Button_04").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Task_04_isRewarded", 1);
        audioSource.Play();
    }
    public void RecieveReward_05()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 500);
        GameObject.Find("Button_05").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Task_05_isRewarded", 1);
        audioSource.Play();
    }
    public void RecieveReward_06()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 500);
        GameObject.Find("Button_06").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Task_06_isRewarded", 1);
        audioSource.Play();
    }
    public void RecieveReward_07()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 1000);
        GameObject.Find("Button_07").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Task_07_isRewarded", 1);
        audioSource.Play();
    }


    void Update ()
    {
        if (PlayerPrefs.GetInt("Task_01") >= 3 &&  PlayerPrefs.GetInt("Task_01_isRewarded",0)==0 )
        {
            GameObject.Find("Button_01").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Task_01",3);
        }
        if (PlayerPrefs.GetFloat("Task_02") >= 400 && PlayerPrefs.GetInt("Task_02_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_02").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Task_02", 400);
        }
        if (PlayerPrefs.GetFloat("Task_03") >= 100 && PlayerPrefs.GetInt("Task_03_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_03").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Task_03", 20);
        }
        if (PlayerPrefs.GetInt("Task_04") >= 2 && PlayerPrefs.GetInt("Task_04_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_04").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Task_04", 2);
        }
        if (PlayerPrefs.GetInt("Task_05") >= 2 && PlayerPrefs.GetInt("Task_05_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_05").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Task_05", 2);
        }
        if (PlayerPrefs.GetInt("Task_06") >= 2 && PlayerPrefs.GetInt("Task_06_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_06").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Task_06", 2);
        }
        if (PlayerPrefs.GetFloat("Task_07") >= 40 && PlayerPrefs.GetInt("Task_07_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_07").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Task_07", 40);
        }








    }
}
