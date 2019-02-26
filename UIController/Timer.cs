using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Timer : MonoBehaviour
{
    public float ShopUpdateTimeInterval = 10;
    int CarID;
    int MaterialID;
    GameObject[] Materials = new GameObject[6];
    private void Start()
    {
        //PlayerPrefs.SetFloat("ShopUpdateTimeInterval", 10);

        ShopUpdateTimeInterval = PlayerPrefs.GetFloat("ShopUpdateTimeInterval");
    }
    private void OnApplicationQuit()
    {
        Debug.Log(ShopUpdateTimeInterval);
        PlayerPrefs.SetFloat("ShopUpdateTimeInterval", ShopUpdateTimeInterval);
        Debug.Log(PlayerPrefs.GetFloat("ShopUpdateTimeInterval"));
    }
    private void Update()
    {
        ShopUpdateTimeInterval -= Time.deltaTime;
        if (ShopUpdateTimeInterval <= 0)
        {
            PlayerPrefs.SetInt("Material_1ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("Material_2ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("Material_3ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("Material_4ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("Material_5ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("Material_6ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("piece_1ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("piece_2ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("piece_3ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("piece_4ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("piece_5ShopUpdateTimeUp", 1);
            PlayerPrefs.SetInt("piece_6ShopUpdateTimeUp", 1);

            ShopUpdateTimeInterval = 10;

        }
        //Debug.Log(ShopUpdateTimeInterval);
    }
}
