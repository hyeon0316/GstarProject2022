using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
  //todo: 체력 갱신, 공격, 사망 등 기본적인 생명체에 대한 추상 함수 선언 
  public Stat Stat;


  /// <summary>
  /// 플레이어가 죽었을때 등 초기화 해야할 상황이 생길 때 사용
  /// </summary>
  public void Init()
  {
      Stat.Hp = Stat.MaxHp;
      //todo: 그외 초기 설정값 적용
  }

  public void UpdateHp(int amount)
  {
      Stat.Hp += amount;

      if (Stat.Hp > Stat.MaxHp)
          Stat.Hp = Stat.MaxHp;
      else if (Stat.Hp < 0)
          Stat.Hp = 0;
      
      //todo: 나중에는 회복과 데미지를 따로 나누는게 좋을 듯(TakeDamage, Heal)
  }
}
