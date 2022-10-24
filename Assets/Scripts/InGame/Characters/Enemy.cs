using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Spider
    
}

public abstract class Enemy : Creature
{
    private void OnEnable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    // Start is called before the first frame update
    void Start()
    {
        _targets.Add(DataManager.Instance.Player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Die()
    {
        base.Die();
        //todo: 소멸 쉐이더
        _animator.SetTrigger(Global.EnemyDeadTrigger);
        Invoke("DestroyObject",1.5f); 
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);//todo: 나중에는 오브젝트풀링으로 관리
    }
}
