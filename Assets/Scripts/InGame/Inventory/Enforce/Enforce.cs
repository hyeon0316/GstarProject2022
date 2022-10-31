using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enforce : MonoBehaviour
{
    
    public ItemStat[] EStat;
    public int[] EnforceNum;

    
    
    public ItemData[] _slots;

    private const int ATTACK = 3;
    private const int HIT = 3;
    private const int SKILLD = 2;
    private const int ALLD = 2;

    private const int DEF = 4;
    private const int DODGE = 3;
    private const int REDEF = 1;

    private const int HP = 50;
    private const int MP = 50;
    private const int POSTION = 100;
    private const int REHP = 5;
    private const int REMP = 3;

    void Start()
    {
        EStat = new ItemStat[6];
        EnforceNum = new int[6];
        
        Item item;
        for (int i = 0; i < 6; i++)
        {
            
            item = _slots[i].CreateItem();
            if(item is EquipmentItem _uItem)
            {
                EStat[i] = _uItem.EquipmentData.GetStat();
            }
               EnforceNum[i] = 0;
        }
        
    }

    /// <summary> 성공1 실패 2 하락3 초기화4 20레벨5</summary>
    public int EnforceSlot(int _num)
    {
        if (_num == 20)
            return 5;
        int _ran;
        _ran = Random.Range(0, 100);
        if (_num < 5 )
        {
            if (_ran < 50)
                return 1;
            else
                return 2;
        }
        if(_num <10)
        {
            if (_ran < 50)
                return 1;
            else if (_ran < 80)
            {
                return 2;
            }
            else
                return 3;
        }
        else if(_num <15)
        {
            if (_ran < 30)
                return 1;
            if (_ran < 80)
                return 2;
            else
                return 3;
        }
        else
        {
            if (_ran < 20)
                return 1;
            if (_ran < 70)
                return 2;
            if (_ran < 90)
                return 3;
            else
                return 4;
        }
    }
    
    public void OnButton(int _index)
    {
        int _i;
        _i = EnforceSlot(EnforceNum[_index]);
        switch(_i)
        {
            case 1:
                EnforceNum[_index]++;
                break;
            case 2:
                break;
            case 3:
                EnforceNum[_index]--;
                break;
            case 4:
                EnforceNum[_index]=1;
                break;
            case 5:
                EnforceNum[_index]=20;
                break;
        }
    }
    public ItemStat GetStat(int _index)
    {
        ItemStat _stat;
        _stat = EStat[_index].DeepCopy();
        SetStat(EnforceNum[_index],ref _stat);
        return _stat;
    }

    public void SetStat(int _num , ref ItemStat _stat)
    {
        if (CheckZero(_stat.Attack))
        {
            _stat.Attack += _num * ATTACK;
        }
        if (CheckZero(_stat.HitPercent))
            _stat.HitPercent += _num * HIT;
        if (CheckZero(_stat.SkillDamage))
            _stat.SkillDamage += _num * SKILLD;
        if (CheckZero(_stat.AllDamge))
            _stat.AllDamge += _num * ALLD;

        if (CheckZero(_stat.Defense))
            _stat.Defense += _num * DEF;
        if (CheckZero(_stat.Dodge))
            _stat.Dodge += _num * DODGE;
        if (CheckZero(_stat.ReduceDamage))
            _stat.ReduceDamage += _num * REDEF;

        if (CheckZero(_stat.MaxHp))
            _stat.MaxHp += _num * HP;
        if (CheckZero(_stat.MaxMp))
            _stat.MaxMp += _num * MP;

        /* 포션 개수
        if (CheckZero(_stat.Max))
            _stat.HitPercent += _num * POSTION;
        */

        if (CheckZero(_stat.RecoveryHp))
            _stat.RecoveryHp += _num * REHP;
        if (CheckZero(_stat.RecoveryMp))
            _stat.RecoveryMp += _num * REMP;
    }
    private bool CheckZero(int _a)
    {
        return _a != 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
