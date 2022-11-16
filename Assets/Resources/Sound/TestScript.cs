using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "InGame")
        {
            Debug.Log("destory");
            Destroy(this.gameObject);
        }
    }
}
