using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class Spider : Enemy
{
    [SerializeField] private BoxCollider _attackArea;


    protected override void OnEnable()
    {
        base.OnEnable();
        _attackArea.enabled = false;
    }

    protected override void Attack()
    {
        _isAttack = true;
        _animator.SetInteger(Global.EnemyStateInteger, 3);
    }

    public override void DisableEnemy()
    {
        ObjectPoolManager.Instance.ReturnObject(PoolType.Spider,this.gameObject);
    }


    public void ActiveAttackCollider()
    {
        _attackArea.enabled = true;
    }
    
    public void InActiveAttackCollider()
    {
        _attackArea.enabled = false;
        _isAttack = false;
    }
}
