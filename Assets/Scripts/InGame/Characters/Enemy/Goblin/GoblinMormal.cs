using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMormal : Enemy
{
    [SerializeField] private ShortAttack _shortAttackArea;
    [SerializeField] private BoxCollider _attackCollider;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _attackCollider.enabled = false;
    }

    protected override void Start()
    {
        base.Start();
        _shortAttackArea.SetStat(Stat);
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
        _attackCollider.enabled = true;
    }
    
    public void InActiveAttackCollider()
    {
        _attackCollider.enabled = false;
        _isAttack = false;
    }
}
