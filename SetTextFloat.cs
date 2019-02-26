using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetTextFloat : MonoBehaviour {

    Text text;
    private string name;
    void Start()
    {
        text = GetComponent<Text>();
        name = text.name;

    }
    void Update()
    {
        text.text = PlayerPrefs.GetFloat(name).ToString();
    }
}
