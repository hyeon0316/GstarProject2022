using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField]
    private MapContentUI mapContentUI;

    [SerializeField] private EnemySpawnArea[] q1_1;
    [SerializeField] private EnemySpawnArea[] q1_2;
    [SerializeField] private EnemySpawnArea[] q2_1;
    [SerializeField] private EnemySpawnArea[] q2_2;
    // Start is called before the first frame update
    
    public void SetUI1_1()
    {
        mapContentUI.enemyarea = q1_1;
    }
    public void SetUI1_2()
    {
        mapContentUI.enemyarea = q1_2;
    }
    public void SetUI2_1()
    {
        mapContentUI.enemyarea = q2_1;
    }
    public void SetUI2_2()
    {
        mapContentUI.enemyarea = q2_2;
    }
    public void ExitBtn()
    {
        gameObject.SetActive(false);
    }
}
