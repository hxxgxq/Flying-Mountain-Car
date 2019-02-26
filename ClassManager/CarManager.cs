using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using LitJson;
public class CarManager : MonoBehaviour {
    //单例模式 
    private static CarManager _instance;
    public static CarManager Instance             //公有的静态方法只会创建一次调用时InventoryManager.Instance() 返回该实例
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.Find("CarManager").GetComponent<CarManager>();
            }
            return _instance;
        }
    }

    public List<Car> carList;  //  car列表

    public void Awake() //场景初加载时解析json文件并实例化car类的列表
    {
        ParseCarJson();
    }
    private void OnDestroy()
    {
        string path = Application.persistentDataPath + "/Car.jason.txt";
        string json = JsonMapper.ToJson(carList);
        File.WriteAllText(path, json, Encoding.UTF8);
    }
    private void OnApplicationQuit()                //游戏退出时调用用于将carlist对象数据转成Json并写入Car.jason中
    {
        string path = Application.persistentDataPath + "/Car.jason.txt";
        string json = JsonMapper.ToJson(carList);
        File.WriteAllText(path, json, Encoding.UTF8);
    }
    void ParseCarJson()                         //Car类对应Json文件的解析
    {
        string carjson;
        if(!File.Exists(Application.persistentDataPath + "/Car.jason.txt"))
        {
            TextAsset carText = Resources.Load("Car.jason") as TextAsset;  //Resources.Loud（）动态加载的方法 TextAsset是Unity中的文本类型
            carjson = carText.text;
        }
        else
        {
            carjson = File.ReadAllText(Application.persistentDataPath + "/Car.jason.txt");
        }
            carList = JsonMapper.ToObject<List<Car>>(carjson);
    }

    public Car GetCarById(float id)                //根据id获取Car类对象
    {
        foreach (Car car in carList)
        {
            if (car.ID == id)
            {
                return car;
            }
        }
        return null;
    }

}
        