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
    private EnemySpawnArea spawnArea;
    private Transform _tr;
    public GameObject MapObj;
    
    private int dunIndex;

    private void Start()
    {
        spawnArea = MapManager.Instance.DunArea;
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
        spawnArea.Difficulty = dunIndex;
    }
    public void SpawnC()
    {
        SoundManager.Instance.BgmPlay(1);
        DataManager.Instance.Player.UseTeleport(_tr);
        MapObj.SetActive(false);
    }
    public void BtnExit()
    {
        MapObj.SetActive(false);
    }
}
