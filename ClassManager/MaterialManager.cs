using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using LitJson;
public class MaterialManager : MonoBehaviour {
    //单例模式
    private static MaterialManager _instance;

    public static MaterialManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("MaterialManager").GetComponent<MaterialManager>();
            }
            return _instance;
        }
    }

    private List<Material> materialList;

    public void Awake()
    {
        ParseMaterialJson();
    }
    private void OnDestroy()
    {
        string path = Application.persistentDataPath + "/Material.jason.txt";
        string json = JsonMapper.ToJson(materialList);
        File.WriteAllText(path, json, Encoding.UTF8);
    }
    private void OnApplicationQuit()
    {
        string path = Application.persistentDataPath + "/Material.jason.txt";
        string json = JsonMapper.ToJson(materialList);
        File.WriteAllText(path, json, Encoding.UTF8);
    }
    void ParseMaterialJson()
    {
        string materialjson;
        if (!File.Exists(Application.persistentDataPath + "/Material.jason.txt"))
        {
            TextAsset materialText = Resources.Load("Material.jason") as TextAsset;  //Resources.Loud（）动态加载的方法 TextAsset是Unity中的文本类型
            materialjson = materialText.text;
        }
        else
        {
            materialjson = File.ReadAllText(Application.persistentDataPath + "/Material.jason.txt");
        }     
        materialList = JsonMapper.ToObject<List<Material>>(materialjson);
    }
    public Material GetMaterialById(float id)                //根据id获取Material类对象
    {
        foreach (Material material in materialList)
        {
            if (material.ID == id)
            {
                return material;
            }
        }
        return null;
    }

}
