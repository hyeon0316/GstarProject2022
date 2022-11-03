using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SpawnEnemyDic : SerializableDictionary<PoolType, int>{}

public class EnemySpawnArea : MonoBehaviour
{
    [Header("스폰 될 적 종류와 수")] 
    [SerializeField] private SpawnEnemyDic _spawnEnemyDics;

    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemys();
    }

    /// <summary>
    /// 딕셔너리에 저장된 적들을 생성
    /// </summary>
    private void SpawnEnemys()
    {
        foreach (var dic in _spawnEnemyDics)
        {
            for (int i = 0; i < _spawnEnemyDics[dic.Key]; i++)
            {
                GameObject enemy = ObjectPoolManager.Instance.GetObject(dic.Key);
                enemy.transform.position = transform.position;
            }
        }
    }

    
    /// <summary>
    /// 적들이 스폰지점 안의 범위에서 랜덤위치 생성
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomSpawnPos()
    {
        float width = _boxCollider.size.x;
        float height = _boxCollider.size.z;
        return Vector3.zero;
    }
   
}
