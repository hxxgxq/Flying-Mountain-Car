using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AchievmentCotroller : MonoBehaviour {

    Car[] car = new Car[12];
    private AudioSource audioSource;
    void Start ()
    {
        audioSource = GameObject.Find("GetMoenyClip").GetComponent<AudioSource>();
        int CarNum = 0; 
        for (int i = 1; i <= 12; i++)
        {
            car[i-1] = CarManager.Instance.GetCarById(i);
            if (car[i-1].IsCollected)
                CarNum += 1;
        }
        PlayerPrefs.SetInt("Achievement_01", PlayerPrefs.GetInt("WinGameTimes", 0));
        PlayerPrefs.SetInt("Achievement_02", PlayerPrefs.GetInt("WinRunTimes", 0));
        PlayerPrefs.SetInt("Achievement_03", PlayerPrefs.GetInt("WinPropTimes", 0));
        PlayerPrefs.SetInt("Achievement_04", PlayerPrefs.GetInt("WinQuickTimes", 0));
        PlayerPrefs.SetInt("Achievement_05", CarNum);
        PlayerPrefs.SetInt("Achievement_06", CarNum);
        PlayerPrefs.SetInt("Achievement_07", CarNum);
        PlayerPrefs.SetFloat("Achievement_08", PlayerPrefs.GetFloat("FlyTime", 0));
        PlayerPrefs.SetFloat("Achievement_09", PlayerPrefs.GetFloat("excursionPath", 0));
        PlayerPrefs.SetInt("Achievement_10", PlayerPrefs.GetInt("UsePropTimes", 0));
        PlayerPrefs.SetFloat("Achievement_11", PlayerPrefs.GetFloat("DashTime", 0));
    }
    public void RecieveReward_01()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 20);
        GameObject.Find("Button_01").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_01_isRewarded", 1);
    }
    public void RecieveReward_02()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 40);
        GameObject.Find("Button_02").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_02_isRewarded", 1);
    }
    public void RecieveReward_03()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 40);
        GameObject.Find("Button_03").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_03_isRewarded", 1);
    }
    public void RecieveReward_04()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 40);
        GameObject.Find("Button_04").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_04_isRewarded", 1);
    }
    public void RecieveReward_05()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 40);
        GameObject.Find("Button_05").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_05_isRewarded", 1);
    }
    public void RecieveReward_06()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 60);
        GameObject.Find("Button_06").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_06_isRewarded", 1);
    }
    public void RecieveReward_07()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 80);
        GameObject.Find("Button_07").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_07_isRewarded", 1);
    }
    public void RecieveReward_08()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 50);
        GameObject.Find("Button_08").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_08_isRewarded", 1);
    }
    public void RecieveReward_09()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 50);
        GameObject.Find("Button_09").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_09_isRewarded", 1);
    }
    public void RecieveReward_10()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 50);
        GameObject.Find("Button_10").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_10_isRewarded", 1);
    }
    public void RecieveReward_11()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 50);
        GameObject.Find("Button_11").GetComponent<Button>().enabled = false;
        PlayerPrefs.SetInt("Achievement_11_isRewarded", 1);
    }

    void Update ()
    {
        if (PlayerPrefs.GetInt("Achievement_01") >= 100 && PlayerPrefs.GetInt("Achievement_01_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_01").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_01", 100);
        }
        if (PlayerPrefs.GetInt("Achievement_02") >= 100 && PlayerPrefs.GetInt("Achievement_02_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_02").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_02", 100);
        }
        if (PlayerPrefs.GetInt("Achievement_03") >= 100 && PlayerPrefs.GetInt("Achievement_03_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_03").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_03", 100);
        }
        if (PlayerPrefs.GetInt("Achievement_04") >= 100 && PlayerPrefs.GetInt("Achievement_04_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_04").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_04", 100);
        }
        if (PlayerPrefs.GetInt("Achievement_05") >= 4 && PlayerPrefs.GetInt("Achievement_05_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_05").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_05", 5);
        }
        if (PlayerPrefs.GetInt("Achievement_06") >= 10 && PlayerPrefs.GetInt("Achievement_06_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_06").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_06", 10);
        }
        if (PlayerPrefs.GetInt("Achievement_07") >= 12 && PlayerPrefs.GetInt("Achievement_07_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_07").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_07", 12);
        }
        if (PlayerPrefs.GetFloat("Achievement_08") >= 1000 && PlayerPrefs.GetInt("Achievement_08_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_08").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_08", 1000);
        }
        if (PlayerPrefs.GetFloat("Achievement_09") >= 10000 && PlayerPrefs.GetInt("Achievement_09_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_09").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_09", 10000);
        }
        if (PlayerPrefs.GetInt("Achievement_10") >= 300 && PlayerPrefs.GetInt("Achievement_10_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_10").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_10", 300);
        }
        if (PlayerPrefs.GetFloat("Achievement_11") >= 1000 && PlayerPrefs.GetInt("Achievement_11_isRewarded", 0) == 0)
        {
            GameObject.Find("Button_11").GetComponent<Button>().enabled = true;
            //PlayerPrefs.SetInt("Achievement_11", 1000);
        }



    }
}
