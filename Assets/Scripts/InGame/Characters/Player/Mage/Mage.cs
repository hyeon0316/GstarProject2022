using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    [Header("기본공격 발사체 위치")]
    [SerializeField] private Transform _nomalAttackPos;

    [Header("범위공격 쿨타임")]
    [SerializeField] private CoolDown _wideAreaBarrageCoolDown;
    
    [Header("체인라이트닝 쿨타임")]
    [SerializeField] private CoolDown _chainLightningCoolDown;
    
    /// <summary>
    /// 기본공격할때 지정 위치에 발사체 생성
    /// </summary>
    public void CreateNomalAttackMissile()
    {
        var obj = ObjectPoolManager.Instance.GetObject(PoolType.NomalAttackMissile);
        obj.transform.position = _nomalAttackPos.position;
        obj.transform.rotation = _nomalAttackPos.rotation;
        
        obj.GetComponent<NomalAttackMissile>().DelayDisable();
    }

    public void UseChainLightning()
    {
        if (!_chainLightningCoolDown.IsCoolDown)
        {
            if (CheckAttackRange(2))
            {
                if (_attackRadius < Vector3.Distance(transform.position, _targets[0].position))
                {
                    _moveCo = MoveTowardEnemyCo(ChainLightning);
                    StartCoroutine(_moveCo);
                }
                else
                {
                    ChainLightning();
                }
            }
        }
    }

    private void ChainLightning()
    {
        Debug.Log("체인 라이트닝");
        _chainLightningCoolDown.SetCoolDown();
        transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        _animator.SetTrigger(Global.ChainLightningTrigger);
    }

    /// <summary>
    /// 광역기 스킬 사용
    /// </summary>
    public void UseWideAreaBarrage()
    {
        if (!_wideAreaBarrageCoolDown.IsCoolDown) 
        {
            if (CheckAttackRange(1))
            {
                if (_attackRadius < Vector3.Distance(transform.position, _targets[0].position))
                {
                    _moveCo = MoveTowardEnemyCo(WideAreaBarrage);
                    StartCoroutine(_moveCo);
                }
                else
                {
                    WideAreaBarrage();
                }
            }
        }
    }

    private void WideAreaBarrage()
    {
        Debug.Log("범위공격");
        _wideAreaBarrageCoolDown.SetCoolDown();
        IsAttack = true;
        transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        _animator.SetTrigger(Global.WideAreaBarrageTrigger);
    }

    public void DelayMove()
    {
        IsAttack = false;
    }
    
    public void CreateWideAreaBarrageEffect()
    {
        var effect = ObjectPoolManager.Instance.GetObject(PoolType.WideAreaBarrageEffect);
        effect.transform.position = new Vector3(_targets[0].position.x, effect.transform.position.y, _targets[0].position.z);
        effect.GetComponent<WideAreaBarrageEffect>().DelayDisable();
        
        var barrage = ObjectPoolManager.Instance.GetObject(PoolType.WideAreaBarrage);
        barrage.transform.position = new Vector3(_targets[0].position.x, barrage.transform.position.y, _targets[0].position.z);
        barrage.GetComponent<WideAreaBarrage>().DelayDisable();
    }
    
}
