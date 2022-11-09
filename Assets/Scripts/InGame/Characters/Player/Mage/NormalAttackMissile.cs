using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NormalAttackMissile : NormalAttack
{
   
   [SerializeField] private float _missileSpeed;
   
   private void FixedUpdate()
   {
      transform.Translate(Vector3.forward * _missileSpeed);
   }

   public void DelayDisable()
   {
      Invoke("DisableObject", 0.5f);
   }
   

   private void OnTriggerEnter(Collider other) 
   {
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
         other.transform.GetComponent<Creature>().TryGetDamage(DataManager.Instance.Player.Stat, this);
         CreateEffect();
         DisableObject();
      }
   }

   private void CreateEffect()
   {
      GameObject effect = ObjectPoolManager.Instance.GetObject(PoolType.NormalAttackEffect);
      effect.transform.position = this.transform.position;
      effect.GetComponent<NormalAttackEffect>().DelayDisable();
   }
}
