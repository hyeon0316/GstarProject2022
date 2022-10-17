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
    /// 애니메이션 관련
    /// </summary>
    public const string SelectTrigger = "Select";
    public const string BackTrigger = "Back";
    public const string MoveBlend = "Move";
    public const string NomalAttackInteger = "NomalAttack";
    public const string WideAreaBarrageTrigger = "WideAreaBarrage";

    
    public const int MaxCombo = 4; // 기본공격의 총 콤보 수
    public const int InitCount = -1; // 기본공격의 처음단계 상태

}
