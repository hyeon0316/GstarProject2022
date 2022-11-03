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
    
    protected override void Start()
    {
        base.Start();
        _attackCount = -1;
        _attackArea.enabled = false;
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
}
