using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum LayerType
{
    Player,
    Enemy
}

public abstract class Creature : MonoBehaviour
{
  public Stat Stat;

  protected Animator _animator;
  protected NavMeshAgent _nav;
  
  [Header("기본공격 범위")]
  [SerializeField] protected float _attackRadius; //실제 멈춰서서 공격하는 범위

  [SerializeField] protected FloatingText _floatingText;
  
  protected List<Transform> _targets = new List<Transform>(); //탐색된 적의 정보

  public List<Transform> Targets => _targets;

  public bool IsDead { get; private set; }
  
  protected Rigidbody _rigid;
  
  protected virtual void Awake()
  {
      _animator = GetComponent<Animator>();
      _nav = GetComponent<NavMeshAgent>();
      _rigid = GetComponent<Rigidbody>();
  }


  /// <summary>
  /// 플레이어가 죽었을때 등 초기화 해야할 상황이 생길 때 사용
  /// </summary>
  protected virtual void Init()
  {
      //Stat.Hp = Stat.MaxHp;
      Debug.Log(Stat.Hp);
      //todo: 그외 초기 설정값 적용
  }

  public void Heal(int amount)
  {
      Stat.Hp += amount;

      if (Stat.MaxHp > Stat.Hp)
          Stat.Hp = Stat.MaxHp;
  }
  
  /// <summary>
  /// 
  /// </summary>
  /// <param name="amount">상대방의 최종 데미지</param>
  /// <param name="pureDamage">상대방의 순수 데미지</param>
  public virtual void TakeDamage(int amount , int pureDamage)
  {
      if (!IsDead)
      {
          int resultDamage = Mathf.Clamp((pureDamage - Stat.Defense) / 2, 0, 100) * amount / 100;
          Stat.Hp -= resultDamage;
          _floatingText.CreateFloatingText(resultDamage);

          if (Stat.Hp <= 0)
          {
              Stat.Hp = 0;
              Die();
          }
      }
  }

    public bool HitDamage(int hitPercent)
    {
        int n = hitPercent - Stat.Dodge + 10;
        n = Math.Clamp(n, 0, 60);
        double result = Math.Pow(0.91f, n);
        result = result * -1;
        result = (result + 1) * 100;
        Debug.Log(result);
        return Random.Range(0f, 100f) < result;
    }

  

  protected virtual void Die()
  {
      _floatingText.ClearText();
      IsDead = true;
      this.gameObject.layer = LayerMask.NameToLayer("Dead");
  }

}
