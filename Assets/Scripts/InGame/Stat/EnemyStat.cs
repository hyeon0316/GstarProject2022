using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 전용 스탯을 따로 추가
/// </summary>
public class EnemyStat : Stat
{
   //todo:적 종류별 생성자 작성

   public EnemyStat()
   {
      MaxHp = 100;
      Hp = MaxHp;
   }
    
}
