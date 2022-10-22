using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class WideAreaBarrageEffect : MonoBehaviour
{
   [SerializeField] private int _damage;

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
         Debug.Log(other.name);
         TakeBarrageDamage(other.GetComponent<Enemy>());
      }
   }

  
   private void TakeBarrageDamage(Enemy enemy)
   {
      StartCoroutine(TakeBarrageDamageCo(enemy));
   }

   private IEnumerator TakeBarrageDamageCo(Enemy enemy)
   {
      int count = 0;
      while (count < 10)
      {
         if (!enemy.isActiveAndEnabled)
            break;
         
         yield return new WaitForSeconds(0.5f);
         enemy.TakeDamage(_damage);
         count++;
      }
   }
}
