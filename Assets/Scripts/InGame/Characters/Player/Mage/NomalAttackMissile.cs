using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttackMissile : MonoBehaviour
{
   
   [SerializeField] private float _missileSpeed;
   [SerializeField] private int _damage;
   
   private void OnEnable()
   {
      Invoke("DisableMissile", 0.5f);
   }

   private void FixedUpdate()
   {
      transform.Translate(Vector3.forward * _missileSpeed);
   }

   private void DisableMissile()
   {
      ObjectPoolManager.Instance.ReturnObject(PoolType.NomalAttackMissile, this.gameObject);
   }


   private void OnTriggerEnter(Collider other) 
   {
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
         //collision.transform.GetComponent<Enemy>().TakeDamage(_damage);
         CreateEffect();
         DisableMissile();
      }
   }

   private void CreateEffect()
   {
      GameObject effect = ObjectPoolManager.Instance.GetObject(PoolType.NomalAttackEffect);
      effect.transform.position = this.transform.position;
   }
}
