using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Ai;
public class SelectCarUI : MonoBehaviour {

    int CarID;
    Car car;
    Image image;
    Text name;
    Text speed;
    Text acceleration;
    Text dashspeed;
    Text flag;
    Button levelButton_01;
    Button levelButton_02;
    void Start ()
    {
        CarID = 1;
        PlayerPrefs.SetInt("CurrentCarID", CarID);
        car = CarManager.Instance.GetCarById(CarID);
        image = GameObject.Find("CarShow").GetComponent<Image>();
        name = GameObject.Find("SelectCarName").GetComponent<Text>();
        speed = GameObject.Find("Speed").GetComponent<Text>();
        acceleration = GameObject.Find("Acceleration").GetComponent<Text>();
        dashspeed = GameObject.Find("DashSpeed").GetComponent<Text>();
        flag = GameObject.Find("Flag").GetComponent<Text>();
        if(gameObject.name == "Race_Cars")
        {
            levelButton_01 = GameObject.Find("Race_Level_1").GetComponent<Button>();
            levelButton_02 = GameObject.Find("Race_Level_2").GetComponent<Button>();
        }
        else if(gameObject.name == "Prop_Cars")
        {
            levelButton_01 = GameObject.Find("Prop_Level_1").GetComponent<Button>();
            levelButton_02 = GameObject.Find("Prop_Level_2").GetComponent<Button>();
        }
        
    }

	void Update ()
    {
        car = CarManager.Instance.GetCarById(CarID);
        Sprite sp = Resources.Load("Textrues/Cars/car_" + CarID, typeof(Sprite)) as Sprite;
        image.sprite = sp;
        name.text = car.Name;
        speed.text = (car.Speed*6).ToString();
        acceleration.text = (car.Acceleration*200).ToString();
        dashspeed.text = (car.DashSpeed*6).ToString();
        if (car.IsCollected)
        {
            flag.text = null;
            levelButton_01.enabled = true;
            levelButton_02.enabled = true;
            //在这里写将选择地图的按钮打开
        }
        else
        {
            flag.text = "未拥有";
            levelButton_01.enabled = false;
            levelButton_02.enabled = false;
            //在这里写将选择地图的按钮关闭
        }

    }
    public void OnclickNext()
    {
        if (CarID < 12)
            CarID += 1;
        else if(CarID == 12)
            CarID = 1;
        PlayerPrefs.SetInt("CurrentCarID", CarID);
    }
    public void OnclickPrevious()
    {
        if (CarID > 1)
            CarID -= 1;
        else if (CarID == 1)
            CarID = 12;
        PlayerPrefs.SetInt("CurrentCarID", CarID);
    }
}
