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
        mapContentUI.SpawnTransform = MapManager.Instance.GetSpwan(1);
        mapContentUI.Name.text = "°ñ·½ ¼­½ÄÁö1";
        mapContentUI.SetContentUI();
    }
    public void SetUI1_2()
    {
        mapContentUI.enemyarea = q1_2;
        mapContentUI.SpawnTransform = MapManager.Instance.GetSpwan(2);
        mapContentUI.Name.text = "°ñ·½ ¼­½ÄÁö2";
        mapContentUI.SetContentUI();
    }
    public void SetUI2_1()
    {
        mapContentUI.enemyarea = q2_1;
        mapContentUI.SpawnTransform = MapManager.Instance.GetSpwan(3);
        mapContentUI.Name.text = "°íºí¸° ¼­½ÄÁö";
        mapContentUI.SetContentUI();
    }
    public void SetUI2_2()
    {
        mapContentUI.enemyarea = q2_2;
        mapContentUI.SpawnTransform = MapManager.Instance.GetSpwan(4);
        mapContentUI.Name.text = "°ñ·½ ¹«´ý";
        mapContentUI.SetContentUI();
    }
    public void ExitBtn()
    {
        gameObject.SetActive(false);
    }
}
