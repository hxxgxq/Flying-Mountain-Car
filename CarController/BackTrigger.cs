using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class BackTrigger : MonoBehaviour {

    private CarUserControl User_Controller;

	void Awake()
    {
        User_Controller = GetComponent<CarUserControl>();
    }

    void OnTriggerStay()
    {
        //User_Controller.isback = true;
    }
    void OnTriggerExit()
    {
        //User_Controller.isback = false;
    }
}
