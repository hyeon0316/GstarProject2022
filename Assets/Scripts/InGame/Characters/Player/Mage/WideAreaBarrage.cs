using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideAreaBarrage : MonoBehaviour
{
    
    public void DelayDisable()
    {
        Invoke("DisableBarrage", 7f);
    }

    private void DisableBarrage()
    {
        ObjectPoolManager.Instance.ReturnObject(PoolType.WideAreaBarrage, this.gameObject);
    }

   
}
