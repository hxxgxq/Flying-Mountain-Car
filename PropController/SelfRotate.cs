using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour {

    public float rotateSpeed = 30;

    void Update ()
    {
        gameObject.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
	}
}
