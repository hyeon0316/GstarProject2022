using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Player : Creature
{
   /// <summary>
   /// 공격을 하고 있는 상태인지에 대한 bool값(연속터치 방지)
   /// </summary>
   public bool IsAttack { get; set; } 
   private int _comboCount;

   /// <summary>
   /// 다음단계의 기본공격이 가능한지에 대한 bool값
   /// </summary>
   private bool _canNextNomalAttack;
   
   /// <summary>
   /// 기본공격 콤보가 끊어지는 시간
   /// </summary>
   private float _nomalAttackCancelDelay; 

   /// <summary>
   /// 어떤 코루틴을 stop할것인지 알기 위한 할당 변수
   /// </summary>
   protected IEnumerator _moveCo; 
   protected delegate void UseAttackType();
   
   public override void Awake()
   {
      base.Awake();
      Stat = new Stat(DataManager.Instance.SelectJobType);
      DataManager.Instance.Player = this;
        
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
   /// 공격할 우선순위 타겟들을(searchCount 수 만큼) 지정
   /// </summary>
   protected bool CheckAttackRange(int searchCount)
   {
      Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius, LayerMask.GetMask("Enemy"));

      _targets.Clear();
      
      if (colliders.Length == 0)
      {
         return false;
      }
      else
      {
         var searchList = colliders.OrderBy(col => Vector3.Distance(transform.position, col.transform.position)).ToList(); //가까운 순으로 정렬

         for (int i = 0; i < searchCount; i++)
         {
            if (i == searchList.Count) //찾고자 하는 타겟 수가 실제 존재하는 타겟 수 보다 적을 경우
               break;
            
            _targets.Add(searchList[i].transform);
         }

         foreach (var target in _targets)
         {
            Debug.Log(target);
         }
         
         return true;
      }
   }

   /// <summary>
   /// 기본 공격 사용
   /// </summary>
   public void UseNomalAttack()
   {
      if (!IsAttack)
      {
         if (CheckAttackRange(1))
         {
            if (_attackRadius < Vector3.Distance(transform.position, _targets[0].position)) //타겟이 공격사거리 밖에있을때
            {
               if (_moveCo == null) //버튼이 여러번 눌렸을때 코루틴 중복 방지
               {
                  _moveCo = MoveTowardEnemyCo(NomalAttack);
                  StartCoroutine(_moveCo);
               }
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
      if (_moveCo != null)
      {
         StopCoroutine(_moveCo);
         _moveCo = null;
      }
   }
   
   /// <summary>
   /// 탐색 사거리 안의 적이 공격사거리 보다 클 때 공격 사거리 안에 들어올때까지 해당 적에게 이동
   /// </summary>
   protected IEnumerator MoveTowardEnemyCo(UseAttackType useAttackType)
   {
      while (true)
      {
         if (_attackRadius >= Vector3.Distance(transform.position, _targets[0].position)) //공격 사거리 안에 들어왔을때
         {
            useAttackType();
            Move(transform.rotation.eulerAngles, 0);
            _moveCo = null;
            break;
         }
         transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
         Move(transform.rotation.eulerAngles, 1);
         yield return new WaitForFixedUpdate();
      }
   }

   
   /// <summary>
   /// 기본공격을 사용될때 실제로 동작되는 기능
   /// </summary>
   private void NomalAttack()
   {
      transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
      _animator.SetInteger(Global.NomalAttackInteger, _comboCount++ % Global.MaxCombo);
      IsAttack = true;
      _canNextNomalAttack = false;
   }
   

   /// <summary>
   /// 기본 공격의 애니메이션 단계를 처음으로 초기화
   /// </summary>
   public void InitNomalAttack()
   {
      _animator.SetInteger(Global.NomalAttackInteger, Global.InitCount);
      IsAttack = false;

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
