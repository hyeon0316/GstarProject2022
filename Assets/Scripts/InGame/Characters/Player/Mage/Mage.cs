using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    [Header("기본공격 발사체 위치")]
    [SerializeField] private Transform _normalAttackPos;

    [Header("불렛레인")]
    [SerializeField] private BulletRain _bulletRain;
    
    [Header("범위공격 쿨타임")]
    [SerializeField] private CoolDown _wideAreaBarrageCoolDown;

    [Header("체인라이트닝 라인")] [SerializeField] private ChainLightningLine _chainLightningLine;

    [Header("체인라이트닝 쿨타임")]
    [SerializeField] private CoolDown _chainLightningCoolDown;

    [Header("불렛레인 쿨타임")] 
    [SerializeField] private CoolDown _bulletRainCoolDown;
    
    
    
    /// <summary>
    /// 기본공격할때 지정 위치에 발사체 생성
    /// </summary>
    public void CreateNormalAttackMissile()
    {
        var obj = ObjectPoolManager.Instance.GetObject(PoolType.NormalAttackMissile);
        obj.transform.position = _normalAttackPos.position;
        obj.transform.rotation = _normalAttackPos.rotation;
        
        obj.GetComponent<NormalAttackMissile>().DelayDisable();
    }

    /// <summary>
    /// 카이사의 이케시아 폭우 사용
    /// </summary>
    public void UseBulletRain()
    {
        if (!_bulletRainCoolDown.IsCoolDown && !IsDead)
        {
            if (IsAttackRange(1))
            {
                if (_attackRadius < Vector3.Distance(transform.position, _targets[0].position))
                {
                    _moveCo = MoveTowardEnemyCo(BulletRain);
                    StartCoroutine(_moveCo);
                }
                else
                {
                    BulletRain();
                }
            }
        }
    }

    private void BulletRain()
    {
        Debug.Log("불렛 레인");
        _bulletRainCoolDown.SetCoolDown();
        transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        _animator.SetTrigger(Global.BulletRainTrigger);
        Invoke("CreateBulletRainMissile",0.5f);
    }

    private void CreateBulletRainMissile()
    {
        _bulletRain.CreateMissile(_targets[0]);
    }

    /// <summary>
    /// 체인 라이트닝 사용
    /// </summary>
    public void UseChainLightning()
    {
        if (!_chainLightningCoolDown.IsCoolDown && !IsDead)
        {
            if (IsAttackRange(4))
            {
                if (_targets.Count >= 2) //체인 라이트닝은 최소 2명이상 부터 발동하도록 함
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
        if (!_wideAreaBarrageCoolDown.IsCoolDown && !IsDead) 
        {
            if (IsAttackRange(1))
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

    public void CreateChainLightningLine()
    {
        _chainLightningLine._targets = _targets;
        _chainLightningLine.CreateLine();
    }
}
