using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcessMgr : MonoBehaviour {

    #region
    private static ResourcessMgr mInstance;
    public static ResourcessMgr GetInstance()
    {
        if (mInstance == null)
        {
            mInstance = new GameObject("_ResourcesMgr").AddComponent<ResourcessMgr>();
            mInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        return mInstance;
    }

    private ResourcessMgr()
    {
        hashtable = new Hashtable();
    }
    #endregion

    private Hashtable hashtable;
    /// <summary>
    /// 加载游戏物体
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="path">路径</param>
    /// <param name="cache">是否缓存</param>
    /// <returns></returns>
    public T Load<T>(string path, bool cache) where T : UnityEngine.Object
    {
        if(hashtable.Contains(path))
        {
            return hashtable[path] as T;
        }
        T assetObj = Resources.Load<T>(path);
        if(assetObj == null)
        {
            Debug.LogError("资源不存在，path = " + path);
        }
        if(cache)
        {
            hashtable.Add(path, assetObj);
            Debug.Log("对象缓存， path = " + path);
        }
        return assetObj;
    }
    /// <summary>
    /// 创建游戏物体
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="cache">是否缓存</param>
    /// <returns></returns>
    public GameObject CreateGameObj(string path,bool cache)
    {
        GameObject assetObj = Load<GameObject>(path, cache);
        GameObject go = Instantiate(assetObj) as GameObject;
        go.transform.SetParent(GameObject.Find("Canvas").transform, false);
        if(go == null)
        {
            Debug.LogError("创建失败, path = " + path);
        }

        return go;
    }

}
