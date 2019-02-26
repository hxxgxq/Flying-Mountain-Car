using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GenerateCars : MonoBehaviour {

	Image image;
    Car car;
    int id;
    Text cost;
    GameObject button;
    Sprite sp;
    float ShopUpdateTime = 10;//商店刷新间隔
    void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name + "CarShopFirstOpen", 0) == 0)
        {
            PlayerPrefs.SetInt("Shop_" + gameObject.name + "_id", Random.Range(1, 12));
            PlayerPrefs.SetInt(gameObject.name + "CarShopFirstOpen", PlayerPrefs.GetInt(gameObject.name + "CarShopFirstOpen") + 1);
        }
        if (PlayerPrefs.GetInt("ShopOpened") == 0 && PlayerPrefs.GetInt(gameObject.name + "ShopUpdateTimeUp") == 1)//如果商店处于未被打开状态并且刷新时间到了就重新生成id并刷新相关信息
        {
            id = Random.Range(2, 12);//随机生成材料id值
            PlayerPrefs.SetInt("Shop_" + gameObject.name + "_id", id);
            PlayerPrefs.SetInt("ShopOpened", 1);
            Initialized();
        }
        else if((PlayerPrefs.GetInt("ShopOpened") == 0 && PlayerPrefs.GetInt(gameObject.name + "ShopUpdateTimeUp") == 0))//如果商店处于未被打开状态并且刷新时间没到，就重新用原来的ID实例化对象并进行相关显示工作
        {
            id = PlayerPrefs.GetInt("Shop_" + gameObject.name + "_id");
            UpdateInfor();
        }
    }
    private void Update()
    {
        UpdateInfor();
        if (car.BuyPrice > PlayerPrefs.GetInt("Diamond"))//如果材料价格大于当前金钱价格则禁用button
        {
            transform.Find(gameObject.name + "_buy").GetComponent<Button>().enabled = false;
        }
        else if(car.BuyPrice <= PlayerPrefs.GetInt("Diamond") && PlayerPrefs.GetInt(gameObject.name+ "IsBuyed") ==0 )//当玩家金钱大于材料价格并且没有买过的时候button才为true
        {
            transform.Find(gameObject.name + "_buy").GetComponent<Button>().enabled = true;
        }
        if(PlayerPrefs.GetInt(gameObject.name + "IsBuyed") == 1)//如果该物品已被购买则使其透明度变为0.5，并且禁用button
        {
            image.color = new Vector4(1, 1, 1, 0.5f);
            transform.Find(gameObject.name + "_buy").GetComponent<Button>().enabled = false;
        }
        if (PlayerPrefs.GetInt(gameObject.name + "ShopUpdateTimeUp") == 1)//商店刷新间隔到了之后就重新初始化
        {
            id = Random.Range(2, 12);//随机生成材料id值
            PlayerPrefs.SetInt("Shop_" + gameObject.name + "_id", id);
            PlayerPrefs.SetInt(gameObject.name + "ShopUpdateTimeUp", 0);
            PlayerPrefs.SetInt(gameObject.name + "IsBuyed", 0);
            image.color = new Vector4(1, 1, 1, 1.0f);//初始化物品购买状态UI信息
        }
        
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("ShopOpened", 0);
        PlayerPrefs.SetInt("Shop_" + gameObject.name + "_id", id);
        PlayerPrefs.SetInt(gameObject.name + "CarShopFirstOpen", PlayerPrefs.GetInt(gameObject.name + "CarShopFirstOpen") + 1);
    }
    private void Initialized()
    {
        PlayerPrefs.SetInt(gameObject.name + "IsBuyed", 0);
        image = gameObject.GetComponent<Image>();
        image.color = new Vector4(1, 1, 1, 1.0f);//初始化物品购买状态UI信息
        UpdateInfor();
    }
    public void UpdateInfor()
    {
        id = PlayerPrefs.GetInt("Shop_" + gameObject.name + "_id");
        Debug.Log(PlayerPrefs.GetInt("Shop_" + gameObject.name + "_id"));
        sp = Resources.Load("Textrues/Cars/Car_" + id.ToString(), typeof(Sprite)) as Sprite;
        image = gameObject.GetComponent<Image>();
        image.sprite = sp;//初始化材料UI图片
        car = CarManager.Instance.GetCarById(id);//实例化材料
        Debug.Log(car.BuyPrice);
        cost = transform.Find(gameObject.name + "_cost").GetComponent<Text>();
        cost.text = car.BuyPrice.ToString();
    }
    public int RetrunID()
    {
        return PlayerPrefs.GetInt("Shop_" + gameObject.name + "_id");
    }
}
