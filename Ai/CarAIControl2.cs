using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityStandardAssets.Vehicles.Car;
namespace Scripts.Ai
//namespace UnityStandardAssets.Vehicles.Car
{
    //[RequireComponent(typeof(CarController))]
    [RequireComponent(typeof(CarAIController))]
    public class CarAIControl2 : MonoBehaviour
    {
        public enum BrakeCondition
        {
            NeverBrake,                 // the car simply accelerates at full throttle all the time.
            TargetDirectionDifference,  // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.
            TargetDistance,             // the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
            // head for a stationary target and come to rest when it arrives there.
        }
        //AI 的三个等级
        public enum AIranks
        {
            Primary,
            Middle,
            Advanced,
        }

        //3中模式的游戏类型
        public enum mode
        {
            normal,
            speed,
            tools,
        }

        //3种难度
        public enum difficuty
        {
            simple,
            general,
            difficult,
        }

        // This script provides input to the car controller in the same way that the user control script does.
        // As such, it is really 'driving' the car, with no special physics or animation tricks to make the car behave properly.

        // "wandering" is used to give the cars a more human, less robotic feel. They can waver slightly
        // in speed and direction while driving towards their target.

        [SerializeField]
        private mode m_mode = mode.normal;                                  //游戏模式设定
        [SerializeField]
        private difficuty m_difficuty = difficuty.simple;                         //游戏难度设定
        [SerializeField]
        [Range(0, 1)]
        private float m_CautiousSpeedFactor = 0.05f;               // percentage of max speed to use when being maximally cautious
        [SerializeField]
        [Range(0, 180)]
        private float m_CautiousMaxAngle = 50f;                  // angle of approaching corner to treat as warranting maximum caution
        [SerializeField]
        private float m_CautiousMaxDistance = 100f;                              // distance at which distance-based cautiousness begins
        [SerializeField]
        private float m_CautiousAngularVelocityFactor = 30f;                     // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)
        [SerializeField]
        private float m_SteerSensitivity = 0.05f;                                // how sensitively the AI uses steering input to turn to the desired direction
        [SerializeField]
        private float m_AccelSensitivity = 0.04f;                                // How sensitively the AI uses the accelerator to reach the current desired speed
        [SerializeField]
        private float m_BrakeSensitivity = 1f;                                   // How sensitively the AI uses the brake to reach the current desired speed
        [SerializeField]
        private float m_LateralWanderDistance = 3f;                              // how far the car will wander laterally towards its target
        [SerializeField]
        private float m_LateralWanderSpeed = 0.1f;                               // how fast the lateral wandering will fluctuate
        [SerializeField]
        [Range(0, 1)]
        private float m_AccelWanderAmount = 0.1f;                  // how much the cars acceleration will wander
        [SerializeField]
        private float m_AccelWanderSpeed = 0.1f;                                 // how fast the cars acceleration wandering will fluctuate
        [SerializeField]
        private BrakeCondition m_BrakeCondition = BrakeCondition.TargetDistance; // what should the AI consider when accelerating/braking?
        [SerializeField]
        public bool m_Driving=true;                                                  // whether the AI is currently actively driving or stopped.
        //[SerializeField]
        private Transform m_Target;                                              // 'target' the target object to aim for.
        [SerializeField]
        private bool m_StopWhenTargetReached;                                    // should we stop driving when we reach the target?
        [SerializeField]
        private float m_ReachTargetThreshold = 2;                                // proximity to target to consider we 'reached' it, and stop driving.
        [SerializeField]
        private float seedistance = 6;                                                     //AI车的预测距离
        //[SerializeField]
        //private AIranks m_AIRank = AIranks.Primary;                                                                //记录AI的等级
      


        private float m_RandomPerlin;             // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)
        private CarAIController m_CarController;    // Reference to actual car controller we are controlling
        private float m_AvoidOtherCarTime;        // time until which to avoid the car we recently collided with
        private float m_AvoidthingsTime;        // time until which to avoid the car we recently collided with
        //private float m_preAvoidOtherCarTime;        //提前检测前方障碍并躲避的时间
        //private float m_AvoidOtherCarSlowdown;    // how much to slow down due to colliding with another car, whilst avoiding
        private float m_AvoidPathOffset;          // direction (-1 or 1) in which to offset path to avoid other car, whilst avoiding
        private Rigidbody m_Rigidbody;
        //private int m_states;                              //记录AI的状态0，正常，1倒车躲避墙体
        //private Vector3 m_hitpointfa;                     //记录对于墙壁碰撞点的法线，用作车辆调整
        //private Vector3 m_hitpointturn;                     //记录对于预测到的障碍物的偏转方向
        private Vector3 tmpTargetPos;                       //记录中间生成的临时节点
        float m_AIhandbreak;                            //用于漂移的手刹控制
        private float m_turntime;                       //控制漂移时间
        private Vector3[] m_forces=new Vector3[9];                          //记录不同状况下的作用力值（0撞墙，1装到障碍物，2撞到汽车，3看到墙壁，4看到障碍物或是AI车，5看到道具，6.附近有AI车，7车后面有玩家，8车前面有玩家）
        //上述作用力最终会反映到车辆的转向上，令包括基本力在内的所有力的值存在则为1，关键是对各个力的权值处理
        private float[] m_forcefactor = { 1, 1, 1, 1, 3, 1, 1, 0.3f, 0.5f,};                           //各个力的权值
        private Vector3 baseforce=new Vector3();                             //基础作用力来自于原始的架势方向
        private float[] m_speedfactor={1,1,1,1,1,1,1,1,1,};                          //上述不同状况下对速度的影响
        private float m_speedf=1;                                                        //综合后速度影响因素
        private Vector3 sumforce=Vector3.zero;                   //合力值
        [Range(0,1)] 
       public float danqi=0;                                                                       //氮气值,限定在0-1之间
        //float[] tooltime = { 0, 0, 0, 0, 0, 0 };                                                //道具持续时间，（使用氮气的作用时间，翅膀，商店风暴，护盾，路障，交换）
        public float m_Maxspeed;                            //AI车的当前最大速度，可能是原始最大速度的一半
        bool superturnstate=false;                        //判断是否进入可以漂移的场所
        public float changRoadfactor = 0.5f;                                //AI车换道的几率
        public float m_mass;
        private Getprop m_prop;
        private float toolMaxStayTime =10;                            //获得道具后的道具最长保留时间（例如：氮气--超过1时最长保留10秒，在这期间持续积攒氮气）
        private float[] toolStayTime = { 0,0,0};                      //当前停留时间（氮气的使用，飞行，障碍物）

        //速度变化时，只在回归1时连续
        //力度变化时，增加力度的速度大于减少力度，
        
        //记录漂移有关的信息
        public struct Superturn
        {
            public float AIturnAnger;                                //记录漂移的角度
            public float direction;                                     //记录漂移方向
        }
        Superturn myturn=new Superturn();

        private void Awake()
        {
            // get the car controller reference
            m_CarController = GetComponent<CarAIController>();
            if (m_CarController == null)
            {
                Debug.Log("not found carcontroller");
            }
               

            // give the random perlin a random value
            m_RandomPerlin = Random.value * 100;
            m_Maxspeed = m_CarController.MaxSpeed;
            m_Rigidbody = GetComponent<Rigidbody>();
            m_mass = m_Rigidbody.mass;
            m_prop = GetComponent<Getprop>();
            m_AIhandbreak = 0;

            //GetComponent<WaypointProgressTracker>().Reset();            
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt("CurrentScene", 0) == 1)//如果是竞速第一关
            {
                SetModeAndDifficulty(mode.speed, difficuty.general);
            }
            else if (PlayerPrefs.GetInt("CurrentScene", 0) == 2)//竞速第二关
            {
                SetModeAndDifficulty(mode.speed, difficuty.difficult);
            }
            else if (PlayerPrefs.GetInt("CurrentScene", 0) == 3)//道具第一关
            {
                SetModeAndDifficulty(mode.tools, difficuty.general);
            }
            else if (PlayerPrefs.GetInt("CurrentScene", 0) == 4)//道具第二关
            {
                SetModeAndDifficulty(mode.tools, difficuty.difficult);
            }
        }
        private void FixedUpdate()
        {
            m_Target = GetComponent<WaypointProgressTracker>().getTarget();

            if (m_Target == null || !m_Driving)
            {
                // Car should not be moving,
                // use handbrake to stop
                //Debug.Log("null");
                //m_CarController.Move(0, 0, -1f, 1f);//一开始没能认真理解
            }
            else
            {
                Vector3 fwd = transform.forward;
                if (m_Rigidbody.velocity.magnitude > m_CarController.MaxSpeed * 0.1f)
                {
                    fwd = m_Rigidbody.velocity;
                }

                float desiredSpeed = m_Maxspeed;

                // now it's time to decide if we should be slowing down...
                switch (m_BrakeCondition)
                {
                    case BrakeCondition.TargetDirectionDifference:
                        {
                            // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.

                            // check out the angle of our target compared to the current direction of the car
                            float approachingCornerAngle = Vector3.Angle(m_Target.forward, fwd);

                            // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
                            float spinningAngle = m_Rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;

                            // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
                            float cautiousnessRequired = Mathf.InverseLerp(0, m_CautiousMaxAngle,
                                                                           Mathf.Max(spinningAngle,
                                                                                     approachingCornerAngle));
                            desiredSpeed = Mathf.Lerp(m_CarController.MaxSpeed, m_CarController.MaxSpeed * m_CautiousSpeedFactor,
                                                      cautiousnessRequired);
                            break;
                        }

                    case BrakeCondition.TargetDistance:
                        {
                            // the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
                            // head for a stationary target and come to rest when it arrives there.

                            // check out the distance to target
                            Vector3 delta = m_Target.position - transform.position;
                            float distanceCautiousFactor = Mathf.InverseLerp(m_CautiousMaxDistance, 0, delta.magnitude);

                            // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
                            float spinningAngle = m_Rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;

                            // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
                            float cautiousnessRequired = Mathf.Max(
                                Mathf.InverseLerp(0, m_CautiousMaxAngle, spinningAngle), distanceCautiousFactor);
                            desiredSpeed = Mathf.Lerp(m_CarController.MaxSpeed, m_CarController.MaxSpeed * m_CautiousSpeedFactor,
                                                      cautiousnessRequired);
                            break;
                        }

                    case BrakeCondition.NeverBrake:
                        break;
                }

                // 记录在路径中的目标位置
                Vector3 offsetTargetPos = m_Target.position;
                tmpTargetPos = m_Target.position;
                baseforce = (m_Target.position - transform.position).normalized*3;//使基础力为3
                Debug.DrawLine(transform.position, transform.position + baseforce);


               
                //决策层处理             
                Decide();
                sumforce=sumforce.normalized*6;//对终止力进行归一化
                //当前行驶目标的位置
                offsetTargetPos = sumforce + transform.position;
                //当前速度等于最大速度*减速因子，令理想速度为正直，方向由m_speedfactoe[0]撞墙因素决定
                desiredSpeed *= m_speedf;
                desiredSpeed = Mathf.Abs(desiredSpeed);

                //Debug.Log(m_speedf + ":" + m_CarController.MaxSpeed + ":" + desiredSpeed + " : " + m_CarController.CurrentSpeed+":"+m_AIhandbreak);

                //show the velocoty and target
                Debug.DrawLine(transform.position, offsetTargetPos, Color.green);
                //Debug.DrawLine(transform.position, m_Target.position, Color.blue);

                // use different sensitivity depending on whether accelerating or braking:
                float accelBrakeSensitivity = (desiredSpeed < m_CarController.CurrentSpeed)
                                                  ? m_BrakeSensitivity
                                                  : m_AccelSensitivity;

                //因为当前速度总是正直
                // decide the actual amount of accel/brake input to achieve desired speed.
                float accel = Mathf.Clamp((desiredSpeed - m_CarController.CurrentSpeed) * accelBrakeSensitivity, -1, 1);

                // add acceleration 'wander', which also prevents AI from seeming too uniform and robotic in their driving
                // i.e. increasing the accel wander amount can introduce jostling and bumps between AI cars in a race
                accel *= (1 - m_AccelWanderAmount) +
                         (Mathf.PerlinNoise(Time.time * m_AccelWanderSpeed, m_RandomPerlin) * m_AccelWanderAmount);

                // calculate the local-relative position of the target, to steer towards
                Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);

                // work out the local angle towards the target
                float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

                // get the amount of steering needed to aim the car towards the target
                //转向角度随速度的正负变化
                float steer = Mathf.Clamp(targetAngle * m_SteerSensitivity, -1, 1) * Mathf.Sign(m_CarController.CurrentSpeed);

                // feed input to the car controller.
                //倒车或刹车时accel是负数
                //Debug.Log("car"+m_AIhandbreak);
                //m_CarController.Move(steer, accel*m_speedfactor[0], accel * m_speedfactor[0], m_AIhandbreak);
                m_CarController.Move(steer, accel , accel , m_AIhandbreak);
                // if appropriate, stop driving when we're close enough to the target.
                if (m_StopWhenTargetReached && localTarget.magnitude < m_ReachTargetThreshold)
                {
                    m_Driving = false;
                }
            }
        }


        public void SetTarget(Transform target)
        {
            m_Target = target;
            m_Driving = true;
        }

        public void SetDring(bool dring)
        {
            m_Driving = dring;
        }
        public void SetModeAndDifficulty(mode m,difficuty d)
        {
            m_mode = m;
            m_difficuty = d;
        }
        //问题：每种力的持续效果？？？
        //决策函数，处理不同模式和难度下的状态迁移
        public void Decide()
        {
            //优先处理极端情况，及车辆跳出赛道
                //碰撞作用处理1，碰撞到车辆
                if (Time.time > m_AvoidOtherCarTime)
                {
                    //碰撞结束，作用力归零
                    m_forces[2] = Vector3.zero;
                    m_speedfactor[2] = 1;
                }
                //碰撞到障碍物或是墙壁
                if(Time.time>m_AvoidthingsTime)
                {
                    //碰撞结束，作用力归零
                    m_forces[0] = Vector3.zero;
                    m_speedfactor[0] = 1;
                    m_forces[1] = Vector3.zero;
                    m_speedfactor[1] = 1;
                }
               //不同模式有不同的功能
                switch (m_mode)
                {
                    //剧情模式下，采用一般的碰撞函数和一般的躲避策略
                    case mode.normal:
                        {
                            advance0();
                            fly();//处理飞跃
                            break;
                        }
                    //竞速模式下，根据难度决定策略
                    case mode.speed:
                        {
                            advance0();
                            fly();
                            switch (m_difficuty)
                            {
                                //竞速模式下的简单模式
                                case difficuty.simple:
                                    {
                                        changRoadfactor = 0.2f;//简单模式换道几率为0.2f；
                                        avoidPlayer();//躲避玩家
                                        break;
                                    }
                                //一般模式
                                case difficuty.general:
                                    {
                                        changRoadfactor = 0.8f;
                                        NAccumulate();//积攒氮气
                                        sprint();//智能冲刺
                                        break;
                                    }
                                //困难模式
                                case difficuty.difficult:
                                    {
                                        changRoadfactor = 1;
                                        hinder();//阻挡玩家
                                        supturn();//自动漂移积攒氮气
                                        sprint();//智能冲刺
                                        break;
                                    }
                                default:
                                    {
                                        Debug.Log("error difficity");
                                        break;
                                    }
                            }
                            break;
                        }
                    //道具模式
                    case mode.tools:
                        {
                            advance0();
                            fly();
                            avoidPlayer();
                            switch (m_difficuty)
                            {
                                //简单道具模式下，AI不会使用道具，不会主动吃道具
                                case difficuty.simple:
                                    {
                                        changRoadfactor = 0.5f;
                                        break;
                                    }
                                //普通模式下，AI不会主动吃道具，会主动使用道具
                                case difficuty.general:
                                    {
                                        changRoadfactor = 0.8f;
                                        usetool();
                                        break;
                                    }
                                //困难模式下，AI主动吃道具，主动使用道具
                                case difficuty.difficult:
                                    {
                                        changRoadfactor = 1;
                                        eattools();//主动吃道具
                                        usetool();
                                        break;
                                    }
                                default:
                                    {
                                        Debug.Log("error difficity");
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            Debug.Log("error mode");
                            break;
                        }
                
                }

                sumforce=baseforce;//合力
                m_speedf = 1;
                //Debug.Log(m_forces[4]);
                //Debug.Log(m_forcefactor[4]);
                //对作用力求加权和
                for(int i=0;i<9;i++)
                {
                    sumforce += m_forces[i]*m_forcefactor[i];
                    if(m_forces[i].magnitude>0)
                    {
                        Debug.DrawLine(transform.position,transform.position+m_forces[i],Color.blue);
                        //Debug.Log(i);
                    }
                }
                //求速度的综合影响因素
                for (int i = 0; i < 9; i++)
                {
                     m_speedf*= m_speedfactor[i];
                }
                //有回归不用倒车
                /*
                if (Time.time < m_AvoidthingsTime)//倒退不减速
                {
                    m_speedf = -1;
                }
                */
            /*
            //漂移过程中不处理其他力
                if (superturnstate == true)
                {
                    sumforce = Vector3.zero;
                    m_speedf = 0.5f;//漂移减速
                    Debug.Log("super turn");
                }*/
                //如果说合力效果和基础力一致，那么说明没有外来因素影响，可以采用噪声函数进行加躁，基本力标准化之后，噪声也要变化
           
             //无论有没有受力都应该受噪声影响
                    Vector3 polin=Vector3.zero;
                    //对基本力的归一化需要考虑距离为0
                    //Debug.Log("random");
                    /*if ((m_Target.position - transform.position).magnitude < 1)
                    {
                        polin = m_Target.right *
                               (Mathf.PerlinNoise(Time.time * m_LateralWanderSpeed, m_RandomPerlin) * 2 - 1) *
                               m_LateralWanderDistance * (m_Target.position - transform.position).magnitude;
                        Debug.DrawLine(transform.position, transform.position + polin);
                        //Debug.Log("polin" + polin);
                    }
                    else
                    {
                        polin = m_Target.right *
                               (Mathf.PerlinNoise(Time.time * m_LateralWanderSpeed, m_RandomPerlin) * 2 - 1) *
                               m_LateralWanderDistance / (m_Target.position - transform.position).magnitude;
                        Debug.DrawLine(transform.position, transform.position + polin);
                    }
                    */
                    polin = m_Target.right *
                              (Mathf.PerlinNoise(Time.time * m_LateralWanderSpeed, m_RandomPerlin) * 2 - 1) *
                              m_LateralWanderDistance;
                    sumforce += polin;


                //Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue);//注意所有使用所谓方向量的数据
                //Debug.DrawLine(transform.position, transform.position + transform.right, Color.blue);//注意所有使用所谓方向量的数据
                //Debug.Log(m_speedf);
            }

        //记录不同状况下的作用力值（0撞墙，1装到障碍物，2撞到汽车，3看到墙壁，4看到障碍物，5看到道具，6.看到AI车，7车后面有玩家，8车前面有玩家）

        //处理一般的状况，碰撞到了车辆、墙壁或一般障碍物，不处理道具
        private void OnCollisionStay(Collision col)
        {
            //Debug.Log(col.collider.name);
            // detect collision against other cars, so that we can take evasive action
            //车辆判断序号2
            if (col.rigidbody != null)
            {
                var otherAI = col.rigidbody.GetComponent<CarAIControl2>();
                if (otherAI != null)
                {
                    // we'll take evasive action for 1 second
                    m_AvoidOtherCarTime = Time.time + 1;

                    // but who's in front?...
                    if (Vector3.Angle(transform.forward, otherAI.transform.position - transform.position) < 90)
                    {
                        // the other ai is in front, so it is only good manners that we ought to brake...
                        if (Vector3.Angle(transform.forward, otherAI.transform.forward) > 70)//是否垂直碰撞
                        {
                            m_speedfactor[2] = -0.5f;
                        }
                        else m_speedfactor[2]= 0.5f;
                    }

                    // both cars should take evasive action by driving along an offset from the path centre,
                    // away from the other car
                    var otherCarLocalDelta = transform.InverseTransformPoint(otherAI.transform.position);
                    float otherCarAngle = Mathf.Atan2(otherCarLocalDelta.x, otherCarLocalDelta.z);
                    m_AvoidPathOffset = m_LateralWanderDistance * -Mathf.Sign(otherCarAngle);
                    m_forces[2] = m_Target.right * m_AvoidPathOffset;
                }
                else
                {
                    //if other car is player
                    var othercar = col.rigidbody.GetComponent<CarController>();
                    if (othercar != null)
                    {
                        // we'll take evasive action for 1 second
                        m_AvoidOtherCarTime = Time.time + 1;

                        // but who's in front?...
                        if (Vector3.Angle(transform.forward, othercar.transform.position - transform.position) < 90)
                        {
                            // the other car is in front, so it is only good manners that we ought to brake...
                            m_speedfactor[2] = 0.5f;
                        }
                    }
                }
            }
            
        //碰到墙壁倒退一点，或是垂直撞到障碍物，障碍物序号1，墙体序号0，道具是触发器不用管
            else 
        {
            //如果碰撞点的法线朝上，那么，碰撞物并非墙体，
            if (Vector3.Angle(Vector3.up, col.contacts[0].normal) > 45)
            {
                //判断发生碰撞的位置是车前还是车后 ，只有在车前墙体发生碰撞才会被处理，只有碰撞幅度足够大才会倒车
                if (Vector3.Angle(transform.forward, col.contacts[0].point - transform.position) < 90 && Vector3.Angle(transform.forward, col.contacts[0].normal) > 100)
                {
                    // we'll take evasive action for 3.5 second
                    m_AvoidthingsTime = Time.time + 2.5f;//由于回归能力，撞墙只需要记录时间即可，次时间内无法进行漂移
                    //m_forces[0] = (transform.position - m_Target.position)*2 + col.contacts[0].normal*5;
                    //m_forces[0] = (transform.position - m_Target.position)*2+col.contacts[0].normal * 5;
                    //m_speedfactor[0]= -1;
                    //m_speedfactor[1] = 1;
                }
            }
        }            
        }   
        private void OnTriggerEnter(Collider col)
        {
            //碰到触发器（漂移点）
            if ((col.tag.Equals("turnpoint") || col.transform.root.tag.Equals("turnpoint")) && superturnstate == false && m_mode == mode.speed && m_difficuty == difficuty.difficult)//只有当处于竞速困难模式下才可以处理漂移
                {
                    myturn.AIturnAnger = col.GetComponent<turnAnger>().anger;//获取漂移的旋转角度
                    myturn.direction = col.GetComponent<turnAnger>().direction;//获取漂移的旋转角度
                    Debug.Log("turn point");
                }
        }

        //一般的避让策略
        private void advance0()
        {
            
            avoidNeibour();
            //建立三条射线
            //seedistance = 6;//预测距离
            RaycastHit hit, hitl, hitr;
            //检测前方一米处的物体
            Vector3 source =transform.up*(-0.05f)+transform.position + transform.forward;
            Vector3 direction = transform.forward * seedistance;
            int sideaway = 0;//0表示没有贴墙走，1表示贴左墙，2表示贴右墙

            Vector3 offset = transform.right * 0.3f;//范围预测
            Ray ray = new Ray(source, direction);
            Debug.DrawLine(source, source + direction, Color.red);
            Ray rayleft = new Ray(source - offset, direction-offset);
            Debug.DrawLine(source - offset, source + direction - 2*offset, Color.red);
            Ray rayright = new Ray(source + offset, direction+offset);
            Debug.DrawLine(source + offset, source + direction + 2*offset, Color.red);
            Ray sidel = new Ray(source - offset, -transform.right);//检测车辆左侧
            Debug.DrawLine(source - offset, source - transform.right - offset, Color.red);
            Ray sider = new Ray(source + offset, transform.right);//检测车辆右侧
            Debug.DrawLine(source + offset, source + transform.right + offset, Color.red);


            //判断车辆是否贴墙而行
            if (Physics.Raycast(sidel, 1)) sideaway = 1;
            if (Physics.Raycast(sider, 1)) sideaway = 2;

            m_forces[5] = Vector3.zero;
            m_speedfactor[5] = 1;

            //归零、、尝试2次递减
            if (m_forces[3].magnitude > Time.deltaTime)
            {
                m_forces[3] -= m_forces[3].normalized * Time.deltaTime*0.5f;//每秒0.5f的速度将力归零
            }
            else
                m_forces[3] = Vector3.zero;

            if (m_forces[4].magnitude > Time.deltaTime)
            {
                m_forces[4] -= m_forces[4].normalized * Time.deltaTime*0.5f;
            }
            else
                m_forces[3] = Vector3.zero;

            if (m_speedfactor[3] < 1)
            {
                m_speedfactor[3] += Time.deltaTime * 0.15f;//注意速度因子变化的速率为每秒增加0.15f
            }
            else m_speedfactor[3] = 1;
            if (m_speedfactor[4] < 1)
            {
                m_speedfactor[4] += Time.deltaTime * 0.15f;//注意速度因子变化的速率为每秒增加0.15f
            }
            else m_speedfactor[4] = 1;

            //看到障碍物序号4，看到墙壁序号3
            //如果是障碍物的话，自动躲避，自动远离墙壁
            //中间射线的检测结果
            if (Physics.Raycast(ray, out hit, seedistance))
            {
                //Debug.DrawLine(hit.point, transform.position, Color.green);
                //障碍物在左边，车右移，否则左移
                if (hit.collider != null && !hit.collider.isTrigger)
                {
                    //只对于障碍物躲避，不躲避爬坡
                    //if (Vector3.Angle(Vector3.up, hit.normal) > 45)
                    //{
                    if (hit.collider.tag.Equals("obstical") || hit.collider.GetComponent<CarAIControl2>() != null)
                    {
                        //Debug.Log(Vector3.Angle(Vector3.up, hit.normal));
                        //Debug.DrawLine(hit.point,hit.point+hit.normal);   
                        if (hit.collider.tag.Equals("obstical"))
                        {
                            if (Vector3.Angle(transform.right, hit.collider.transform.position - transform.position) < 80 && Vector3.Angle(transform.forward, hit.collider.transform.position - transform.position) < 10)//在右侧，不要完全依赖
                            {
                                m_forces[4] += -transform.right * Time.deltaTime * 3 * m_Rigidbody.velocity.magnitude/20;//向左的力(每秒1的大小增大)                                 
                                //Debug.Log("turn left");
                            }
                            else
                            {
                                m_forces[4] += transform.right * Time.deltaTime * 3;//向右的力
                                //Debug.Log("turn right");
                            }

                            if (sideaway == 1)
                            {
                                m_forces[3] += transform.right * Time.deltaTime;
                                m_forces[4] += transform.right * Time.deltaTime;
                            }
                            else if (sideaway == 2)
                            {
                                m_forces[3] += -transform.right * Time.deltaTime;
                                m_forces[4] += transform.right * Time.deltaTime;
                            }
                            m_speedfactor[4] = 0.6f;
                        }
                        else
                        {
                            //判断与AI车的相对速度，如果自己的车速大，有超车风险，才会转向
                            if(m_Rigidbody.velocity.magnitude>hit.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude)
                            {
                                if (sideaway == 1)
                                {
                                    m_forces[3] += transform.right * Time.deltaTime;
                                }
                                else 
                                {
                                    m_forces[3] += -transform.right * Time.deltaTime;
                                }
                            }
                        }

                    }
                        if (hit.transform.root.tag.Equals("wall") && Vector3.Angle(hit.normal, Vector3.up) > 60&&!hit.transform.tag.Equals("podao"))//检测到的是墙体时
                        //if (hit.collider.tag.Equals("wall"))//检测到的是墙体时
                        {
                            Debug.Log(hit.collider.name + "wall");
                            float dis = Vector3.Dot(transform.position - hit.point, hit.normal);//阻碍力应该随距离减小而增加
                            Mathf.Clamp(dis, 0.5f, 2);
                            if(Vector3.Angle(transform.forward,-hit.normal)>60)//侧向墙体施加法相力，正向墙体施加侧向力
                                  m_forces[3] += hit.normal / dis * Time.deltaTime * 3 * m_Rigidbody.velocity.magnitude / 20;//针对侧向墙体
                            else
                                m_forces[3] = transform.right * Time.deltaTime * 3 * m_Rigidbody.velocity.magnitude / 20;
                        }
                    }
                //看到道具走上前去
                if (hit.transform.tag == "tool" || hit.transform.tag == "Nitrogen"&&m_difficuty==difficuty.difficult)
                {
                    m_forces[5] = hit.collider.transform.position - transform.position - baseforce;//若是距离越近越容易靠近，是否考虑分母为0？
                    //Debug.Log("see tool1");
                }
                //Debug.Log(m_forces[4]);
                //}
            }
            
            //左侧射线处理（左右同时检测到或只有左侧检测到时覆盖上面的力）
            if (Physics.Raycast(rayleft, out hitl, seedistance))
            {
                //障碍物在左边，车右移，否则左移
                if (hitl.collider != null && !hitl.collider.isTrigger)
                {
                    if (hitl.collider.tag.Equals("obstical"))
                    {
                        //只对于障碍物躲避，不躲避爬坡
                       // if (Vector3.Angle(Vector3.up, hitl.normal) > 45)
                        //{
                            Debug.Log(hitl.collider.name + "leftobstical");
                            //Debug.Log("left see");
                            m_forces[4] += transform.right * Time.deltaTime*3;
                            //Debug.Log("turn right");
                            m_speedfactor[4] = 0.8f;
                        //}
                    }
                    if (Vector3.Angle(hitl.normal, Vector3.up) > 60 && hitl.transform.root.tag.Equals("wall")&&!hitl.transform.tag.Equals("podao"))
                    //if (hitl.collider.tag.Equals("wall"))
                    {
                        //Debug.Log(hitl.collider.name + "leftwall");
                        float dis = Vector3.Dot(transform.position - hitl.point, hitl.normal);
                        Mathf.Clamp(dis, 0.5f, 2);
                        if (Vector3.Angle(transform.forward, -hitl.normal) > 60)//侧向墙体施加法相力，正向墙体施加侧向力
                            m_forces[3] += hitl.normal / dis * Time.deltaTime * 3 * m_Rigidbody.velocity.magnitude / 20;//针对侧向墙体
                        else
                            m_forces[3] += transform.right * Time.deltaTime * 3 * m_Rigidbody.velocity.magnitude / 20;              
                    }
                }
                //看到道具走上前去
                if (hitl.transform.tag == "tool" || hitl.transform.tag == "Nitrogen" && m_difficuty == difficuty.difficult)
                {
                    m_forces[5] = hitl.collider.transform.position - transform.position - baseforce;//若是距离越近越容易靠近，是否考虑分母为0？
                    //Debug.Log("see tool2");
                }
                //Debug.Log(m_forces[4]);
            }
            //右侧实物
            if (Physics.Raycast(rayright, out hitr, seedistance))
            {
                //障碍物在左边，车右移，否则左移
                if (hitr.collider != null && !hitr.collider.isTrigger)
                {
                    if (hitr.collider.tag.Equals("obstical"))
                    {
                        //只对于障碍物躲避，不躲避爬坡
                       // if (Vector3.Angle(Vector3.up, hitr.normal) > 45)
                        //{
                            Debug.Log(hitr.collider.name + "rightobstical");
                            //Debug.Log("right see");
                            m_forces[4] += -transform.right * Time.deltaTime*3;
                            //Debug.Log("turn left");
                            m_speedfactor[4]= 0.8f;
                        //}
                    }
                    if (Vector3.Angle(hitr.normal, Vector3.up) > 60 && hitr.transform.root.tag.Equals("wall") && !hitr.transform.tag.Equals("podao"))
                    //if (hitr.collider.tag.Equals("wall"))
                    {
                        //Debug.Log(hitr.collider.name + "rightwall");
                        float dis = Vector3.Dot(transform.position - hitr.point, hitr.normal);
                        Mathf.Clamp(dis, 0.5f, 2);
                        if (Vector3.Angle(transform.forward, -hitr.normal) > 60)//侧向墙体施加法相力，正向墙体施加侧向力
                            m_forces[3] += hit.normal / dis * Time.deltaTime * 3 * m_Rigidbody.velocity.magnitude / 20;//针对侧向墙体
                        else
                            m_forces[3] += -transform.right * Time.deltaTime * 3 * m_Rigidbody.velocity.magnitude / 20;
                    }
                }
                //看到道具走上前去
                if (hitr.transform.tag == "tool" || hitr.transform.tag == "Nitrogen" && m_difficuty == difficuty.difficult)
                {
                    m_forces[5] = hitr.collider.transform.position - transform.position - baseforce;//若是距离越近越容易靠近，是否考虑分母为0？
                    //Debug.Log("see tool3");
                }
            }
        }

        //避让玩家序号8
        private void avoidPlayer()
        {
            if (m_forces[8].magnitude > Time.time)
            {
                m_forces[8] -= m_forces[7].normalized * Time.deltaTime*0.5f;
            }
            else
                m_forces[8] = Vector3.zero;
            m_speedfactor[8] = 1;
            Collider[] col = Physics.OverlapSphere(transform.position + transform.forward * seedistance/2, 1);//预测3米前1米半径球内是否有玩家
            //Debug.DrawRay(transform.position,transform.position + transform.forward * 5,Color.blue);
            foreach(var item in col)
            {
                if(item.transform.root.tag.Equals("Player"))
                {
                    //Debug.Log("player");
                    if (Vector3.Angle(transform.position - item.transform.position,transform.right)>90)//玩家在AI车右侧
                        m_forces[8] += -transform.right * Time.deltaTime*3;
                    else
                        m_forces[8] += transform.right * Time.deltaTime*3;
                    break;
                }
            }
        }

        //积攒氮气（不要处理消耗）
        private void NAccumulate()
        {
            if(m_Rigidbody.velocity.magnitude>0&&m_Maxspeed!=1.5f*m_CarController.MaxSpeed)//非加速状态下积攒氮气
            {
                //Debug.Log("积攒氮气"+danqi);
                danqi+=Time.deltaTime*0.1f;//10s积攒满1氮气值
            }
            if (danqi > 1 && m_Maxspeed != 1.5f * m_CarController.MaxSpeed) //非加速状态下氮气值达到1开始记录保留时间
                toolStayTime[0] += Time.deltaTime;
        }

        //冲刺检测：坡道，停留时间，当前状态
        private bool sprintCheck()
        {
            if (m_Maxspeed == 1.5f * m_CarController.MaxSpeed)
            {
                //Debug.Log(m_Maxspeed+":"+m_CarController.MaxSpeed);
                return false;
            }
            Collider[] col = Physics.OverlapSphere(transform.position + transform.forward * seedistance, 3);//前方6米是否有爬坡
            
            //Debug.DrawLine(transform.position, transform.position + transform.forward * seedistance,Color.green);
            foreach (var item in col)
            {
                if (item.tag == "podao")//发现前方有坡道
                {
                    Debug.Log("podao");
                    return true;
                }
            }
            return false;
        }

        //使用氮气冲刺
        private void sprint()
        {
            //氮气足够，检测是否可以使用
            if (danqi > 1)
            {
                if (sprintCheck() || toolStayTime[0] > 10)
                {
                    //开始加速时，停留时间置零，速度增加
                    Debug.Log("sprint");
                    toolStayTime[0] = 0;
                    m_Maxspeed = 1.5f * m_CarController.MaxSpeed;
                }
                else
                    toolStayTime[0] += Time.deltaTime;
            }
            if (m_Maxspeed == 1.5f * m_CarController.MaxSpeed)//冲刺时消耗氮气
            {
                danqi -= Time.deltaTime * 0.5f;
            }
            if (danqi < Time.deltaTime)//氮气不足停止加速
            {
                m_Maxspeed = m_CarController.MaxSpeed;
            }
        }

        //自动尝试漂移，漂移会积攒氮气
        private void supturn()
        {
            //如果可以进行漂移，查看一下当前状态，如果不是撞击到障碍物或是撞墙回归的话，可以进行漂移
            if(Time.time>m_AvoidOtherCarTime&&Time.time>m_AvoidthingsTime)
            {
                //漂移的方法是将车辆减小一定重力，按住手刹，并且旋转，旋转按照每帧一定角度进行，获取转角度数，漂移积攒的氮气与转角读书成正比
                if (myturn.AIturnAnger > 0)
                {
                    transform.RotateAround(transform.position + 0.8f * transform.forward, transform.up, myturn.direction * 80 * Time.deltaTime);//每秒旋转80
                    //m_Rigidbody.AddForce(m_Rigidbody.mass * 9.8f * Vector3.up / 6);//添加一个1/6的反重力，方便滑动
                    m_Rigidbody.mass = m_mass * 0.5f;
                    myturn.AIturnAnger -= 90 * Time.deltaTime;
                    m_AIhandbreak = 0.8f;//手刹置为1，后轮锁住
                    danqi +=Time.deltaTime;//漂移过程积攒氮气值，每秒积攒1氮气
                    superturnstate = true;
                    Debug.Log("super turn");
                    //Debug.Log(myturn.AIturnAnger);
                }
                else
                {
                     superturnstate = false;//漂移结束
                     m_Rigidbody.mass = m_mass;
                     m_AIhandbreak = 0;
                }
            }
            else
            {
                  m_Rigidbody.mass = m_mass;
                  superturnstate = false;//漂移结束
                  m_AIhandbreak = 0;
            }                
        }

        //直道冲刺，判断是否是直道，如果是直道且没有来自墙壁，障碍物的阻碍力，且氮气值达到指定数目，可以尝试冲刺
        /*
        private void AIsprint()
        {
            if (m_Maxspeed == 3/2 * m_CarController.MaxSpeed)//如果正在冲刺
            {
                   danqi -= Time.deltaTime * 0.5f;//2秒使用完氮气
                   Debug.Log("冲刺");
            }
            if (danqi > 0.99&&superturnstate==false)//氮气值足够，不在漂移，开始冲刺
            {
                if (Vector3.Angle(m_Target.position - transform.position, transform.forward) < 10)//判断是否是直道
                {
                    if (m_forces[3] == Vector3.zero && m_forces[4] == Vector3.zero && m_forces[8] == Vector3.zero && Time.time > m_AvoidOtherCarTime && Time.time > m_AvoidthingsTime)
                        //判断阻碍力的作用，判断撞击时间
                        m_Maxspeed = 3/ 2 * m_CarController.MaxSpeed;
                }
            }
            if (danqi < Time.deltaTime&&m_Maxspeed==3/2*m_CarController.MaxSpeed)
            {
                m_Maxspeed = m_CarController.MaxSpeed;
            }
        }
        */
        //阻挡玩家序号7
        private void hinder()
        {
            if(m_forces[7].magnitude>Time.time)
            {
                m_forces[7] -= m_forces[7].normalized * Time.deltaTime;
            }
            else
                m_forces[7] = Vector3.zero;
            m_speedfactor[7] = 1;
            Collider[] col = Physics.OverlapSphere(transform.position - transform.forward * seedistance, 3);//检测车辆后方5m处是否有玩家角色
            //Debug.DrawRay(transform.position,transform.position - transform.forward * 5,Color.blue);
            foreach (var item in col)
            {
                if (item.transform.root.tag.Equals("Player"))
                {
                    Debug.Log("back player");
                    if (Vector3.Angle(transform.position - item.transform.position, transform.right) > 90)//玩家在AI车右侧
                        m_forces[7] += transform.right * Time.deltaTime*2;
                    else
                        m_forces[7] += -transform.right * Time.deltaTime*2;
                    break;
                }
            }
        }

        //使用道具
        private void usetool()
        {
            for (int i = 0; i < 2; i++)
            {
                if (m_prop.proplist[i] == "Wing")//如果有飞行道具，在飞行状态或加速状态或爬坡下使用
                {
                    if (m_CarController.iffly()||toolStayTime[1]>10||sprintCheck())
                    { 
                        //开始飞行时，停留时间置零
                        toolStayTime[1] = 0;
                        Debug.Log("use Wing");
                        m_prop.UseProp("Wing");
                        m_prop.proplist[i] = "";
                    }
                    else 
                        toolStayTime[1] += Time.deltaTime;//有飞行道具未使用时，开始记录停留时间
                }
                if (m_prop.proplist[i] == "Barry")//如果有障碍物道具，后方有玩家或没有AI车时使用
                {
                    //达到使用条件
                    if(BarryCheck())
                    {
                        //使用障碍物，停留时间置零
                        toolStayTime[2] = 0;
                        Debug.Log("use Barry");
                        m_prop.UseProp("Barry");
                        m_prop.proplist[i] = "";
                    }
                    else
                        toolStayTime[2] += Time.deltaTime;//有障碍物道具未使用时，开始记录停留时间
                }
            }
            sprint();//氮气值满使用氮气
        }

        //检测是否达到障碍物使用条件
        private bool BarryCheck()
        {
            Collider[] col = Physics.OverlapSphere(transform.position - transform.forward * seedistance, 3);//检测车辆后方5m处是否有玩家角色
            foreach (var item in col)
            {
                if (item.transform.root.tag.Equals("Player"))
                {
                    Debug.Log("back player");
                    return true;
                }
            }
            //检测停留时间是否足够
            if (toolStayTime[2] > 10)
                return true;
            return false;
        }

        //AI主动吃道具，看到道具序号5，鉴于球形检测的距离受限，除非正好在车辆前方且不重合才可以到达，不如射线检测直接，但是比射线检测范围大
        private void eattools()
        {
            bool turntool = false;
            Collider toolcol = null ;
            //m_forces[5] = Vector3.zero;/此处不置零，预测函数中置零
            m_speedfactor[5] = 1;
            Collider[] col = Physics.OverlapSphere(transform.position + transform.forward * seedistance, 4);
            //Debug.DrawLine(transform.position, transform.position + transform.forward * 6,Color.green);
            //Gizmos.DrawWireSphere(transform.position + transform.forward * 2,2);
            foreach (var item in col)
            {
                //如果看到道具时没有碰撞处理，那么可以把基本力归零
                //如果道具附近已经有其他车辆，那就不去争了
                if (item.GetComponent<CarAIControl2>() != null)
                {
                    turntool = false;
                    break;
                }
                if (item.tag.Equals("tool") || item.tag.Equals("Nitrogen"))
                {
                    turntool = true;
                    toolcol = item;
                    //Debug.Log(" see tools");
                }
            }
            if (turntool == true&&toolcol!=null) 
                m_forces[5] = toolcol.transform.position - transform.position - baseforce;//若是距离越近越容易靠近，是否考虑分母为0？
        }

        //处理爬坡的飞跃效果，只有非道具模式下才可以有飞跃效果，飞跃使得移动距离增加
        private void fly()
        {
            /*
            Debug.DrawLine(transform.position, transform.position - Vector3.up * 0.2f, Color.white);
            //查看是否离地，可以向地面发射射线检测是否离地，此处检测离地面0.8米的状况，此处射线起点位置需要考虑
            if(!Physics.Raycast(transform.position,-Vector3.up,0.2f))
            {
                //下方没有物体，可以飞跃
                //飞跃方式，失重，可以在车辆飞跃的过程中，冻结绕x 轴的旋转
                Debug.Log("fly");
                m_Rigidbody.AddForce(transform.up*m_Rigidbody.mass*9.8f*6/7);
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
            }
            else
                m_Rigidbody.constraints = ~RigidbodyConstraints.FreezeRotationX;
             * */
            if (m_CarController.iffly())//判断是否处于滞空状态
            {
                //Debug.Log("fly");
                //此处的持续力的作用时间为一帧

                //m_Rigidbody.AddForce(transform.up * m_Rigidbody.mass * 9.8f * 2 / 6, ForceMode.Force);//这里的力或许在持续作用，是否在函数之外仍然有效，此力应该只能有一次
                m_Rigidbody.mass = m_mass / 2;
                //飞跃时对力的作用值不定
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                m_Rigidbody.mass = m_mass;
                m_Rigidbody.constraints = RigidbodyConstraints.None;
            }
        }

        //避免附近的车靠近产生碰撞序号6
        private void avoidNeibour()
        {
            if (m_forces[6].magnitude > Time.time)
            {
                m_forces[6] -= m_forces[6].normalized * Time.deltaTime*0.5f;//力度变化为每秒变化0.5f
            }
            else
                m_forces[6] = Vector3.zero;
            if (m_speedfactor[6] < 1)
            {
                m_speedfactor[6] += Time.deltaTime*0.15f;//注意速度因子变化的速率为每秒增加0.15f
            }
            else m_speedfactor[6] = 1;
            
            Collider[] col = Physics.OverlapSphere(transform.position, seedistance/6);//预测附近1米内是否有AI
            //Gizmos.DrawWireSphere(transform.position, seedistance / 2);
            //Debug.DrawRay(transform.position,transform.position + transform.forward * 5,Color.blue);
            foreach (var item in col)
            {
                if (item.GetComponent<CarAIControl2>() != null && (item.transform.position-transform.position).magnitude>0.8f)//此处注意车辆中部件和车辆本身的区别
                {
                    //Debug.Log("near AI");
                    //注意，此处的力度应该只是向侧方有一定偏移用于抵消朝向目标节点的侧向力，侧向力的大小需要限定在0-1之间
                    m_forces[6] += Mathf.Clamp(Vector3.Dot(transform.position - m_Target.position, transform.right), 0, 1) * transform.right * Time.deltaTime;
                    //根据位置判定是否减速，注意夹角计算
                    if(Vector3.Angle(item.transform.position-transform.position,transform.forward)<90)//对方在前面
                          m_speedfactor[6] = 0.8f;
                    break;//只对第一个部件作用，避免过多的力的处理
                }

            }
        }

        //用于重生时的归零
        public void closeforce()
        {
            for(int i=0;i<+9;i++)
            {
                m_forces[i] = Vector3.zero;
                m_speedfactor[i] = 1;
            }
        }
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, seedistance / 6);
                //Gizmos.DrawWireSphere(circuit.GetRoutePosition(progressDistance), 1);
                //Gizmos.color = Color.yellow;
                //Gizmos.DrawLine(target.position, target.position + target.forward);
            }
        }

    }
}
