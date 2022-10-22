using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 장비 아이템 </summary>
public abstract class EquipmentItemData : ItemData
{
    public ItemStat Stat;
    
    public ItemStat GetStat()
    {
        Stat.Attack = _damage;
        Stat.HitPercent = _hit;
        
        Stat.Defense = _defense;
        Stat.Dodge = _evasion;
        return Stat;
    }
    /// <summary> 최대 내구도 </summary>
    [SerializeField] private int  MaxDurability => _maxDurability;
    /// <summary> 공격력 </summary>
    [SerializeField] private int  _damage = 0;
    /// <summary> 명중 </summary>
    [SerializeField] private int  _hit = 0;
    /// <summary> 스킬데미지 </summary>
    [SerializeField] private int  _skillDamage = 0;
    /// <summary> 모든데미지 </summary>
    [SerializeField] private int  _allDamage = 0;
    /// <summary> 방어력 </summary>
    [SerializeField] private int  _defense = 0;
    /// <summary> 회피 </summary>
    [SerializeField] private int  _evasion = 0;
    /// <summary> 모든 받는 데미지 감소 </summary>
    [SerializeField] private int  _allDeense = 0;
    /// <summary> HP </summary>
    [SerializeField] private int _hp = 0;
    /// <summary> MP </summary>
    [SerializeField] private int _mP = 0;
    /// <summary> 최대물약개수 </summary>
    [SerializeField] private int  _Postion = 0;
    /// <summary> HP회복량 </summary>
    [SerializeField] private int  _hpRe = 0;
    /// <summary> MP회복량 </summary>
    [SerializeField] private int  _mpRe = 0;
    [SerializeField] private int _maxDurability = 100;
    [SerializeField] private int _equType = 1;

    public int EquType => _equType;
}