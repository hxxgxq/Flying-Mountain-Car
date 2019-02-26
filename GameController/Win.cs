using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;
using Scripts.Ai;

public class Win : MonoBehaviour {

    private bool IsComing = false;
    private bool IsPause = false;

    public GameObject[] dlg;
    Rigidbody rigid;
    GameObject[] AICars;
    float speed = 0;
    public int rank;
    Text BestTime;
    public Text Reward_num;
    GameObject Car_instance;
    excursion Excursion;
    ShowMessage showMessage;
    PlayerController playerController;
    GetProp getProp;
    AudioListener audioListener;
    bool isAdd;
    bool isUpdateTime;
    int i = 0;//指示当前是第几关
    void Start ()
    {
        isAdd = false;
        isUpdateTime = false;
        rank = 1;
        rigid = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        AICars = GameObject.FindGameObjectsWithTag("AI");
        speed = rigid.velocity.magnitude;
        
        IsComing = false;
        float besttime = 0;
        i = 0;
        BestTime = GameObject.Find("bestTime").GetComponent<Text>();
        
        if (PlayerPrefs.GetInt("CurrentScene", 0) == 1)//竞速第一关
        {
            i = 1;
        }
        else if (PlayerPrefs.GetInt("CurrentScene", 0) == 2)//竞速第二关
        {
            i = 2;
        }
        else if (PlayerPrefs.GetInt("CurrentScene", 0) == 3)//道具第一关
        {
            i = 3;
        }
        else if (PlayerPrefs.GetInt("CurrentScene", 0) == 4)//道具第二关
        {
            i = 4;
        }
        besttime = PlayerPrefs.GetFloat("FinalTime_0" + i.ToString(), 0);
        BestTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)besttime / 60,
            (int)besttime % 60,
            (int)(besttime * 100) % 60
            );
         Car_instance = GameObject.FindGameObjectWithTag("Player");
         Excursion = Car_instance.GetComponent<excursion>();
         showMessage = Car_instance.GetComponent<ShowMessage>();
         playerController = Car_instance.GetComponent<PlayerController>();
         getProp = Car_instance.GetComponent<GetProp>();
         audioListener = GameObject.Find("Cameras").GetComponent<AudioListener>();
    }
    private void OnDestroy()
    {
        if (PlayerPrefs.GetInt("IsQuickGame", 0) == 1)
            PlayerPrefs.SetInt("IsQuickGame", 0);
    }
    void OnTriggerEnter(Collider car)
    {
        if(car.gameObject.tag == "AI")
        {
            rank++;
            car.GetComponent<CarAIControl2>().m_Driving = false;
        }
        if (car.gameObject.name == rigid.gameObject.name)
        {
            IsComing = true;
        }
        ShowMessage showMessage = GameObject.Find("ShowMessage").GetComponent<ShowMessage>();
        showMessage.StopTiming();
        float FinalTime = showMessage.ReturnFinalTime();
        if (!isUpdateTime)
        { 
            if(PlayerPrefs.GetFloat("FinalTime_0"+i.ToString(),0) == 0)//第一次完成赛道
            {
                PlayerPrefs.SetFloat("FinalTime_0" + i.ToString(), FinalTime);
                BestTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                (int)FinalTime / 60,
                (int)FinalTime % 60,
                (int)(FinalTime * 100) % 60
                 );
            }
            else if (FinalTime < PlayerPrefs.GetFloat("FinalTime_0" + i.ToString(), 0))//新成绩比原来好的时候更新
            {
                PlayerPrefs.SetFloat("FinalTime_0" + i.ToString(), FinalTime);
                BestTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                (int)FinalTime / 60,
                (int)FinalTime % 60,
                (int)(FinalTime * 100) % 60
                );
            }
            isUpdateTime = true;
        }
        if (!isAdd && car.gameObject.tag == "Player")
        {
            PlayerPrefs.SetFloat("DashTime", PlayerPrefs.GetFloat("DashTime", 0) + showMessage.ReturnDashTime());
            PlayerPrefs.SetFloat("excursionPath", PlayerPrefs.GetFloat("excursionPath", 0) + Excursion.ReturnPath());
            PlayerPrefs.SetInt("UsePropTimes", PlayerPrefs.GetInt("UsePropTimes", 0) + getProp.ReturnUsePropTimes());
            PlayerPrefs.SetFloat("FlyTime", PlayerPrefs.GetFloat("FlyTime", 0) + playerController.ReturnFlyTime());
            if (rank == 1)
            {
                int i = Random.Range(15, 20);
                Reward_num.text = i.ToString();
                PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond", 0) + i);
            }
            else if (rank == 2)
            {
                int i = Random.Range(10, 15);
                Reward_num.text = i.ToString();
                PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond", 0) + i);
            }
            else
            {
                int i = Random.Range(5, 10);
                Reward_num.text = i.ToString();
                PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond", 0) + i);
            }

            if(PlayerPrefs.GetInt("IsQuickGame", 0) == 1 && rank ==1)
            {
                PlayerPrefs.SetInt("WinQuickTimes", PlayerPrefs.GetInt("WinQuickTimes", 0) + 1);
                PlayerPrefs.SetInt("WinGameTimes", PlayerPrefs.GetInt("WinGameTimes", 0) + 1);
            }
            else if ((PlayerPrefs.GetInt("CurrentScene", 0) == 1 && rank == 1) || (PlayerPrefs.GetInt("CurrentScene", 0) == 2 && rank ==1) )
            {
                Debug.Log(rank);
                PlayerPrefs.SetInt("WinRunTimes", PlayerPrefs.GetInt("WinRunTimes", 0) + 1);
                PlayerPrefs.SetInt("WinGameTimes", PlayerPrefs.GetInt("WinGameTimes", 0) + 1);
            }
            else if ( (PlayerPrefs.GetInt("CurrentScene", 0) == 3 && rank == 1) || (PlayerPrefs.GetInt("CurrentScene", 0) == 4 && rank == 1) )
            {
                PlayerPrefs.SetInt("WinPropTimes", PlayerPrefs.GetInt("WinPropTimes", 0) + 1);
                PlayerPrefs.SetInt("WinGameTimes", PlayerPrefs.GetInt("WinGameTimes", 0) + 1);
            }
            isAdd = true;
            audioListener.enabled = false;
        }
        
    }

    void Update()
    {
        OnPause();
        OnWinning();
        OnFailed();
        Debug.Log(rank);
    }

    void OnPause()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !IsComing && !IsPause)
        {
            Time.timeScale = 0;
            dlg[0].SetActive(true);
            IsPause = true;
            GameObject.Find("Cameras").GetComponent<AudioListener>().enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && IsPause)
        {
            Time.timeScale = 1;
            dlg[0].SetActive(false);
            IsPause = false;
            GameObject.Find("Cameras").GetComponent<AudioListener>().enabled = true;
        }
    }
    void OnWinning()
    {
        if (IsComing == true)
        {
            dlg[1].SetActive(true);
            rigid.gameObject.GetComponent<CarUserControl>().brake = true;
            GameObject.Find("ShowMessage").GetComponent<ShowMessage>().isPlaying = false;
        }
    }
    void OnFailed()
    {

    }

}
