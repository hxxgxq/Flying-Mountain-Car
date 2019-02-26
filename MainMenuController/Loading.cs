using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

    Image image;
    Text text;

    AsyncOperation async_operation;

    void Start ()
    {
        Time.timeScale = 1;
        image = gameObject.GetComponent<Image>();
        text = GameObject.Find("Rate").GetComponent<Text>();
        image.fillAmount = 0;
        StartCoroutine(LoadScene());
    }
	
    //异步加载场景
    IEnumerator LoadScene()
    {
        if (PlayerPrefs.GetInt("CurrentScene", 0) == 1)//如果是竞速第一关
        {
            async_operation = SceneManager.LoadSceneAsync("RunLevel1");

        }
        else if (PlayerPrefs.GetInt("CurrentScene", 0) == 2)//竞速第二关
        {
            async_operation = SceneManager.LoadSceneAsync("RunLevel2");

        }
        else if (PlayerPrefs.GetInt("CurrentScene", 0) == 3)//道具第一关
        {
            async_operation = SceneManager.LoadSceneAsync("PropLevel1");

        }
        else if (PlayerPrefs.GetInt("CurrentScene", 0) == 4)//道具第二关
        {
            async_operation = SceneManager.LoadSceneAsync("PropLevel2");
        }
        async_operation.allowSceneActivation = false;
        yield return async_operation;
    }

	void Update ()
    {
        //if (async_operation.progress < 0.8)
        //{
        //    image.fillAmount += async_operation.progress;
        //    text.text = (async_operation.progress * 100).ToString();
        //}
        //else
        //{
        //    image.fillAmount = 1;
        //    text.text = "100";
        //    Invoke("Jump", 2.0f);
        //}
        image.fillAmount += Time.deltaTime * 0.4f;
        text.text = (image.fillAmount * 100).ToString();
        if (image.fillAmount == 1)
        {
            Invoke("Jump", 0.5f);
        }

    }

    void Jump()
    {
        async_operation.allowSceneActivation = true;
    }


}
