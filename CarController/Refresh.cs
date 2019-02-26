using UnityEngine;
using System.Collections;

public class Refresh : MonoBehaviour {

    private PlayerController controller;
    private GameObject player;
    private Transform m_transform;

    void Start ()
    {
		player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        m_transform = player.transform;
    }

    public IEnumerator refresh()
    {
        float startTime = Time.time;
        while (Time.time - startTime < 5)
        {
            yield return new WaitForSeconds(1);
            if (controller.CurrentSpeed < 5)
            {
                Debug.Log("REFRESH");
                player.SetActive(false);
                player.SetActive(true);
                break;
            }
        }
    }
}