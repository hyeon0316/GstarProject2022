using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Enemy
{
    public override void Awake()
    {
        base.Awake();
        Stat = new EnemyStat();
    }

    public override void Die()
    {
        Debug.Log($"{this.GetType().Name}사망");
    }
}
