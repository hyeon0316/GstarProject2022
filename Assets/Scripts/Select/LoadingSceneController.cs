using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private string _sceneName;
    private float _time;


    public void LoadAsyncScene()
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadAsyncSceneCo());
    }
    
    private IEnumerator LoadAsyncSceneCo()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)//씬 호출되기 전 까지 반복
        {
            _time += Time.deltaTime;

            if (_time >= Global.LoadingTime)
                operation.allowSceneActivation = true;

            yield return null;
        }

    }
}
