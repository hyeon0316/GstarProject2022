using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinArcherArrow : MonoBehaviour
{
    [SerializeField] private float _missileSpeed;
    [SerializeField] private int _damage;
   
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _missileSpeed);
    }
    
    public void DelayDisable()
    {
        Invoke("DisableMissile", 0.5f);
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.GetComponent<Creature>().TakeDamage(_damage, _damage);
            DisableMissile();
        }
    }
    
    private void DisableMissile()
    {
        ObjectPoolManager.Instance.ReturnObject(PoolType.GoblinArcherArrow, this.gameObject);
    }
}
