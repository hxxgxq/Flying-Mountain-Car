using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {

    //该脚本绑定于barrierInstantiation.prefab上
    //预制物体在场景中存在3秒后自灭

    void Start()
    {
        StartCoroutine(destroy());
    }

    public IEnumerator destroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    
}
