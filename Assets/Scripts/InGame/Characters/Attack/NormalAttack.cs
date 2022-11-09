using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : Attack
{
   

    public override int CalculateDamage(Stat stat)
    {
        int resultDamage = stat.Attack * stat.AllDamge / 100;
        return resultDamage;
    }
}
