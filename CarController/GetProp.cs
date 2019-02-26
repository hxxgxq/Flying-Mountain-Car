using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetProp : MonoBehaviour {

    private GameObject[] propImage = new GameObject[2];
    private string[] proplist = new string[2];
    bool isChange = false;
    bool getProp = false;
    string propName = "";
    public int UsePropTimes;
    private Properties prop;

    void Awake()
    {
        prop = GetComponent<Properties>();
    }

    void Start ()
    {
        UsePropTimes = 0;        
        proplist[0] = "";
        proplist[1] = "";

        //道具模式隐藏prop
        if(PlayerPrefs.GetInt("CurrentScene", 0) == 3 || PlayerPrefs.GetInt("CurrentScene", 0) == 4)
        {
            propImage[0] = GameObject.Find("prop_1");
            propImage[1] = GameObject.Find("prop_2");
            propImage[0].SetActive(false);
            propImage[1].SetActive(false);
        }
        

    }
	
	
	void Update ()
    {
		if(isChange)
        {
            showprop();          
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseProp(proplist[0]);
            proplist[0] = proplist[1];
            proplist[1] = "";
            isChange = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseProp(proplist[1]);
            proplist[1] = "";
            isChange = true;
        }
        
    }
    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="prop"></param>
    void OnTriggerEnter(Collider prop)
    {
        if (prop.gameObject.tag == "tool")
        {
            getProp = true;
        }

        if(prop.gameObject.tag == "Nitrogen")
        {
            GameObject.Find("Energy").GetComponent<Image>().fillAmount += 0.3f;
            Destroy(prop.gameObject);
        }

        if (getProp)
        {
            Destroy(prop.gameObject);            
            Debug.Log("获得道具");
            getProp = false;
            propName = RandomProp();
            if (propName != proplist[0] && propName != proplist[1])
            {
                if (proplist[0] == "")
                {
                    proplist[0] = propName;
                }
                else if (proplist[1] == "")
                {
                    proplist[1] = propName;
                }
                else
                {
                    proplist[0] = proplist[1];
                    proplist[1] = propName;
                }
            }
            Debug.Log(propName);
            isChange = true;
        }
    }

    /// <summary>
    /// 显示道具信息
    /// </summary>
    void showprop()
    {
        for(int i = 0;i<2;i++)
        {
            if(proplist[i] != "")
            {
                propImage[i].SetActive(true);
                propImage[i].GetComponent<Image>().sprite = Resources.Load("UITextures/" + proplist[i], typeof(Sprite)) as Sprite;
            }
            if(proplist[i] == "")
            {
                propImage[i].SetActive(false);
            }
        }
        isChange = false;
    }
    /// <summary>
    /// 随机获得一个道具
    /// </summary>
    /// <returns 道具名称></returns>
    string RandomProp()
    {
    
        string propName = "";
        switch(Random.Range(1, 6))
        {
            case 1:
                propName = "Shield";
                break;
            case 2:
                propName = "Switch";
                break;
            case 3:
                propName = "ThunderStorm";
                break;
            case 4:
                propName = "Wing";
                break;
            case 5:
                propName = "Barry";
                break;
            case 6:
                propName = "Nitrogen";
                break;              
        }
        return propName;
    }
    /// <summary>
    /// 使用道具
    /// </summary>
    /// <param name="propname_1"></param>
    void UseProp(string propname_1)
    {
        if (propname_1 != "") 
        {
            UsePropTimes += 1;
        }
        if(propname_1 == "Shield")
        {
            StartCoroutine(prop.shield());
        }
        if (propname_1 == "Switch")
        {
            prop.PositionChange();
        }
        if (propname_1 == "ThunderStorm")
        {
            prop.ThunderStorm();
        }
        if (propname_1 == "Wing")
        {
            StartCoroutine(prop.Wing());           
        }
        if (propname_1 == "Barry")
        {
            prop.BarrierInstantiate();
        }
        if (propname_1 == "Nitrogen")
        {
            prop.GetN2();
        }
    }
    public int ReturnUsePropTimes()
    {
        return UsePropTimes;
    }




}
