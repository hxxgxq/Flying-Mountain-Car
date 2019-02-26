using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class excursion : MonoBehaviour {

    private Rigidbody m_Rigidbody;
    private bool _excursion = false;
    private int excursionLimit;
    private Transform m_transform;
    private Refresh refresh;
    private float path = 0;
    private Vector3 ExcursionPoint;
    public float AntiGravity = 6000;
    CarAudioControl carAudioControl;
    private bool isPlayedEX = false;
    private Vector3 LastPos;

    public bool Excursion { get { return _excursion; } }

    void Awake ()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        excursionLimit = (int)(90 / (Time.deltaTime * 70));
        m_transform = transform;
        ExcursionPoint = m_transform.position + m_transform.forward*3.3f;
    }

    void Start()
    {
        refresh = GameObject.FindGameObjectWithTag("Camera").GetComponent<Refresh>();
        carAudioControl = GetComponent<CarAudioControl>();
    }

    void FixedUpdate ()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && h != 0 && GameObject.Find("ShowMessage").GetComponent<ShowMessage>().isPlaying)
        {
            if (_excursion == false)//刚按下E时(只在按下E的第一瞬间有作用)
                _excursion = true;
            if (excursionLimit == (int)(90 / (Time.deltaTime * 70)))
                LastPos = m_transform.position;
            if (--excursionLimit > 0)
            {
                m_Rigidbody.AddForce(m_transform.up * AntiGravity);
                ExcursionPoint = m_transform.position + m_transform.forward * 2.6f;
                m_transform.RotateAround(ExcursionPoint, Vector3.up, 70 * h / Mathf.Abs(h) * Time.deltaTime);
                //m_transform.position += m_transform.forward * m_Rigidbody.velocity.magnitude *
                                                                        //((int)(90 / (Time.deltaTime * 70)) - excursionLimit) / 1700;

                path += (m_transform.position - LastPos).magnitude;
                LastPos = m_transform.position;
            }

            if (!isPlayedEX)
            {
                carAudioControl.Play("漂移");
                isPlayedEX = true;
            }

        }
        else if (_excursion)
        {
            _excursion = false;
            StartCoroutine(refresh.refresh());
            excursionLimit = (int)(90 / (Time.deltaTime * 70));
            isPlayedEX = false;
            carAudioControl.Play("move1");
        }

    }
    public float ReturnPath()
    {
        return path;
    }
    
}