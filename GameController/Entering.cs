using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Entering : MonoBehaviour {

    public GameObject pre;
    public GameObject cue;
    Image image;
    Text text;
    bool TurnDark = false;
    float IAlfa = 1;
    float TAlfa = 0;
    float a = 1;
    int time = 0;

    void Start()
    {
        pre.SetActive(false);
        image = GameObject.Find("Shade").GetComponent<Image>();
        text = cue.GetComponent<Text>();
        IAlfa = 1;
        image.color = new Color(0, 0, 0, IAlfa);
    }

	void Update ()
    {
        LogoSpangled();
        if(pre.activeInHierarchy)
        {
            TextSpanled();
            if (Input.anyKeyDown)
                SceneManager.LoadScene("MainMenu");
        }
	}

    void TextSpanled()
    {
        if (TAlfa == 1)
        {
            TurnDark = false;
        }
        if (TAlfa == 0)
        {
            TurnDark = true;
        }
        if (TurnDark == false)
        {
            a -= Time.deltaTime * 0.5f;
            TAlfa = Mathf.Clamp(a, 0, 1);
        }
        if (TurnDark == true)
        {
            a += Time.deltaTime * 0.5f;
            TAlfa = Mathf.Clamp(a, 0, 1);
        }
        text.color = new Color(255, 255, 255, TAlfa);
    }

    void LogoSpangled()
    {
        if (IAlfa == 1)
        {
            TurnDark = false;
            time++;
        }
        if (time == 2)
        {
            Invoke("load", 1.0f);
        }
        if (IAlfa == 0)
        {
            TurnDark = true;
        }
        if (TurnDark == false)
        {
            a -= Time.deltaTime;
            IAlfa = Mathf.Clamp(a, 0, 1);
        }
        if (TurnDark == true)
        {
            Invoke("dark", 2f);
        }
        image.color = new Color(0, 0, 0, IAlfa);
    }

    void dark()
    {
        a += Time.deltaTime;
        IAlfa = Mathf.Clamp(a, 0, 1);
    }
    
    void load()
    {
        pre.SetActive(true);
    }


}
