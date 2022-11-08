using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpikeAttack : MonoBehaviour
{
   [SerializeField] private int _percentDamage;
   
   public void DelayDisable()
   {
      Invoke("DisableSpike", 3f);
   }
   
   private void DisableSpike()
   {
      ObjectPoolManager.Instance.ReturnObject(PoolType.VolcanicSpike, this.gameObject);
   }
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
         Stat playerStat = DataManager.Instance.Player.Stat;
         float resultDamage = playerStat.Attack * _percentDamage / 100 * playerStat.SkillDamage / 100 *
            playerStat.AllDamge / 100 * Random.Range(0.8f, 1f);
         
         other.GetComponent<Creature>().TakeDamage((int)resultDamage, playerStat.Attack);
      }
   }
}
