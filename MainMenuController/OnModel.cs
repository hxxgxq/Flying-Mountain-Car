using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OnModel : MonoBehaviour {

    private Vector3 BeginPlace;
    private Vector3[] EndPlace = new Vector3[4];
    public GameObject[] model;
    public GameObject[] end;
    int j = 0;
    bool IsCome = false;
    private AudioSource audioSource;
    private AudioSource backaudioSource;
    void Start()
    {
        BeginPlace = GameObject.Find("Begin").transform.position;
        audioSource = GameObject.Find("SelectModel").GetComponent<AudioSource>();
        backaudioSource = GameObject.Find("BackButtonClip").GetComponent<AudioSource>();
        for (int i = 0; i < 4; i++)
        {
            model[i].transform.position = BeginPlace;
            EndPlace[i] = end[i].transform.position;
        }
    }

    void Update()
    {
        if ((model[j].transform.position - EndPlace[j]).sqrMagnitude > 0.5)
        {
            model[j].transform.position = Vector3.Lerp(BeginPlace, EndPlace[j], 13f * Time.deltaTime);
            BeginPlace = model[j].transform.position;           
        }
        else
        {
            IsCome = true;
        }

        if (IsCome)
        {
            if (j < 3) 
                j++;
            BeginPlace = GameObject.Find("Begin").transform.position;
            IsCome = false;
        }
    }

    public void BackClick()
    {
        SceneMgr.Instance.SwitchScence("HomeDlg");
        backaudioSource.Play();
    }

    public void StoryClick()
    {
        SceneMgr.Instance.SwitchScence("StoryLevelDlg");
        audioSource.Play();
    }

    public void RaceClick()
    {
        SceneMgr.Instance.SwitchScence("RaceLevelDlg");
        audioSource.Play();
    }

    public void PropClick()
    {
        SceneMgr.Instance.SwitchScence("PropLevelDlg");
        audioSource.Play();
    }
    public void QuickClick()
    {
        int i = Random.Range(2, 5);
        //SceneManager.LoadScene(i);
        SceneMgr.Instance.SwitchScence("Loading");
        PlayerPrefs.SetInt("CurrentScene", i - 1);
        PlayerPrefs.SetInt("IsQuickGame", 1);
        PlayerPrefs.SetInt("CurrentCarID", Random.Range(1, 12));
        audioSource.Play();
    }

}
