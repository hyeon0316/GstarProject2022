using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Creature
{
   
   public virtual void Awake()
   {
      Stat = new PlayerStat(DataManager.Instance.SelectJobType);
   }

   public void Move(Vector3 angle, float moveDistance)
   {
      transform.rotation = Quaternion.Euler(angle);
      transform.Translate(Vector3.forward * moveDistance * Stat.MoveSpeed * Time.fixedDeltaTime);
   }
 
}
