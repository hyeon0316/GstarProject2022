using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어와 적이 공통적으로 가지고 있는 스탯 변수
/// </summary>
public class Stat
{
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int RecoveryHp { get; set; } //자연 Hp회복량
    public int RecoveryMp { get; set; } //자연 Mp회복량
    public int Defense { get; set; }
    public int Dodge { get; set; }
    public int HitPercent { get; set; }
    public int ReduceDamage { get; set; } // 받는 모든 데미지 감소량
    public int MoveSpeed { get; set; }
    public int Attack { get; set; }
    public int SkillDamage { get; set; } //스킬데미지
    public int AllDamge { get; set; }   //모든데미지
    public int MaxMp { get; set; }
    public int Mp { get; set; }
    public int MaxPostion { get; set; }


    public Stat()
    {
        
    }
    
    /// <summary>
    /// 플레이어 스탯
    /// </summary>
    public Stat(JobType jobType)
    {
        switch (jobType)
        {
            case JobType.Knight:
                SetPlayerStat(Resources.Load<PlayerStatData>("Stats/Player/KnightStat"));
                break;
            case JobType.Mage:
                SetPlayerStat(Resources.Load<PlayerStatData>("Stats/Player/MageStat"));
                break;
        }
    }

    /// <summary>
    /// 적 스탯
    /// </summary>
    public Stat(EnemyType enemyType)
    {
        //todo: ScriptableObject로 관리하여 로드
        switch (enemyType)
        {
            case EnemyType.Spider:
                Hp = 100;
                break;
            case EnemyType.FrightFly:
                Hp = 100;
                break;
            case EnemyType.ForestGolem:
                Hp = 100;
                break;
            case EnemyType.SpecialGolem:
                Hp = 100;
                break;
        }
    }
    
    /// <summary>
    /// ScriptableObject로 관리되는 데이터값을 가져와 변수에 대입
    /// </summary>
    private void SetPlayerStat(PlayerStatData playerStatData)
    {
        MaxHp = playerStatData.MaxHp;
        Hp = MaxHp;
        MaxMp = playerStatData.MaxMp;
        Mp = MaxMp;
        Defense = playerStatData.Defense;
        RecoveryHp = playerStatData.RecoveryHp;
        Dodge = playerStatData.Dodge;
        HitPercent = playerStatData.HitPercent;
        ReduceDamage = playerStatData.ReduceDamage;
        MoveSpeed = playerStatData.MoveSpeed;
        Attack = playerStatData.Attack;
    }

    /// <summary>
    /// ScriptableObject로 관리되는 데이터값을 가져와 변수에 대입
    /// </summary>
    private void SetEnemyStat()
    {
        
    }
}
