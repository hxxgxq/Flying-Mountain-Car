using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr
{
    #region 
    protected static SceneMgr mInstance;
    public static SceneMgr Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = new SceneMgr();
            }
            return mInstance;
        }
    }
    #endregion

    #region 数据定义

    private GameObject curren;

    public static bool IsJump = false;

    #endregion

    public void SwitchScence(string name)
    {
        GameObject scence = ResourcessMgr.GetInstance().CreateGameObj("UIprefabs/" + name, false);
        if(curren!=null && curren.name != name)
        {
            GameObject.Destroy(curren);
        }
        curren = scence;
    }



}
