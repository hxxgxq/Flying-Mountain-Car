using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Return : MonoBehaviour {

    Image image;
    Text text;

    AsyncOperation async_operation;

    void Start()
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
        async_operation = SceneManager.LoadSceneAsync("MainMenu");
        async_operation.allowSceneActivation = false;
        Debug.Log(async_operation.progress);
        yield return async_operation;
    }

    void Update()
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
