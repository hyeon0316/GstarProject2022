using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ActiveSlotUI : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI MainStatName;
    public TextMeshProUGUI MainStat;
    public TextMeshProUGUI SubStat;
    public TextMeshProUGUI EnforceStat;
    private ItemStat _itemStat;
    public void ShowUI() => gameObject.SetActive(true);
    public void HideUI() => gameObject.SetActive(false);

    // Start is called before the first frame update
    void Start()
    {
        HideUI();
    }
    public void SetStat(ItemStat _stat)
    {
        _itemStat = _stat;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ResetUI()
    {
        _itemStat = null;
        Icon.sprite = null;
        HideUI();
    }
    public void UpdateUI(Item _item)
    {
        
        if (_item is WeaponItem _weaponItem)
        {
            _itemStat = _weaponItem.EquipmentData.GetStat();
            SetMainStat(_itemStat.Attack, 2);
            SetSubStat();
        }
        else if (_item is EquipmentItem _uItem)
        {
            _itemStat = _uItem.EquipmentData.GetStat();
            SetMainStat(_itemStat.Defense, 1);
            SetSubStat();
        }
        else if(_item is IUsableItem _use)
        {
            
        }
        else
        {

        }
        SetIcon(_item.Data.IconSprite);
        
        ShowUI();
    }
    public void SetIcon(Sprite _image)
    {
        Icon.sprite = _image;
    }
    public void SetName(string _str)
    {
        Name.text = _str;
    }
    public void SetMainStat(int _stat,int _i)
    {
        string _str = "";
        if(_i == 1)
        {
            _str = "방어력";
        }
        if (_i == 2)
        {
            _str = "공격력";
        }
        if(_i == 3)
        {
            _str = "소모품";
        }
        if (_i == 4)
        {
            _str = "재료";
        }
        MainStatName.text = _str;
        MainStat.text = _stat.ToString();
       
    }
    public void SetSubStat()
    {
        SubStat.text = Setstr();
    }
    public void SetEnforceStat(int _stat)
    {
        EnforceStat.text = _stat.ToString();
    }
    private string Setstr()
    {
        string _str = "";
        if (_itemStat.HitPercent != 0)
            _str += "명중 : " + _itemStat.HitPercent + "\n";
        if (_itemStat.SkillDamage != 0)
            _str += "스킬데미지 : " + _itemStat.SkillDamage + "%\n";
        if (_itemStat.AllDamge != 0)
            _str += "모든데미지 : " + _itemStat.AllDamge + "%\n";
        if (_itemStat.Dodge != 0)
            _str += "회피 : " + _itemStat.Dodge + "\n";
        if (_itemStat.ReduceDamage != 0)
            _str += "받는 모든 데미지 감소 : " + _itemStat.ReduceDamage + "%\n";
        if (_itemStat.MaxHp != 0)
            _str += "최대 채력 : " + _itemStat.MaxHp + "\n";
        if (_itemStat.MaxMp != 0)
            _str += "최대 엠피 : " + _itemStat.MaxMp + "\n";
        if (_itemStat.MaxPostion != 0)
            _str += "물약 소지 개수 : " + _itemStat.MaxPostion + "\n";
        if (_itemStat.RecoveryHp != 0)
            _str += "HP회복량 : " + _itemStat.RecoveryHp + "\n";
        if (_itemStat.RecoveryMp != 0)
            _str += "MP회복량 : " + _itemStat.RecoveryMp + "\n";

        return _str;
            
    }
}
