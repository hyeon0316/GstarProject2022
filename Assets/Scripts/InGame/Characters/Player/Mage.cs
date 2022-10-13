using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    [Header("기본공격 발사체 위치")]
    [SerializeField] private Transform _nomalAttackPos;

    /// <summary>
    /// 기본공격할때 지정 위치에 발사체 생성하여 발사
    /// </summary>
    public void LaunchNomalAttack()
    {
        var obj = ObjectPoolManager.Instance.GetObject(PoolType.Bullet);
        obj.transform.position = _nomalAttackPos.position;
        obj.transform.rotation = _nomalAttackPos.rotation;
    }

}
