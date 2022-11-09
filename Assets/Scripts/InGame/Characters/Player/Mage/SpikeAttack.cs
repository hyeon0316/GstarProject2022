using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpikeAttack : SkillAttack
{
   public void DelayDisable()
   {
      Invoke("DisableObject", 3f);
   }
   
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
         other.GetComponent<Creature>().TryGetDamage(DataManager.Instance.Player.Stat,this);
      }
   }
}
