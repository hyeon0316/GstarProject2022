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


    public void AutoMode()
    {
        StartCoroutine(AutoModeCo());
    }

    /// <summary>
    /// 자동사냥
    /// </summary>
    private IEnumerator AutoModeCo()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius * 4, LayerMask.GetMask("Enemy"));
            if (colliders.Length != 0)
            {
                break;
            }

            if (!_wideAreaBarrageCoolDown.IsCoolDown)
            {
                UseWideAreaBarrage();
            }
            else if (!_bulletRainCoolDown.IsCoolDown)
            {
                UseBulletRain();
            }
            else if (!_chainLightningCoolDown.IsCoolDown)
            {
                UseChainLightning();
            }
            else
            {
                UseNormalAttack();
            }
            yield return null;
        }
    }
    
    
    
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
            if (_targets.Count != 0)
            {
                AttackFromDistance(BulletRain);
            }
            else
            {
                CheckAttackRange(1, BulletRain);
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
            if (_targets.Count >= 2) //todo 타겟을 지정하고 하면 체인라이트닝 안댐
            {
                AttackFromDistance(ChainLightning);
            }
            else //타겟을 모두 처치하여 없거나 원래 없었던 경우 다시 주변 검사하여 타겟을 지정하도록 함
            {
                CheckAttackRange(4, ChainLightning);
            }

        }
    }

    private void ChainLightning()
    {
        if (_targets.Count >= 2) //체인 라이트닝은 최소 2명이상 부터 발동하도록 함
        {
            Debug.Log("체인 라이트닝");
            _chainLightningCoolDown.SetCoolDown();
            transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
            _animator.SetTrigger(Global.ChainLightningTrigger);
        }
    }

    /// <summary>
    /// 광역기 스킬 사용
    /// </summary>
    public void UseWideAreaBarrage()
    {
        if (!_wideAreaBarrageCoolDown.IsCoolDown && !IsDead) 
        {
            if (_targets.Count != 0)
            {
                AttackFromDistance(WideAreaBarrage);
            }
            else
            {
                CheckAttackRange(1, WideAreaBarrage);
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
