using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

public class Spider : Enemy
{
    [SerializeField] private BoxCollider _attackArea;
    
    protected override void Awake()
    {
        base.Awake();
        Stat = new Stat(EnemyType.Spider);
        _nav = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        _attackArea.enabled = false;
    }


    protected override void Attack()
    {
        StartCoroutine(AttackCo());
    }

    private IEnumerator AttackCo()
    {
        _isAttack = true;
        yield return new WaitForSeconds(1f);
        _animator.SetInteger(Global.EnemyStateInteger, 3);
        _attackArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        _attackArea.enabled = false;
        _isAttack = false;
    }
}
