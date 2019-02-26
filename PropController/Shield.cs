using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public bool shield = false;
    public int LOCK = 0;

    void Update ()
    {
        if (Input.GetKey(KeyCode.H))
        {
            if (LOCK == 0)
            {
                shield = !shield;
                Debug.Log(shield);
                LOCK = 1;
            }
        }
        else
        {
            LOCK = 0 ;
        }
    }


}
