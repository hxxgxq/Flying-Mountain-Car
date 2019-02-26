using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class ShowMessage : MonoBehaviour {

    #region
    public GameObject countdown;
    GameObject car;

    GameObject[] wakeFlame;
    int skidMarkNum = 0;

    Image energyBar;
    Image speedBar;
    Text time;
    Text speedNum;
    float FinalTime = 0;

    float RealSpeed;
    float MaxSpeed;
    bool IsSpeedUp = false;
    bool isTiming = true;
    float DashTime = 0;
    public bool isPlaying = false;
    private bool isPlayedDash = false;
    private bool isPlayedMove = false;
    CarAudioControl carAudioControl;

    #endregion

    void Start()
    {
        car = GameObject.FindWithTag("Player");
        carAudioControl = car.GetComponent<CarAudioControl>();

        wakeFlame = GameObject.FindGameObjectsWithTag("WakeFlame");
        foreach (GameObject flame in wakeFlame)
        {
            flame.SetActive(false);
        }
        energyBar = GameObject.Find("Energy").GetComponent<Image>();
        energyBar.fillAmount = 0.0f;
        speedBar = GameObject.Find("Speed").GetComponent<Image>();
        speedBar.fillAmount = 0.0f;
        time = GameObject.Find("Time").GetComponent<Text>();
        time.text = "00:00:00";
        speedNum = GameObject.Find("SpeedNum").GetComponent<Text>();
        speedNum.text = "0";
    }


    void Update()
    {
        if(isPlaying)
            OnMessage();
    }

    void OnMessage()
    {
       
        if (energyBar.fillAmount <= 1)
        {
            if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)))
            {
                if (PlayerPrefs.GetInt("CurrentScene", 0) == 1 || PlayerPrefs.GetInt("CurrentScene", 0) == 2)
                {
                    energyBar.fillAmount += Time.deltaTime * 0.3f;
                }
                if (skidMarkNum == 0)
                {
                    GameObject skidMark_L = Resources.Load("PropPrefabs/SkidMark", typeof(GameObject)) as GameObject;
                    Instantiate(skidMark_L, car.transform);
                    skidMark_L.transform.position = new Vector3(-0.861f, -0.664f, -1.293f);
                    GameObject skidMark_R = Resources.Load("PropPrefabs/SkidMark", typeof(GameObject)) as GameObject;
                    Instantiate(skidMark_R, car.transform);
                    skidMark_R.transform.position = new Vector3(0.861f, -0.664f, -1.293f);
                    skidMarkNum = 2;
                }

            }
            if(Input.GetKeyUp(KeyCode.LeftShift) && skidMarkNum == 2)
            {
                GameObject[] skidMark = GameObject.FindGameObjectsWithTag("SkidMark");
                foreach (GameObject mark in skidMark)
                {
                    mark.transform.parent = null;
                    skidMarkNum = 0;
                }
            }
        }
        if (IsSpeedUp)
        {
            energyBar.fillAmount -= Time.deltaTime * 0.2f;
            car.GetComponent<CarUserControl>().speedup();
            DashTime += Time.deltaTime;
            foreach(GameObject falme in wakeFlame)
            {
                falme.SetActive(true);
            }
        }
        else
        {
            car.GetComponent<CarUserControl>().speeddown();
            foreach (GameObject falme in wakeFlame)
            {
                falme.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && energyBar.fillAmount > 0)
        {
            IsSpeedUp = true;
            if (!isPlayedDash)
            {
                isPlayedDash = true;
                isPlayedMove = false;
                carAudioControl.Play("dash");
            }
        }
        if (energyBar.fillAmount <= 0)
        {
            IsSpeedUp = false;
            if (!isPlayedMove)
            {
                carAudioControl.Play("move1");
                isPlayedDash = false;
                isPlayedMove = true;
            }
        }


        RealSpeed = car.GetComponent<PlayerController>().CurrentSpeed;
        MaxSpeed = car.GetComponent<PlayerController>().MaxSpeed;
        speedBar.fillAmount = RealSpeed / (MaxSpeed * 2);

        if (!countdown.activeInHierarchy)
        {
            FinalTime += Time.deltaTime;
            speedNum.text = ((int)(RealSpeed * 6)).ToString();
        }
        time.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)FinalTime / 60,
            (int)FinalTime % 60,
            (int)(FinalTime * 100) % 60
            );
    }
    public void StopTiming()
    {
        isTiming = false;
    }
    public float ReturnFinalTime()
    {
        return FinalTime;
    }
    public float ReturnDashTime()
    {
        return DashTime;
    }
}
