using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnclickBuyMaterial : MonoBehaviour {
    GameObject parent;
    int id;
    Material material;
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
        id = parent.GetComponent<GenerateMaterials>().RetrunID();//返回父物体随机生成的id
        material = MaterialManager.Instance.GetMaterialById(id);//实例化出对应id的material
        //实现点击之后的具体功能
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - material.BuyPrice);
        material.Capacity += 1;
        PlayerPrefs.SetInt(parent.name + "IsBuyed", 1);//标志该物品已被购买
        GetComponent<Button>().enabled = false;//禁用购买按钮
        audioSource.Play();
    }
    public bool ReturnFlag()
    {
        return IsBuyed;
    }
}
