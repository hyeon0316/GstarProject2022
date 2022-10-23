using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    //todo: 적 종류 추가
}

public abstract class Enemy : Creature
{
    
    // Start is called before the first frame update
    void Start()
    {
        _targets.Add(DataManager.Instance.Player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
