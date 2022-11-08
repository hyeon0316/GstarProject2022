using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMormal : Enemy
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
        _animator.SetInteger(Global.EnemyStateInteger, 2);
    }

    public override void DisableEnemy()
    {
        ObjectPoolManager.Instance.ReturnObject(PoolType.Goblin,this.gameObject);
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
