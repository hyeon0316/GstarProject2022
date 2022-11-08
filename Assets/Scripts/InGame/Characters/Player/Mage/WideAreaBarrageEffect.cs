using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class WideAreaBarrageEffect : MonoBehaviour
{
   [SerializeField] private int _percentDamage;

   [Header("데미지가 적용되기 까지 시간")]
   [SerializeField] private float _dealTime;

   /// <summary>
   /// 적들이 장판안에 들어올때 각 적마다의 정보와 데미지 발생을 위함과 장판에 나갔을때 데미지 발생을 중단하기 위한 데이터 저장 변수
   /// </summary>
   private Dictionary<Collider, IEnumerator> _targets = new Dictionary<Collider, IEnumerator>();

   public void DelayDisable()
   {
      Invoke("DisableEffect", 7f);
   }
   

   private void DisableEffect()
   {
      ObjectPoolManager.Instance.ReturnObject(PoolType.WideAreaBarrageEffect, this.gameObject);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
         if(!_targets.ContainsKey(other))
            _targets.Add(other, TakeBarrageDamageCo(other.GetComponent<Creature>()));
         
         Debug.Log($"들어옴{other.name}");
         
         StartCoroutine(_targets[other]);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
         if (_targets.ContainsKey(other))
         {
            StopCoroutine(_targets[other]);
            _targets.Remove(other);
         }
      }
   }

   private IEnumerator TakeBarrageDamageCo(Creature enemy)
   {
      int count = 0;
      while (count < 10)
      {
         if (enemy == null)
            break;
         
         yield return new WaitForSeconds(_dealTime);
         Stat playerStat = DataManager.Instance.Player.Stat;
         float resultDamage = playerStat.Attack * _percentDamage / 100 * playerStat.SkillDamage / 100 *
            playerStat.AllDamge / 100 * Random.Range(0.8f, 1f);
         enemy.TakeDamage((int)resultDamage, playerStat.Attack);
         count++;
      }
   }

  
  

  
}
