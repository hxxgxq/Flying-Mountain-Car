using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CarUserControl : MonoBehaviour {

    public float m_AccelSensitivity = 0.04f;                                // How sensitively the AI uses the accelerator to reach the current desired speed
    [SerializeField]
    private float m_BrakeSensitivity = 1f;                                   // How sensitively the AI uses the brake to reach the current desired speed
    private PlayerController m_CarController;    // Reference to actual car controller we are controlling
    [HideInInspector]
    public bool excursion = false;
    private bool Lock = true;
    public bool useN2 = true;
    private float currentTime;
    private float desiredSpeed;
    public bool brake = false;
    private excursion Excursion;

    private void Awake()
    {
        Excursion = GetComponent<excursion>();
        m_CarController = GetComponent<PlayerController>();
    }
    
    void Start()
    {
        desiredSpeed = m_CarController.MaxSpeed;
    }

    private void FixedUpdate()
    {        
        float acceleration = (desiredSpeed < m_CarController.CurrentSpeed)
                                                    ? m_BrakeSensitivity
                                                    : m_AccelSensitivity;
        
        float accel = Mathf.Clamp((desiredSpeed - m_CarController.CurrentSpeed) * acceleration, -1, 1);
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        if (brake == false)
        {
            if (Excursion.Excursion == false)
                m_CarController.Move(h, accel, accel, 0);
            else
                m_CarController.Move(h, 0, 0, 1);
        }
        else
        {
            m_CarController.Move(0, 0, 0, 1000);
        }
    }

    public void speedup()
    {
        m_AccelSensitivity = desiredSpeed = float.MaxValue;
        currentTime = Time.time;
    }

    public void speeddown()
    {
        m_AccelSensitivity = 0.04f;
        desiredSpeed = m_CarController.MaxSpeed;
    }


}