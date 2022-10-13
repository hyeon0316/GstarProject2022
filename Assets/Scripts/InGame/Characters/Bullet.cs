using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   
   [SerializeField] private float _bulletSpeed;
   [SerializeField] private int _damage;
   private void OnEnable()
   {
      Invoke("DisableBullet", 1.5f);
   }

   private void FixedUpdate()
   {
      transform.Translate(Vector3.forward * _bulletSpeed);
   }

   private void DisableBullet()
   {
      ObjectPoolManager.Instance.ReturnObject(PoolType.Bullet, this.gameObject);
   }


   private void OnTriggerEnter(Collider other) //rigidbody가 이 함수를 가진 오브젝트에 붙어있어야 한다
   {
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
         //collision.transform.GetComponent<Enemy>().TakeDamage(_damage);
         DisableBullet();
      }
   }
}
