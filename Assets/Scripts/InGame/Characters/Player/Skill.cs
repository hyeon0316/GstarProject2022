using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private int _percentDamage;

    public float GetDamage()
    {
        Stat playerStat = DataManager.Instance.Player.Stat;
        // 플레이어공격력 * _percentDamage * 스킬데미지% * 모든데미지% * 80%~100%
        float resultDamage = playerStat.Attack * _percentDamage / 100 * playerStat.SkillDamage / 100 *
            playerStat.AllDamge / 100 * Random.Range(0.8f, 1f);

        return resultDamage;
    }


    
}
