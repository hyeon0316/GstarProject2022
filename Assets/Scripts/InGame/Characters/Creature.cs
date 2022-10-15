using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
  //todo: 체력 갱신, 공격, 사망 등 기본적인 생명체에 대한 추상 함수 선언 
  public Stat Stat;

  protected Animator _animator;

  [Header("적 탐색 범위")]
  [SerializeField] protected float _searchRadius;
  
  [Header("기본공격 범위")]
  [SerializeField] protected float _attackRadius; //실제 멈춰서서 공격하는 범위

  protected Transform _targets; //탐색된 적의 정보

  
  public virtual void Awake()
  {
      _animator = GetComponent<Animator>();
  }


  /// <summary>
  /// 플레이어가 죽었을때 등 초기화 해야할 상황이 생길 때 사용
  /// </summary>
  public void Init()
  {
      Stat.Hp = Stat.MaxHp;
      //todo: 그외 초기 설정값 적용
  }

  public void Heal(int amount)
  {
      Stat.Hp += amount;

      if (Stat.MaxHp > Stat.Hp)
          Stat.Hp = Stat.MaxHp;
  }
  public void TakeDamage(int amount)
  {
      Stat.Hp -= amount;

      if (Stat.Hp < 0)
      {
          Stat.Hp = 0;
          Die();
      }
  }



  public abstract void Die();

}
