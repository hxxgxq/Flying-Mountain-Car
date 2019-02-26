using Scripts.Ai;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

    Text Count;
    GameObject player;
    float time = 6f;
    GameObject[] Aicars;
    bool isCountDown = false;
	void Start ()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<CarUserControl>().brake = true;
        Aicars = GameObject.FindGameObjectsWithTag("AI");
        Count = gameObject.GetComponent<Text>();
        Count.text = "Ready";
        Count.fontSize = 250;
    }
	
	void Update ()
    {
        time -= Time.deltaTime;        
        if (time>1 && time<4)
        {
            Count.text = ((int)time).ToString();
            Count.fontSize = 250;
        }

        if (time >= 0 && time < 1)  
        {
            Count.text = "Go";
            Count.fontSize = 250;
        }
        if(time<0.5f)
        {
            foreach (var item in Aicars)
            {
                item.GetComponent<CarAIControl2>().m_Driving = true;
            }
            Count.gameObject.SetActive(false);
            GameObject.Find("ShowMessage").GetComponent<ShowMessage>().isPlaying = true;
            player.GetComponent<CarUserControl>().brake = false;
            if(PlayerPrefs.GetInt("CurrentScene", 0) == 1|| PlayerPrefs.GetInt("CurrentScene", 0) == 3)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -2.5f);
            }
            else if(PlayerPrefs.GetInt("CurrentScene", 0) == 2 || PlayerPrefs.GetInt("CurrentScene", 0) == 4)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 2.5f);
            }
            isCountDown = true;
        }
	}
    public bool ReturnisCountDown()
    {
        return isCountDown;
    }

}
