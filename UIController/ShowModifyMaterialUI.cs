using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowModifyMaterialUI : MonoBehaviour
{
    Car car;
    Material material;
    string carID;
    string material_id;
    string material_real_id;
    int material_quality;
    Image image;
    private void Start()
    {
        carID = PlayerPrefs.GetString("ModifyCarID");//获取之前点击的赛车ID
        car = CarManager.Instance.GetCarById(int.Parse(carID));//实例化赛车
        material_id = System.Text.RegularExpressions.Regex.Replace(gameObject.name, @"[^0-9]+", "");//根据解析对象名获取当前对象的材料种类
        inInitializedShow();
    }
    
    private void Update()
    {
        material_real_id = ParseMaterialID();//刷新当前材料真实ID
        Sprite sp = Resources.Load("Textrues/Material_" + material_real_id, typeof(Sprite)) as Sprite;//根据真实ID刷新图片
        material = MaterialManager.Instance.GetMaterialById(int.Parse(material_real_id));//根据解析出的真实id实例化material
        ButtonDisabled();//根据flag的情况禁用相应button
        if (material.Capacity == 0 ) //如果该材料数量为零则将图片透明度置为0.5并且禁用改装button
        {
            image.color = new Color(1,1,1,0.5f);
            string buttonName = "Modify_button_" + material_id;
            GameObject.Find(buttonName).GetComponent<Button>().enabled = false;
        }
        image.sprite = sp;
    }
    void inInitializedShow()
    {
        image = gameObject.GetComponent<Image>();
        material_real_id = ParseMaterialID();
        Sprite sp = Resources.Load("Textrues/Material_" + material_real_id, typeof(Sprite)) as Sprite;
        image.sprite = sp;
    }
    public string  ParseMaterialID()
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
        return ( (int.Parse(material_id) - 1)*4  + material_quality ).ToString();
    }
    public void ButtonDisabled()
    {
        if (material_id == "01" && car.Material_01_Finished)
        {
            string buttonName = "Modify_button_" + material_id;
            GameObject.Find(buttonName).GetComponent<Button>().enabled = false;
        }
        if (material_id == "02" && car.Material_02_Finished)
        {
            string buttonName = "Modify_button_" + material_id;
            GameObject.Find(buttonName).GetComponent<Button>().enabled = false;
        }
        if (material_id == "03" && car.Material_03_Finished)
        {
            string buttonName = "Modify_button_" + material_id;
            GameObject.Find(buttonName).GetComponent<Button>().enabled = false;
        }
        if (material_id == "04" && car.Material_04_Finished)
        {
            string buttonName = "Modify_button_" + material_id;
            GameObject.Find(buttonName).GetComponent<Button>().enabled = false;
        }
    }
}
