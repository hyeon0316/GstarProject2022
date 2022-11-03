using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

public class Spider : Enemy
{
    [SerializeField] private BoxCollider _attackArea;
    

    protected override void Start()
    {
        base.Start();
        _attackArea.enabled = false;
    }


    protected override void Attack()
    {
        _isAttack = true;
        _animator.SetInteger(Global.EnemyStateInteger, 3);
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
