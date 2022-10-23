using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Enemy
{
    public override void Awake()
    {
        base.Awake();
        Stat = new Stat();
        Stat.Hp = 100;
    }

    public override void Die()
    {
        Debug.Log($"{this.gameObject.name}사망");
        Destroy(this.gameObject);
    }
}
