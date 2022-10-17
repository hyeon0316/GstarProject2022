using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttackEffect : MonoBehaviour
{
  
   public void DelayDisable()
   {
      Invoke("DisableEffect", 1.5f);
   }
   
   private void DisableEffect()
   {
      ObjectPoolManager.Instance.ReturnObject(PoolType.NomalAttackEffect, this.gameObject);
   }
}
