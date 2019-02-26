using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine;
using Scripts.Ai;

//该脚本放置于所有的车上
//闪电风暴，使自己以外的其它所有车速度减半，持续3秒。一次只能一个车用。

public class ThunderStorm : MonoBehaviour {

    ThunderStorm[] Cars;
    GameObject[] AICars;
    private bool thunder = false;//thunder变量是个锁，使得按下C时只设置一次风暴
                                                              //因为GetKeyDown()有时并不管用
    private CarAIControl2 controller;
    private bool shield = true; //保护罩，暂时没有用到

    void Awake()
    {
        controller = gameObject.GetComponent<CarAIControl2>();
        AICars = GameObject.FindGameObjectsWithTag("AI");
    }


    //void Update ()
    //{
    //    if (gameObject.tag == "Player")
    //        SetStorm();//玩家车专用，对AI车发风暴
    //    else
    //        SetStorm_AI();//AI车专用，对玩家车和其他AI车发风暴
    //}


    public void SetStorm()
    {
        //if (Input.GetKey(KeyCode.T))
        //{
        //    if (thunder == false)
        //    {                                        
        //        foreach (ThunderStorm car in Cars)//遍历所有其他车，设置风暴
        //        {                                                                        //会误伤自己，解决方案有二(未实施)：
        //                                                                                 //1.将数组限定为不含自己的数组
        //                                                                                 //2.在设置风暴前判断目标车辆是不是自己

        //            StartCoroutine(car.getStorm());//设置风暴
        //        }
        //        thunder = true;
        //    }
        //}
        //else
        //{
        //    thunder = false;
        //}
        foreach (GameObject car in AICars)
        {
            Debug.Log(car);
            StartCoroutine(car.GetComponent<ThunderStorm>().getStorm());
        }
            

    }


    private void SetStorm_AI()//与上面函数逻辑类似，将来可能会做修改
    {
        if (Input.GetKey(KeyCode.P))
        {
            if (thunder == false)
            {
                foreach (ThunderStorm car in Cars)
                {
                        StartCoroutine(car.getStorm());
                }
                thunder = true;
            }
        }
        else
        {
            thunder = false;
        }
    }

    public IEnumerator getStorm()//车被设置风暴后作用于自己的函数
    {
        if (shield)
        {
            float maxspeed = controller.m_Maxspeed;
            controller.m_Maxspeed= controller.m_Maxspeed / 2;//速度减半
            //Debug.Log("发动闪电风暴");
            yield return new WaitForSeconds(3);//等待三秒
            controller.m_Maxspeed = maxspeed;//速度恢复
        }
    }
}
