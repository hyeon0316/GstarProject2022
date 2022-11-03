using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
   [SerializeField] private int _damage;
   
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
         other.GetComponent<Creature>().TakeDamage(_damage);
      }
   }
}
