using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 전용 스탯을 따로 추가
/// </summary>
public class PlayerStat : Stat
{
    public int MaxMp { get; set; }
    public int Mp { get; set; }
    
    public PlayerStat(JobType jobType)
    {
        switch (jobType)
        {
            case JobType.Archer:
                SetStat(Resources.Load<PlayerStatData>("Stats/Player/ArcherStat"));
                break;
            case JobType.Mage:
                SetStat(Resources.Load<PlayerStatData>("Stats/Player/MageStat"));
                break;
        }
    }
    
    /// <summary>
    /// ScriptableObject로 관리되는 데이터값을 가져와 변수에 대입
    /// </summary>
    private void SetStat(PlayerStatData playerStatData)
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

}
