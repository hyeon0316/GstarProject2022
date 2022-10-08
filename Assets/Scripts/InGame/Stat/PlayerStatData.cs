using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "New Stat/PlayerStat")] 
public class PlayerStatData : ScriptableObject
{
    [Header("최대 체력")]
    public int MaxHp;
    [Header("최대 마나")]
    public int MaxMp;
    [Header("자연 Hp회복량")]
    public int RecoveryHp;
    [Header("방어력")]
    public int Defense;
    [Header("회피")]
    public int Dodge;
    [Header("명중")]
    public int HitPercent;
    [Header("받는 모든 데미지 감소량")]
    public int ReduceDamage;
    [Header("이동속도")]
    public int MoveSpeed;
    [Header("기본 공격력")]
    public int Attack;
    
}
