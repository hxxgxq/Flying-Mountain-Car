using UnityEngine;
using System.Collections;

public class HandleCollision : MonoBehaviour {

    private Rigidbody m_Rigidbody;
    private excursion Excursion;
    private Reborn reborn;
    bool IsDead = false;
    CountDown countDown;
    ShowMessage showMessage;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        Excursion = GetComponent<excursion>();
    }

    void Start()
    {
        reborn = GameObject.FindWithTag("reborn").GetComponent<Reborn>();
        countDown = GameObject.Find("CountDown").GetComponent<CountDown>();
        showMessage = GameObject.Find("ShowMessage").GetComponent<ShowMessage>();
     }


   IEnumerator OnCollisionStay()
    {
        yield return new WaitForSeconds(1);
        if ( showMessage.isPlaying &&  m_Rigidbody.velocity.magnitude <1 && Excursion.Excursion == false || IsDead)
        {
            reborn.reborn(gameObject);
            IsDead = false;
        }
    }

    void OnTriggerEnter(Collider Death)
    {
        if(Death.gameObject.name == "Death")
        {
            IsDead = true;
        }
    }
}