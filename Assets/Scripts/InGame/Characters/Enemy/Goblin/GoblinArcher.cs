using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinArcher : Enemy
{
    [SerializeField] private Transform _attackPos;
    
    protected override void Attack()
    {
        StartCoroutine(AttackCo());
    }

    private IEnumerator AttackCo()
    {
        _isAttack = true;
        yield return new WaitForSeconds(1f);
        _animator.SetInteger(Global.EnemyStateInteger,2);
        yield return new WaitForSeconds(0.5f);
        _isAttack = false;
    }
    
    public void CreateArrow()
    {
        var arrow = ObjectPoolManager.Instance.GetObject(PoolType.GoblinArcherArrow);
        arrow.transform.position = _attackPos.position;
        arrow.transform.rotation = _attackPos.rotation;
        
        arrow.GetComponent<GoblinArcherArrow>().DelayDisable();
    }

    public override void DisableEnemy()
    {
        ObjectPoolManager.Instance.ReturnObject(PoolType.GoblinArcher,this.gameObject);
    }
}
