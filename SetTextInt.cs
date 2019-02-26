using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SetTextInt : MonoBehaviour {
    Text text;
    private string name;
    
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        name = text.name;
       // PlayerPrefs.SetInt(name, 10000);
	}
	
	// Update is called once per frame
	void Update () {
        text.text = PlayerPrefs.GetInt(name).ToString() ;
	}
}
