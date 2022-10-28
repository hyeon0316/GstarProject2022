using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.AI;

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
  
  
  protected virtual void Awake()
  {
      _animator = GetComponent<Animator>();
      _nav = GetComponent<NavMeshAgent>();
  }


  /// <summary>
  /// 플레이어가 죽었을때 등 초기화 해야할 상황이 생길 때 사용
  /// </summary>
  public void Init()
  {
      Stat.Hp = Stat.MaxHp;
      Debug.Log(Stat.Hp);
      //todo: 그외 초기 설정값 적용
  }

  public void Heal(int amount)
  {
      Stat.Hp += amount;

      if (Stat.MaxHp > Stat.Hp)
          Stat.Hp = Stat.MaxHp;
  }
  
  public virtual void TakeDamage(int amount)
  {
      if (!IsDead)
      {
          Stat.Hp -= amount;
          _floatingText.CreateFloatingText(amount);

          if (Stat.Hp <= 0)
          {
              Stat.Hp = 0;
              Die();
          }

          //Debug.Log(Stat.Hp);
      }
  }

  

  protected virtual void Die()
  {
      _floatingText.ClearText();
      IsDead = true;
      this.gameObject.layer = LayerMask.NameToLayer("Dead");
  }

}
