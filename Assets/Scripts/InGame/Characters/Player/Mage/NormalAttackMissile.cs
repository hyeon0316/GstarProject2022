using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NormalAttackMissile : MonoBehaviour
{
   
   [SerializeField] private float _missileSpeed;
   [SerializeField] private int _percentDamage;
   
  

   private void FixedUpdate()
   {
      transform.Translate(Vector3.forward * _missileSpeed);
   }

   public void DelayDisable()
   {
      Invoke("DisableMissile", 0.5f);
   }

   private void DisableMissile()
   {
      ObjectPoolManager.Instance.ReturnObject(PoolType.NormalAttackMissile, this.gameObject);
   }


   private void OnTriggerEnter(Collider other) 
   {
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
         Stat playerStat = DataManager.Instance.Player.Stat;
         float resultDamage = playerStat.Attack * _percentDamage / 100 * playerStat.SkillDamage / 100 *
            playerStat.AllDamge / 100 * Random.Range(0.8f, 1f);
         other.transform.GetComponent<Creature>().TakeDamage((int)resultDamage, playerStat.Attack);
         CreateEffect();
         DisableMissile();
      }
   }

   private void CreateEffect()
   {
      GameObject effect = ObjectPoolManager.Instance.GetObject(PoolType.NormalAttackEffect);
      effect.transform.position = this.transform.position;
      effect.GetComponent<NormalAttackEffect>().DelayDisable();
   }
}
