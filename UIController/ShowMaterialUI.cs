using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
public class ShowMaterialUI : MonoBehaviour {
    public Text num;
    public Text materialname;
    private void Start()
    {

        showMaterialUI();
    }

    public void showMaterialUI()
    {
        string name = gameObject.name;
        string id = System.Text.RegularExpressions.Regex.Replace(name, @"[^0-9]+", "");
        Material material = MaterialManager.Instance.GetMaterialById((int.Parse(id)));//根据游戏物体的名字获取材料id以实例化
        materialname.text = material.Name;          //显示材料名
        num.text = material.Capacity.ToString();    //显示材料数量
        Image image = gameObject.GetComponent<Image>();
        Sprite sp = Resources.Load("Textrues/Material_" + int.Parse(id).ToString(), typeof(Sprite)) as Sprite;
        image.sprite = sp;

        }
    }
