using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGolem : Enemy
{
    [SerializeField] private ShortAttack _shortAttackArea;
    [SerializeField] private BoxCollider _attackCollider;

    /// <summary>
    /// 공격콤보 카운트
    /// </summary>
    private int _attackCount;

    protected override void OnEnable()
    {
        base.OnEnable();
        _attackCollider.enabled = false;
        _attackCount = -1;
    }
    
    protected override void Start()
    {
        base.Start();
        _shortAttackArea.SetStat(Stat);
    }
    
    protected override void Attack()
    {
        _isAttack = true;
        _animator.SetInteger(Global.EnemyAttackInteger, ++_attackCount % Global.GolemMaxComboAttack);
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
    
    public override void DisableEnemy()
    {
        if(gameObject.name.Contains("ForestGolem1"))
            ObjectPoolManager.Instance.ReturnObject(PoolType.ForestGolem1,this.gameObject);
        else if(gameObject.name.Contains("ForestGolem2"))
            ObjectPoolManager.Instance.ReturnObject(PoolType.ForestGolem2,this.gameObject);
        else if(gameObject.name.Contains("ForestGolem3"))
            ObjectPoolManager.Instance.ReturnObject(PoolType.ForestGolem3,this.gameObject);
    }
}
