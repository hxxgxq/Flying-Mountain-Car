using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private WheelCollider[] m_WheelColliders = new WheelCollider[4];//碰撞器数组
    [SerializeField]
    private GameObject[] m_WheelMeshes = new GameObject[4];
    [SerializeField]
    private Vector3 m_CentreOfMassOffset;
    [Range(0, 1)]
    private float m_SteerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
    [Range(0, 1)]
    private float m_TractionControl; // 0 is no traction control, 1 is full interference
    [Range(0, 1)]
    public float steerSensitivity;

    private float m_ReverseTorque;
    private float m_MaxHandbrakeTorque;
    private float m_Downforce = 100f;
    private float m_TopSpeedOrigin;
    private static int NoOfGears = 5;
    private float m_RevRangeBoundary = 1f;
    private float m_SlipLimit;
    private float m_BrakeTorque;
    private Quaternion[] m_WheelMeshLocalRotations;
    private excursion Excursion;
    private Vector3 m_Prevpos, m_Pos;
    private float m_SteerAngle;
    private int m_GearNum;
    private float m_GearFactor;
    private float m_OldRotation;
    private float m_CurrentTorque;
    private const float k_ReversingThreshold = 0.01f;
    private Rigidbody m_Rigidbody;
    private float FlyTime;
    public float N2Speed;

    public float m_FullTorqueOverAllWheels;
    public float m_Topspeed = 20;
    public float m_MaximumSteerAngle;

    public bool Skidding { get; private set; }
    public float BrakeInput { get; private set; }
    public float CurrentSteerAngle { get { return m_SteerAngle; } }
    public float CurrentSpeed { get { return m_Rigidbody.velocity.magnitude * 2.23693629f; } }
    public float MaxSpeed { get { return m_Topspeed; } }
    public float TopSpeedOrigin { get { return m_TopSpeedOrigin; } }
    public float Revs { get; private set; }
    public float AccelInput { get; private set; }

    void Start()
    {
        m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;
        m_MaxHandbrakeTorque = float.MaxValue;
        m_Rigidbody = GetComponent<Rigidbody>();
        Excursion = GetComponent<excursion>();
        m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);
        m_TopSpeedOrigin = m_Topspeed;

    }


    private void GearChanging()
    {
        float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
        float upgearlimit = (1 / (float)NoOfGears) * (m_GearNum + 1);
        float downgearlimit = (1 / (float)NoOfGears) * m_GearNum;

        if (m_GearNum > 0 && f < downgearlimit)
        {
            m_GearNum--;
        }

        if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
        {
            m_GearNum++;
        }
    }


    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }


    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }


    private void CalculateGearFactor()
    {
        float f = (1 / (float)NoOfGears);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
        m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_GearNum / (float)NoOfGears;
        var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
    }


    public void Move(float steering, float accel, float footbrake, float handbrake)
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 position;
            m_WheelColliders[i].GetWorldPose(out position, out quat);
            m_WheelMeshes[i].transform.position = position;
            m_WheelMeshes[i].transform.rotation = quat;
        }

        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        // handbrake = Mathf.Clamp(handbrake, 0, 1);

        //Set the steer on the front wheels.
        //Assuming that wheels 0 and 1 are the front wheels.
        m_SteerAngle = steering * m_MaximumSteerAngle * steerSensitivity;
        m_WheelColliders[0].steerAngle = m_SteerAngle;
        m_WheelColliders[1].steerAngle = m_SteerAngle;

        SteerHelper();
        ApplyDrive(accel, footbrake);
        // if(!Input.GetKey(KeyCode.Q))
        CapSpeed();   //和Car user control脚本中accel=maths.clamp作用重复
                      // 2018/7/4 后来发现作用并不重复 这里的CapSpeed()通过直接给速度赋值的方式来限制速度
                      //Car User Control 是通过加减速来限制速度
                      //Set the handbrake.
                      //Assuming that wheels 2 and 3 are the rear wheels.
        if (handbrake > 0f)
        {
            var hbTorque = handbrake * m_MaxHandbrakeTorque;
            m_WheelColliders[2].brakeTorque = hbTorque;
            m_WheelColliders[3].brakeTorque = hbTorque;
        }


        CalculateRevs();
        GearChanging();

        //AddDownForce();
        // CheckForWheelSpin();
        TractionControl();
        // Debug.Log(m_CurrentTorque);
        //Debug.Log(Mathf.Abs(m_WheelColliders[1].rpm - m_WheelColliders[0].rpm));
    }


    private void CapSpeed()
    {
        float speed = m_Rigidbody.velocity.magnitude * 2.23693629f;
        if (speed > N2Speed)
            m_Rigidbody.velocity = (N2Speed / 2.23693629f) * m_Rigidbody.velocity.normalized;

    }


    private void ApplyDrive(float accel, float footbrake)
    {

        float thrustTorque = accel * (m_CurrentTorque / 4f);
        for (int i = 0; i < 4; i++)
        {
            m_WheelColliders[i].motorTorque = thrustTorque;
        }

        for (int i = 0; i < 4; i++)
        {
            if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 50f)
            {
                m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
            }
            else if (footbrake > 0)
            {
                m_WheelColliders[i].brakeTorque = 0f;
                m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
            }
        }
    }


    private void SteerHelper()
    {
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelhit;
            m_WheelColliders[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return; // wheels arent on the ground so dont realign the rigidbody velocity
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
        {
            var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
        }
        m_OldRotation = transform.eulerAngles.y;
    }


    // this is used to add more grip in relation to speed
    private void AddDownForce()
    {
        m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up * m_Downforce *
                                                     m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
    }

    private void TractionControl()
    {
        WheelHit wheelHit;
        for (int i = 0; i < 4; i++)
        {
            m_WheelColliders[i].GetGroundHit(out wheelHit);
            AdjustTorque(wheelHit.forwardSlip);
        }
    }

    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
        {
            m_CurrentTorque -= 10 * m_TractionControl;
        }
        else
        {
            m_CurrentTorque += 10 * m_TractionControl;
            if (m_CurrentTorque > m_FullTorqueOverAllWheels)
            {
                m_CurrentTorque = m_FullTorqueOverAllWheels;
            }
        }
    }

    private void Update()
    {
        if (Excursion.Excursion == false)
            fly();
    }

    private void fly()
    {

        if (this.isFly())//判断是否处于滞空状态
        {
            m_Rigidbody.AddForce(transform.up * m_Rigidbody.mass * 9.8f * 1 / 6, ForceMode.Force);//这里的力或许在持续作用，是否在函数之外仍然有效，此力应该只能有一次
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            FlyTime += Time.deltaTime;
        }
        else
        {
            m_Rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    private bool isFly()
    {
        WheelHit wheelHit;
        for (int i = 0; i < 4; i++)
        {
            //判断是否与地面发生碰撞
            if (m_WheelColliders[i].GetGroundHit(out wheelHit))
            {
                //只要有一个轮胎与地面碰撞，都不算是滞空
                //Debug.Log("ground");
                return false;
            }
        }
        return true;
    }

    public float ReturnFlyTime()
    {
        return FlyTime;
    }


}
