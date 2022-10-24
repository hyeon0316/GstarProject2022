using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전역 변수 모음
/// </summary>
public static class Global
{
    public const int LoadingTime = 5;

    /// <summary>
    /// 플레이어 애니메이션 관련
    /// </summary>
    public const string SelectTrigger = "Select";
    public const string BackTrigger = "Back";
    public const string MoveBlend = "Move";
    public const string NormalAttackInteger = "NormalAttack";
    public const string WideAreaBarrageTrigger = "WideAreaBarrage";
    public const string ChainLightningTrigger = "ChainLightning";
    public const string BulletRainTrigger = "BulletRain";
    
    public const int MaxComboAttack = 4; // 기본공격의 총 콤보 수
    public const int InitAttackCount = -1; // 기본공격의 처음단계 상태

    /// <summary>
    /// 적 애니메이션 관련
    /// </summary>
    public const string EnemyDeadTrigger = "Dead";
    public const string EnemyStateInteger = "State";

}
