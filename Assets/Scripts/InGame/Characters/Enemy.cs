using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    //todo: 적 종류 추가
}

public abstract class Enemy : Creature
{
    protected static Player _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _targets.Add(_player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
