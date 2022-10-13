using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Creature
{
   public bool IsNomalAttack { get; set; }
   private int _comboCount;

   private bool _isNextNomalAttack; //다음단계의 기본공격이 가능한지에 대한 bool값
   private float _nomalAttackDelay; //기본공격 콤보가 끊어지는 시간
   
   
   
   public override void Awake()
   {
      base.Awake();
      Stat = new PlayerStat(DataManager.Instance.SelectJobType);
   }

   private void Update()
   {
      if (_isNextNomalAttack)
      {
         CheckInitCombo(); //코루틴 대신 사용
      }
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.green;
      Gizmos.DrawSphere(transform.position, _attackRadius);
   }

   /// <summary>
   /// 한번 타겟을 잡은 적이 공격범위에서 벗어날때 다시 가까운 다른 적을 타겟으로 잡는다.
   /// </summary>
   private bool IsOtherTarget()
   {
      Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRadius, LayerMask.GetMask("Enemy"));
      if (colliders.Length != 0)
      {
         foreach (var col in colliders)
         {
            if (col.transform == _targets)
               return false;
         }
      }
      else //주번 적이 한명도 없을 때
      {
         _targets = null;
         return false;
      }

      return true;
   }

   /// <summary>
   /// 공격할 우선순위 타겟을 지정
   /// </summary>
   private void CheckAttackRange() //todo: 근접캐릭터 만들때 추상함수들로 자식에서 재정의해서 Mage랑 구분 짓기
   {
      if (IsOtherTarget())
      {
         Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRadius, LayerMask.GetMask("Enemy"));
         float shortDis = _attackRadius; //가장 가까운 적과의 거리

         foreach (var col in colliders)
         {
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (shortDis > distance)
            {
               shortDis = distance;
               _targets = col.transform;
            }
         }
         Debug.Log(_targets.name);
      }
   }

   /// <summary>
   /// 기본 공격 사용
   /// </summary>
   public void UseNomalAttack()
   {
      if (!IsNomalAttack)
      {
         CheckAttackRange();

         if (_targets != null)
         {
            transform.LookAt(new Vector3(_targets.position.x, transform.position.y, _targets.position.z));
            _animator.SetInteger(Global.NomalAttack, _comboCount++ % Global.MaxCombo);
            IsNomalAttack = true;
            _isNextNomalAttack = false;
         }
      }
   }

   /// <summary>
   /// 기본 공격의 애니메이션 단계를 처음으로 초기화
   /// </summary>
   public void InitNomalAttack()
   {
      _animator.SetInteger(Global.NomalAttack, Global.InitCount);
      IsNomalAttack = false;

      _nomalAttackDelay = 1f;
      _isNextNomalAttack = true;
   }

   /// <summary>
   /// 콤보를 초기화
   /// </summary>
   private void CheckInitCombo()
   {
      _nomalAttackDelay -= Time.deltaTime;
      if (_nomalAttackDelay <= 0)
      {
         _comboCount = 0;
         _isNextNomalAttack = false;
      }
   }

   public void Move(Vector3 angle, float moveDistance)
   {
      transform.rotation = Quaternion.Euler(angle);
      transform.Translate(Vector3.forward * moveDistance * Stat.MoveSpeed * Time.fixedDeltaTime);
      _animator.SetFloat(Global.MoveBlend, moveDistance);
   }

 
}
