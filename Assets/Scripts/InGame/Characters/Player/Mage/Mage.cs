using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    private enum SkillCoolType
    {
        WideAreaBarrage,
        ChainLightning,
        BulletRain,
        WindAttack,
        SpikeAttack
    }
    
    [Header("스킬 쿨타임 모음")]
    [SerializeField] private CoolDown[] _skiilCoolDown;
    
    [Header("기본공격 발사체 위치")]
    [SerializeField] private Transform _normalAttackPos;

    [Header("불렛레인")]
    [SerializeField] private BulletRain _bulletRain;
    
    [Header("체인라이트닝 라인")] [SerializeField] private ChainLightningLine _chainLightningLine;
    

    
    protected override void Update()
    {
        base.Update();
        if (_isAutoHunt)
        {
            if (!_skiilCoolDown[(int)SkillCoolType.WideAreaBarrage].IsCoolDown && !IsAttack)
            {
                UseWideAreaBarrage();
            }
            else if (!_skiilCoolDown[(int)SkillCoolType.BulletRain].IsCoolDown && !IsAttack)
            {
                UseBulletRain();
            }
            else if (!_skiilCoolDown[(int)SkillCoolType.ChainLightning].IsCoolDown && !IsAttack)
            {
                UseChainLightning();
            }
            else if (_skiilCoolDown[(int)SkillCoolType.WindAttack].IsCoolDown && !IsAttack)
            {
                UseWindAttack();
            }
            else if (_skiilCoolDown[(int) SkillCoolType.SpikeAttack].IsCoolDown && !IsAttack)
            {
                UseSpikeAttack();
            }
            else
            {
                UseNormalAttack();
            }
            
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

    public void UseSpikeAttack()
    {
        if (!_skiilCoolDown[(int) SkillCoolType.SpikeAttack].IsCoolDown && !IsDead)
        {
            CheckAttackRange(1, SpikeAttack);
        }
    }

    private void SpikeAttack()
    {
        IsAttack = true;
        Debug.Log("스파이크 어택");
        _skiilCoolDown[(int)SkillCoolType.SpikeAttack].SetCoolDown();
        transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        _animator.SetTrigger(Global.SpikeAttackTrigger);
    }

    public void CreateSpikeAttack()
    {
        GameObject spike = ObjectPoolManager.Instance.GetObject(PoolType.VolcanicSpike);
        spike.transform.position =
            new Vector3(_targets[0].position.x, _targets[0].transform.position.y + spike.transform.position.y,
                _targets[0].position.z);
        spike.GetComponent<SpikeAttack>().DelayDisable();
    }
    
    /// <summary>
    /// 불렛라인 사용
    /// </summary>
    public void UseBulletRain()
    {
        if (!_skiilCoolDown[(int)SkillCoolType.BulletRain].IsCoolDown && !IsDead)
        {
            CheckAttackRange(1, BulletRain);
        }
    }

    private void BulletRain()
    {
        IsAttack = true;
        Debug.Log("불렛 레인");
        _skiilCoolDown[(int)SkillCoolType.BulletRain].SetCoolDown();
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
        if (!_skiilCoolDown[(int)SkillCoolType.ChainLightning].IsCoolDown && !IsDead)
        {
            CheckAttackRange(4, ChainLightning);
        }
    }

    private void ChainLightning()
    {
        IsAttack = true;
        Debug.Log("체인 라이트닝");
        _skiilCoolDown[(int)SkillCoolType.ChainLightning].SetCoolDown();
        transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        _animator.SetTrigger(Global.ChainLightningTrigger);
    }
    
    public void CreateChainLightningLine()
    {
        _chainLightningLine.CreateLine();
    }

    /// <summary>
    /// 광역기 스킬 사용
    /// </summary>
    public void UseWideAreaBarrage()
    {
        if (!_skiilCoolDown[(int)SkillCoolType.WideAreaBarrage].IsCoolDown && !IsDead) 
        {
            CheckAttackRange(1, WideAreaBarrage);
        }
    }

    public void UseWindAttack()
    {
        if (!_skiilCoolDown[(int)SkillCoolType.WideAreaBarrage].IsCoolDown && !IsDead) 
        {
            CheckAttackRange(1, WindAttack);
        }
    }

    public void CreateWindAttackEffect()
    {
        var windAttack = ObjectPoolManager.Instance.GetObject(PoolType.WindAttackEffect);
        windAttack.transform.position =
            new Vector3(_targets[0].position.x, _targets[0].transform.position.y,
                _targets[0].position.z);
        windAttack.GetComponent<WindAttack>().DelayDisable();
    }

    private void WindAttack()
    {
        Debug.Log("범위공격");
        _skiilCoolDown[(int)SkillCoolType.WindAttack].SetCoolDown();
        transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        _animator.SetTrigger(Global.WindAttackTrigger);
        IsAttack = true;
    }

    private void WideAreaBarrage()
    {
        Debug.Log("범위공격");
        _skiilCoolDown[(int)SkillCoolType.WindAttack].SetCoolDown();
        IsAttack = true;
        transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        _animator.SetTrigger(Global.WideAreaBarrageTrigger);
    }


    public void CreateWideAreaBarrageEffect()
    {
        var effect = ObjectPoolManager.Instance.GetObject(PoolType.WideAreaBarrageEffect);
        effect.transform.position =
            new Vector3(_targets[0].position.x, _targets[0].transform.position.y + effect.transform.position.y,
                _targets[0].position.z);
        effect.GetComponent<WideAreaBarrageEffect>().DelayDisable();

        var barrage = ObjectPoolManager.Instance.GetObject(PoolType.WideAreaBarrage);
        barrage.transform.position = new Vector3(_targets[0].position.x,
            _targets[0].transform.position.y + barrage.transform.position.y,
            _targets[0].position.z);
        barrage.GetComponent<WideAreaBarrage>().DelayDisable();
    }


}
