using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


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
    private void Start()
    {
        SpawnDicsEnemy();
    }
    

    /// <summary>
    /// 딕셔너리에 저장된 적들을 생성
    /// </summary>
    private void SpawnDicsEnemy()
    {
        foreach (var dic in _spawnEnemyDics)
        {
            for (int i = 0; i < _spawnEnemyDics[dic.Key]; i++)
            {
                GameObject enemy = ObjectPoolManager.Instance.GetObject(dic.Key);
                enemy.GetComponent<Enemy>().SpawnArea = _boxCollider;
                enemy.transform.position = RandomSpawnPos();
            }
        }
    }

    /// <summary>
    /// 딕셔너리에 있는 적 종류 중 랜덤한 적을 스폰
    /// </summary>
    public void SpawnRandomEnemy()
    {
        int index = Random.Range(0, _spawnEnemyDics.Count);
        KeyValuePair<PoolType, int> randomDic = _spawnEnemyDics.ElementAt(index);
        GameObject enemy = ObjectPoolManager.Instance.GetObject(randomDic.Key);
        enemy.GetComponent<Enemy>().SpawnArea = _boxCollider;
        enemy.GetComponent<Enemy>().ShowAppearance();
        enemy.transform.position = RandomSpawnPos();
    }
    
    
    /// <summary>
    /// 적들이 스폰지점 안의 범위에서 랜덤위치 생성
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomSpawnPos()
    {
        Vector3 originPos = transform.position;
        
        float width = _boxCollider.bounds.size.x;
        float height = _boxCollider.bounds.size.z;

        float randomX = Random.Range((width / 2) * -1, width / 2);
        float randomZ = Random.Range((height / 2) * -1, height / 2);
        Vector3 randomPos = new Vector3(randomX, 0, randomZ);

        Vector3 spawnPos = originPos + randomPos;
        return spawnPos;
    }
   
}
