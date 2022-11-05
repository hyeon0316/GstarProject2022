using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGolem : Enemy
{
    [SerializeField] private BoxCollider _attackArea;

    /// <summary>
    /// 공격콤보 카운트
    /// </summary>
    private int _attackCount;

    protected override void OnEnable()
    {
        base.OnEnable();
        _attackArea.enabled = false;
        _attackCount = -1;
    }
    
    
    protected override void Attack()
    {
        _isAttack = true;
        _animator.SetInteger(Global.EnemyAttackInteger, ++_attackCount % Global.GolemMaxComboAttack);
        Debug.Log(_attackCount);
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
