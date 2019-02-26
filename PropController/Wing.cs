using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : MonoBehaviour {

    private Rigidbody m_Rigidbody;
    public float flyMass;
    private float originMass;
    float timelimit = 5;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        originMass = m_Rigidbody.mass;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            m_Rigidbody.useGravity = false;
        }
        else if(m_Rigidbody.useGravity != true)
            m_Rigidbody.useGravity = true;
    }

    public void carFly()
    {
        if(timelimit>0)
        {
            timelimit -= Time.deltaTime;
            m_Rigidbody.useGravity = false;
        }
        else
        {
            m_Rigidbody.useGravity = true;
        }
    }
}
