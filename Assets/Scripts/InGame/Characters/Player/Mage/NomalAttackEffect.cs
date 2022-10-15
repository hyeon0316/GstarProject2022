using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttackEffect : MonoBehaviour
{
   private void OnEnable()
   {
      Invoke("DisableEffect", 1.5f);
   }

   private void DisableEffect()
   {
      ObjectPoolManager.Instance.ReturnObject(PoolType.NomalAttackEffect, this.gameObject);
   }
}
