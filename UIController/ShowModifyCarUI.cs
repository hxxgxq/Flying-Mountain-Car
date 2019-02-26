using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowModifyCarUI : MonoBehaviour
{
    Text ModifyCarname;
    Car car;
    Text Modifyspeed;
    Text Modifyacceleration;
    Text ModifyDashspeed;
    private void Start()
    {
        string id = PlayerPrefs.GetString("ModifyCarID");
        car = CarManager.Instance.GetCarById(int.Parse(id));
        Image image = gameObject.GetComponent<Image>();
        Sprite sp = Resources.Load("Textrues/Cars/car_" + id, typeof(Sprite)) as Sprite;
        image.sprite = sp;
        ModifyCarname = GameObject.Find("ModifyCarname").GetComponent<Text>();
        Modifyspeed = GameObject.Find("Modifyspeed").GetComponent<Text>();
        Modifyacceleration = GameObject.Find("Modifyacceleration").GetComponent<Text>();
        ModifyDashspeed = GameObject.Find("ModifyDashspeed").GetComponent<Text>();
        ModifyCarname.text = car.Name;
    }
    private void Update()
    {
        Modifyspeed.text = (car.Speed * 6).ToString();
        ModifyDashspeed.text = (car.DashSpeed * 6).ToString();
        Modifyacceleration.text = (car.Acceleration * 200).ToString();
    }


}
