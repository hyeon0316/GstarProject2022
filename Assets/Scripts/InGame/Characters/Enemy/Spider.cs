using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    public override void Awake()
    {
        base.Awake();
        Stat = new Stat(EnemyType.Spider);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
