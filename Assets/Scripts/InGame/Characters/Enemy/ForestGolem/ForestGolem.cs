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
    
    protected override void Awake()
    {
        base.Awake();
        Stat = new Stat(EnemyType.ForestGolem);
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
        _attackArea.enabled = true;
    }
}
