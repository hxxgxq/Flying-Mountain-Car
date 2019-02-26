using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnclickModify : MonoBehaviour {
    string material_id;
    int material_quality;
    string material_real_id;
    Car car;
    Material material;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GameObject.Find("ModifyClip").GetComponent<AudioSource>();
    }
    public void OnClickModify()
    {
       
        string carID = PlayerPrefs.GetString("ModifyCarID");//获取之前点击的赛车ID
        car = CarManager.Instance.GetCarById(int.Parse(carID));
        material_id = PlayerPrefs.GetString("ModifyMaterialID");//获取当前材料种类
        material_real_id = ParseMaterialID();//获取真实ID以实例化material
        UpdateFlag();//更新每个赛车每种零件的改装完成情况 当是第一种材料并且car.Material_01_Quality == 4并且按下改装键时会调用该函数
        material = MaterialManager.Instance.GetMaterialById(int.Parse(material_real_id));//实例化点击之前的当前材料
        material.Capacity -= 1;
        UpdateQuality();
        UpdateCarInfor();
        audioSource.Play();
    }
    public string ParseMaterialID()//获取当前材料对应的实例ID
    {
        //根据当前材料种类获取其相应的品质
        if (material_id == "01")
        {
            material_quality = car.Material_01_Quality;
        }
        else if (material_id == "02")
        {
            material_quality = car.Material_02_Quality;
        }
        else if (material_id == "03")
        {
            material_quality = car.Material_03_Quality;
        }
        else if (material_id == "04")
        {
            material_quality = car.Material_04_Quality;
        }
        return ((int.Parse(material_id) - 1) * 4 + material_quality).ToString();
    }
    public void UpdateQuality()//更新赛车当前材料的品质
    {
        if (material_id == "01" && car.Material_01_Quality != 4 )
        {
            car.Material_01_Quality += 1;
        }
        else if (material_id == "02" && car.Material_02_Quality != 4)
        {
            car.Material_02_Quality += 1;
        }
        else if (material_id == "03" && car.Material_03_Quality != 4)
        {
            car.Material_03_Quality += 1;
        }
        else if (material_id == "04" && car.Material_04_Quality != 4)
        {
            car.Material_04_Quality += 1;
        }
    }
    public void UpdateCarInfor()//更新改装后赛车的信息
    {
        car.Speed += material.SpeedIncrease;
        car.Acceleration += material.AccelerationIncrease;
        car.DashSpeed += material.DashSpeedIncrease;
    }
    public void UpdateFlag()
    {
        if (material_id == "01" && car.Material_01_Quality == 4)
        {
            car.Material_01_Finished = true;
        }
        if (material_id == "02" && car.Material_02_Quality == 4)
        {
            car.Material_02_Finished = true;
        }
        if (material_id == "03" && car.Material_03_Quality == 4)
        {
            car.Material_03_Finished = true;
        }
        if (material_id == "04" && car.Material_04_Quality == 4)
        {
            car.Material_04_Finished = true;
        }
    }
}
