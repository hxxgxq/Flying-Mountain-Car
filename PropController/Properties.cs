using Scripts.Ai;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Properties : MonoBehaviour
{
    //生成障碍 barrierInstantiate
    private GameObject barrier;//在资源文件夹中有一个barrierInstantiation.prefab预制物体
    private Transform barrierTransform;
    private Transform m_transform;
    //交换位置 positionChange
    private Transform[] AI;

    //闪电风暴 ThunderStorm
    public Properties[] Cars;
    private CarAIControl2 controller;

    private bool _shield = true; //保护罩，暂时没有用到
    public bool Shield { get { return _shield; } }

    //翅膀 Wing
    private Rigidbody m_Rigidbody;
    private ConstantForce wing;

    private GameObject[] AIcars;

    //氮气瓶
    private Image N2bottle;

    private bool Lock = true;

    void Awake()
    {
        //生成障碍 barrierInstantiate
        barrierTransform = transform.Find("barrierPos");

        //交换位置 positionChange
        m_transform = transform;

        //闪电风暴 ThunderStorm
        controller = GetComponent<CarAIControl2>();

        //翅膀 Wing
        m_Rigidbody = GetComponent<Rigidbody>();
        wing = GetComponent<ConstantForce>();
        wing.force = new Vector3(0, m_Rigidbody.mass * 9.8f * 3 / 7, 0);
    }

    void Start()
    {
        //生成障碍 barrierInstantiate
        barrier = Resources.Load("PropPrefabs/barrierInstantiation") as GameObject;

        //交换位置 positionChange
        AIcars = GameObject.FindGameObjectsWithTag("AI");
        var children = new Transform[AIcars.Length];
        int n = 0;
        foreach (GameObject child in AIcars)
        {
            children[n++] = child.transform;
        }
        AI = children;

        //闪电风暴 ThunderStorm

        //翅膀 Wing

        //氮气瓶
        N2bottle = GameObject.Find("Energy").GetComponent<Image>();
    }

    //生成障碍 barrierInstantiate
    public void BarrierInstantiate()
    {
        Instantiate(barrier, m_transform.position - m_transform.forward * 0.5f, m_transform.localRotation);
    }

    //交换位置 positionChange
    public void PositionChange()
    {
        float mindistance = float.MaxValue;
        Transform AIcar = m_transform;
        Vector3 player = ProjectionOnXY(m_transform.position);
        Vector3 forward = ProjectionOnXY(m_transform.forward);
        foreach (var car in AI)//遍历AI车，找出前方距离最近的AI车
        {
            Vector3 carpos = ProjectionOnXY(car.position);
            float dist = Vector3.Distance(player, carpos);
            if (dist < mindistance && Vector3.Angle(forward, carpos - player) < 70)
            {                                   //if语句的第二个条件是约束从主角车到AI车的方向必须与主角车的forward方向的偏离不超过70度
                                                //即在主角车的前方140度视野内的AI车，才是可以被交换的车
                mindistance = dist;
                AIcar = car;
            }
        }
        if (AIcar.gameObject.GetComponent<Properties>().Shield)
            changeTransform(m_transform, AIcar);//交换位置信息
    }
    private Vector3 ProjectionOnXY(Vector3 vec)//矢量在XY平面的投影
    {
        float x = vec.x;
        float z = vec.z;
        return new Vector3(x, 0, z);
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

    //闪电风暴 ThunderStorm
    public void ThunderStorm()
    {
        if (gameObject.tag == "Player")
            SetStorm();//玩家车专用，对AI车发风暴
        else
            SetStorm_AI();//AI车专用，对玩家车和其他AI车发风暴
    }
    private void SetStorm()
    {
        foreach (GameObject car in AIcars)//遍历所有其他车，设置风暴
        {                                                                        //会误伤自己，解决方案有二(未实施)：
                                                                                 //1.将数组限定为不含自己的数组
                                                                                 //2.在设置风暴前判断目标车辆是不是自己

            StartCoroutine(car.GetComponent<Properties>().getStorm());//设置风暴
        }
    }
    private void SetStorm_AI()//与上面函数逻辑类似，将来可能会做修改
    {
        foreach (Properties car in Cars)
        {
            StartCoroutine(car.getStorm());
        }
    }
    public IEnumerator getStorm()//车被设置风暴后作用于自己的函数
    {
        if (_shield)
        {
            float maxspeed = controller.m_Maxspeed;
            Debug.Log(controller);
            controller.m_Maxspeed = controller.m_Maxspeed / 2;//速度减半
            yield return new WaitForSeconds(3);//等待三秒
            controller.m_Maxspeed = maxspeed;//速度恢复
        }
    }

    //翅膀 Wing
    public IEnumerator Wing()
    {
        wing.enabled = true;
        yield return new WaitForSeconds(3);
        wing.enabled = false;

    }

    //氮气瓶
    public void GetN2()
    {
        N2bottle.fillAmount += 0.5f;
    }

    public IEnumerator shield()
    {
        _shield = false;
        yield return new WaitForSeconds(3);
        _shield = true;
    }

}