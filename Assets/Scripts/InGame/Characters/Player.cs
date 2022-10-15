using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Creature
{
   public bool IsNomalAttack { get; set; } //기본공격을 하고 있는 상태인지에 대한 bool값
   public bool IsMoveToward { get; set; } //자동이동을 하고있는지에 대한 bool값
   private int _comboCount;

   private bool _canNextNomalAttack; //다음단계의 기본공격이 가능한지에 대한 bool값
   private float _nomalAttackCancelDelay; //기본공격 콤보가 끊어지는 시간

   private IEnumerator _moveCo; //어떤 코루틴을 stop할것인지 알기 위한 할당 변수
   
   public override void Awake()
   {
      base.Awake();
      Stat = new PlayerStat(DataManager.Instance.SelectJobType);
   }

   private void Update()
   {
      if (_canNextNomalAttack)
      {
         CheckInitCombo(); //코루틴 대신 사용
      }
   }


   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.green;
      Gizmos.DrawSphere(transform.position, _searchRadius);
   }

   /// <summary>
   /// 한번 타겟을 잡은 적이 공격범위에서 벗어날때 다시 가까운 다른 적을 타겟으로 잡는다.
   /// </summary>
   private bool IsOtherTarget()
   {
      Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius, LayerMask.GetMask("Enemy"));
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
         Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius, LayerMask.GetMask("Enemy"));
         float shortDis = _searchRadius; //가장 가까운 적과의 거리

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
            if (_attackRadius < Vector3.Distance(transform.position, _targets.transform.position))
            {
               _moveCo = MoveTowardEnemyCo();
               StartCoroutine(_moveCo);
            }
            else
            {
               NomalAttack();
            }
         }
      }
   }

   public void StopMoveCo()
   {
      if(_moveCo !=null)
         StopCoroutine(_moveCo);
   }
   
   /// <summary>
   /// 탐색 사거리 안의 적이 공격사거리 보다 클 때 공격 사거리 안에 들어올때까지 해당 적에게 이동
   /// </summary>
   private IEnumerator MoveTowardEnemyCo()
   {
      while (true)
      {
         if (_attackRadius >= Vector3.Distance(transform.position, _targets.transform.position)) //공격 사거리 안에 들어왔을때
         {
            NomalAttack();
            Move(transform.rotation.eulerAngles, 0);
            break;
         }
         transform.LookAt(new Vector3(_targets.position.x, transform.position.y, _targets.position.z));
         Move(transform.rotation.eulerAngles, 1);
         yield return new WaitForFixedUpdate();
      }
   }

   
   /// <summary>
   /// 기본공격을 사용될때 실제로 동작되는 기능
   /// </summary>
   private void NomalAttack()
   {
      transform.LookAt(new Vector3(_targets.position.x, transform.position.y, _targets.position.z));
      _animator.SetInteger(Global.NomalAttack, _comboCount++ % Global.MaxCombo);
      IsNomalAttack = true;
      _canNextNomalAttack = false;
   }
   

   /// <summary>
   /// 기본 공격의 애니메이션 단계를 처음으로 초기화
   /// </summary>
   public void InitNomalAttack()
   {
      _animator.SetInteger(Global.NomalAttack, Global.InitCount);
      IsNomalAttack = false;

      _nomalAttackCancelDelay = 1f;
      _canNextNomalAttack = true;
   }

   /// <summary>
   /// 콤보를 초기화
   /// </summary>
   private void CheckInitCombo()
   {
      _nomalAttackCancelDelay -= Time.deltaTime;
      if (_nomalAttackCancelDelay <= 0)
      {
         _comboCount = 0;
         _canNextNomalAttack = false;
      }
   }

   /// <summary>
   /// 플레이어 이동
   /// </summary>
   /// <param name="angle">이동방향각도</param>
   /// <param name="moveDistance">조이스틱 이동거리</param>
   public void Move(Vector3 angle, float moveDistance)
   {
      transform.rotation = Quaternion.Euler(angle);
      transform.Translate(Vector3.forward * moveDistance * Stat.MoveSpeed * Time.fixedDeltaTime);
      _animator.SetFloat(Global.MoveBlend, moveDistance);
   }

   public override void Die()
   {
      //todo: 플레이어가 죽었을때 처리(마을 부활, 체력 회복 등)
   }
}
