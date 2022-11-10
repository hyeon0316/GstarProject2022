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

    private UseActionType[] _useSkills = new UseActionType[5];

    protected override void Start()
    {
        base.Start();
        _useSkills[0] = UseWideAreaBarrage;
        _useSkills[1] = UseChainLightning;
        _useSkills[2] = UseBulletRain;
        _useSkills[3] = UseWindAttack;
        _useSkills[4] = UseSpikeAttack;
    }

    protected override void Update()
    {
        base.Update();
        if (_isAutoHunt && !IsDead)
        {
            if (_isNextPattern)
            {
                SetPrioritySkill();
                if (_autoSkill.Count == 0) //사용할 스킬이 없을때는 일반공격 사용
                {
                    UseNormalAttack();
                }
                else
                {
                    UseAutoSkill(_autoSkill.Dequeue());
                }

                _isNextPattern = false;
            }
        }
    }

    /// <summary>
    /// Queue에서 꺼낸 함수를 사용
    /// </summary>
    private void UseAutoSkill(UseActionType useActionType)
    {
        useActionType();
    }
    

    /// <summary>
    /// 오토모드때 우선적으로 사용할 스킬들을 셋팅
    /// </summary>
    private void SetPrioritySkill()
    {
        for (int i = 0; i < _useSkills.Length; i++)
        {
            if(_skiilCoolDown[i].IsCoolDown) //쿨다운 중인 스킬은 담지 않는다.
                continue;
            
            if(!_autoSkill.Contains(_useSkills[i])) //중복 방지
                _autoSkill.Enqueue(_useSkills[i]);
        }
    }

    /// <summary>
    /// 기본공격할때 지정 위치에 발사체 생성
    /// </summary>
    public void CreateNormalAttackMissile()
    {
        if (_targets.Count != 0)
        {
            var obj = ObjectPoolManager.Instance.GetObject(PoolType.NormalAttackMissile);
            obj.transform.position = _normalAttackPos.position;
            obj.transform.rotation = _normalAttackPos.rotation;

            obj.GetComponent<NormalAttackMissile>().DelayDisable();
        }
    }

    public void UseSpikeAttack()
    {
        if (!_skiilCoolDown[(int) SkillCoolType.SpikeAttack].IsCoolDown && !IsAttack)
        {
            if (_targets.Count != 0)
            {
                ActionFromDistance(SpikeAttack, _targets[0]);
            }
            else
            {
                CheckAttackRange(1, SpikeAttack);
            }
        }
    }

    private void SpikeAttack()
    {
        if (_targets.Count != 0)
        {
            IsAttack = true;
            Debug.Log("스파이크 어택");
            _skiilCoolDown[(int) SkillCoolType.SpikeAttack].SetCoolDown();
            transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        }
        _animator.SetTrigger(Global.SpikeAttackTrigger);
    }

    public void CreateSpikeAttack()
    {
        if (_targets.Count != 0)
        {
            GameObject spike = ObjectPoolManager.Instance.GetObject(PoolType.VolcanicSpike);
            spike.transform.position =
                new Vector3(_targets[0].position.x, _targets[0].transform.position.y + spike.transform.position.y,
                    _targets[0].position.z);
            spike.GetComponent<SpikeAttack>().DelayDisable();
        }
    }
    
    /// <summary>
    /// 불렛라인 사용
    /// </summary>
    public void UseBulletRain()
    {
        if (!_skiilCoolDown[(int)SkillCoolType.BulletRain].IsCoolDown && !IsAttack)
        {
            if (_targets.Count != 0)
            {
                ActionFromDistance(BulletRain, _targets[0]);
            }
            else
            {
                CheckAttackRange(1, BulletRain);
            }
        }
    }

    private void BulletRain()
    {
        if (_targets.Count != 0)
        {
            IsAttack = true;
            Debug.Log("불렛 레인");
            _skiilCoolDown[(int) SkillCoolType.BulletRain].SetCoolDown();
            transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        }
        _animator.SetTrigger(Global.BulletRainTrigger);
    }

    private void CreateBulletRainMissile()
    {
        if(_targets.Count != 0)
            _bulletRain.CreateMissile(_targets[0]);
    }

    /// <summary>
    /// 체인 라이트닝 사용
    /// </summary>
    public void UseChainLightning()
    {
        if (!_skiilCoolDown[(int)SkillCoolType.ChainLightning].IsCoolDown && !IsAttack)
        {
            if (_targets.Count != 0)
            {
                ActionFromDistance(ChainLightning, _targets[0]);
            }
            else
            {
                CheckAttackRange(1, ChainLightning);
            }
        }
    }

    private void ChainLightning()
    {
        if (_targets.Count != 0)
        {
            IsAttack = true;
            Debug.Log("체인 라이트닝");
            _skiilCoolDown[(int) SkillCoolType.ChainLightning].SetCoolDown();
            transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        }
        _animator.SetTrigger(Global.ChainLightningTrigger);
    }
    
    public void CreateChainLightningLine()
    {
        if(_targets.Count != 0)
            _chainLightningLine.CreateLine();
    }

    /// <summary>
    /// 광역기 스킬 사용
    /// </summary>
    public void UseWideAreaBarrage()
    {
        if (!_skiilCoolDown[(int)SkillCoolType.WideAreaBarrage].IsCoolDown && !IsAttack) 
        {
            if (_targets.Count != 0)
            {
                ActionFromDistance(WideAreaBarrage, _targets[0]);
            }
            else
            {
                CheckAttackRange(1, WideAreaBarrage);
            }
        }
    }

    public void UseWindAttack()
    {
        if (!_skiilCoolDown[(int)SkillCoolType.WindAttack].IsCoolDown && !IsAttack) 
        {
            if (_targets.Count != 0)
            {
                ActionFromDistance(WindAttack, _targets[0]);
            }
            else
            {
                CheckAttackRange(1, WindAttack);
            }
        }
    }

    public void CreateWindAttackEffect()
    {
        if (_targets.Count != 0)
        {
            var windAttack = ObjectPoolManager.Instance.GetObject(PoolType.WindAttackEffect);
            windAttack.transform.position =
                new Vector3(_targets[0].position.x, _targets[0].transform.position.y,
                    _targets[0].position.z);
            windAttack.GetComponent<WindAttack>().DelayDisable();
        }
    }

    private void WindAttack()
    {
        if (_targets.Count != 0)
        {
            Debug.Log("바람범위공격");
            IsAttack = true;
            _skiilCoolDown[(int) SkillCoolType.WindAttack].SetCoolDown();
            transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        }
        _animator.SetTrigger(Global.WindAttackTrigger);
    }

    private void WideAreaBarrage()
    {
        if (_targets.Count != 0)
        {
            Debug.Log("범위공격");
            _skiilCoolDown[(int) SkillCoolType.WideAreaBarrage].SetCoolDown();
            IsAttack = true;
            transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        }
        _animator.SetTrigger(Global.WideAreaBarrageTrigger);
    }


    public void CreateWideAreaBarrageEffect()
    {
        if (_targets.Count != 0)
        {
            var effect = ObjectPoolManager.Instance.GetObject(PoolType.WideAreaBarrage);
            effect.transform.position =
                new Vector3(_targets[0].position.x, _targets[0].transform.position.y + effect.transform.position.y,
                    _targets[0].position.z);
            effect.GetComponent<WideAreaBarrageEffect>().DelayDisable();
        }
    }


}
