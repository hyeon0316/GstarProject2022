using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightFly : Enemy
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

    public void CreateMissile()
    {
        var missile = ObjectPoolManager.Instance.GetObject(PoolType.FrightFlyMissile);
        missile.transform.position = _attackPos.position;
        missile.transform.rotation = _attackPos.rotation;
        
        missile.GetComponent<FrightFlyMissile>().DelayDisable();
    }
}
