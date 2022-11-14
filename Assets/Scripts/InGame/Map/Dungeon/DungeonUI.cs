using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Gold1;
    [SerializeField] private TextMeshProUGUI Gold2;
    [SerializeField] private TextMeshProUGUI Gold3;
    [SerializeField] private EnemySpawnArea spawnArea;
    private Transform _tr;
    public GameObject MapObj;
    
    private int dunIndex;

    private void Start()
    {
        _tr = spawnArea.gameObject.transform;
    }

    public void BtnEasy()
    {
        SpawnC();
        dunIndex = 0;
        Invoke("SetSpwan", 2f);

    }
    public void BtnNormal()
    {
        SpawnC();
        dunIndex = 1;
        Invoke("SetSpwan", 2f);
    }
    public void BtnHard()
    {
        SpawnC();
        dunIndex = 2;
        Invoke("SetSpwan", 2f);
    }
    public void SetSpwan()
    {
        spawnArea.Init();
        spawnArea.Difficulty = dunIndex;
        spawnArea.SpawnDungeonEnemy();
       
    }
    public void SpawnC()
    {
       DataManager.Instance.Player.UseTeleport(_tr);
        MapObj.SetActive(false);
    }
}
