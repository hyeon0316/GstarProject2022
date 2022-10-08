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
    public int Defense { get; set; }
    public int Dodge { get; set; }
    public int HitPercent { get; set; }
    public int ReduceDamage { get; set; } // 받는 모든 데미지 감소량
    public int MoveSpeed { get; set; }
    public int Attack { get; set; }


}
