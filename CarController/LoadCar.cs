using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class LoadCar : MonoBehaviour {
    GameObject player;
    Car car;
    int id;
    // Use this for initialization
    void Awake ()
    {
        id = PlayerPrefs.GetInt("CurrentCarID");
        //Debug.Log(id);
        player = (GameObject)Instantiate(Resources.Load("ModelPrefabs/Car_"+id.ToString()));
        
    }
    private void Start()
    {
        car = CarManager.Instance.GetCarById(id);
        //Debug.Log(car.Speed);
       // Debug.Log(car.DashSpeed);
        player.GetComponent<PlayerController>().m_Topspeed = float.Parse(car.Speed.ToString());
        player.GetComponent<CarUserControl>().m_AccelSensitivity = float.Parse(car.Acceleration.ToString());
        player.GetComponent<PlayerController>().N2Speed =  float.Parse(car.DashSpeed.ToString());
        if (PlayerPrefs.GetInt("CurrentScene",0) == 1)//如果是竞速第一关
        {
            player.transform.position = new Vector3(190f, 3.24f, -243f);

        }
        else if(PlayerPrefs.GetInt("CurrentScene",0) == 2)//竞速第二关
        {
            player.transform.position = new Vector3(50f, -0.5f, 60f);
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (PlayerPrefs.GetInt("CurrentScene",0) == 3)//道具第一关
        {
            player.transform.position = new Vector3(190f, 3.24f, -243f);

        }
        else if (PlayerPrefs.GetInt("CurrentScene",0) == 4)//道具第二关
        {
            player.transform.position = new Vector3(50f, -0.5f, 60f);
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }





    }
    // Update is called once per frame
    void Update ()
    {
		//Debug.Log(player.GetComponent<PlayerController>().CurrentSpeed);
	}
}
