using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//该脚本放置于玩家车上
//与玩家车的前方距离最近的AI车交换位置和旋转信息

public class PositionChange : MonoBehaviour {

    GameObject[] AICars;
    Transform[] AI;//在面板中或者在start函数里绑定上所有的AI车
    private bool change = false; //change变量是个锁，使得按下C时只调用一次Change()
                                 //因为GetKeyDown()有时并不管用
    private Transform m_transform;

    void Awake()
    {
        m_transform = transform;
        AICars = GameObject.FindGameObjectsWithTag("AI");
    }

    //void Update ()
    //{
    //    if (Input.GetKey(KeyCode.C))
    //    {
    //        if (change == false)//锁是开的
    //        {                                      
    //            Change(); //调用change()
    //            change = true;//关锁
    //        }
    //    }
    //    else
    //    {
    //        change = false;//开锁
    //    }
    //}

    public void Change()
    {
        float mindistance = float.MaxValue;
        Transform AIcar = transform;
        Vector3 player = ProjectionOnXY(m_transform.position);
        Vector3 forward = ProjectionOnXY(m_transform.forward);
        foreach (GameObject car in AICars)//遍历AI车，找出前方距离最近的AI车
        {          
            Vector3 carpos = ProjectionOnXY(car.transform.position);
            float dist = Vector3.Distance(player, carpos);
            if (dist < mindistance && Vector3.Angle(forward, carpos-player) < 70)
            {                                   //if语句的第二个条件是约束从主角车到AI车的方向必须与主角车的forward方向的偏离不超过70度
                                                //即在主角车的前方140度视野内的AI车，才是可以被交换的车
                mindistance = dist;
                AIcar = car.transform;
            }
        }
        changeTransform(m_transform, AIcar);//交换位置信息
    }

    private Vector3 ProjectionOnXY(Vector3 vec)//矢量在XY平面的投影
    {
        float x = vec.x;
        float z = vec.z;
        return new Vector3(x, 0 ,z);
    }   

    private void changeTransform(Transform a, Transform b)
    {
        Vector3 temp = a.position;
        a.position = b.position;
        b.position = temp;

        Quaternion _temp = a.rotation;
        a.rotation = b.rotation;
        b.rotation = _temp;
    }


}
