using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//该脚本放置于玩家车的barrierPos子物体上，它是一个空物体，在离车正后方约1/3车身距离的一个位置
//在玩家车后设置一个障碍
public class barrierInstantiate : MonoBehaviour {

    GameObject barrier;//在资源文件夹中有一个barrierInstantiation.prefab预制物体，需要绑定
    [SerializeField]
    private bool Lock = true;//Lock变量是个锁，使得按下B时只设置一次障碍
                             //因为GetKeyDown()有时并不管用
    private Transform m_transform;

    void Awake()
    {
        m_transform = transform;
        
    }

    void Update ()
    {
        //if(Input.GetKey(KeyCode.B))
        //{
        //    if (Lock)
        //    {
        //        //生成障碍物
        //        Instantiate(barrier, m_transform.position, m_transform.root.localRotation);
        //        Lock = false;
        //    }
        //}
        //else
        //{
        //    Lock = true;
        //}
	}

    public void CreateBarry()
    {
        barrier = Resources.Load("PropPrefabs/barrierInstantiation") as GameObject;
        Instantiate(barrier, m_transform.position, m_transform.root.localRotation);
    }
}
