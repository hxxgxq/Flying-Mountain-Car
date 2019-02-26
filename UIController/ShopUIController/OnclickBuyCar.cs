using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnclickBuyCar : MonoBehaviour {
    GameObject parent;
    int id;
    Car car;
    bool IsBuyed;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GameObject.Find("Buy").GetComponent<AudioSource>();
    }

    public void OnClickBuy()
    {
        parent = transform.parent.gameObject;   //获取该button的父物体
        Debug.Log(parent.name);
        id = parent.GetComponent<GenerateCars>().RetrunID();//返回父物体随机生成的id
        car = CarManager.Instance.GetCarById(id);//实例化出对应id的car
        //实现点击之后的具体功能
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") - car.BuyPrice);
        car.CarPiece += 1;
        PlayerPrefs.SetInt(parent.name + "IsBuyed", 1);//标志该物品已被购买
        GetComponent<Button>().enabled = false;//禁用购买按钮
        audioSource.Play();
    }
    public bool ReturnFlag()
    {
        return IsBuyed;
    }
}
