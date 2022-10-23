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
            _str = "����";
        }
        if (_i == 2)
        {
            _str = "���ݷ�";
        }
        if(_i == 3)
        {
            _str = "�Ҹ�ǰ";
        }
        if (_i == 4)
        {
            _str = "���";
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
            _str += "���� : " + _itemStat.HitPercent + "\n";
        if (_itemStat.SkillDamage != 0)
            _str += "��ų������ : " + _itemStat.SkillDamage + "%\n";
        if (_itemStat.AllDamge != 0)
            _str += "��絥���� : " + _itemStat.AllDamge + "%\n";
        if (_itemStat.Dodge != 0)
            _str += "ȸ�� : " + _itemStat.Dodge + "\n";
        if (_itemStat.ReduceDamage != 0)
            _str += "�޴� ��� ������ ���� : " + _itemStat.ReduceDamage + "%\n";
        if (_itemStat.MaxHp != 0)
            _str += "�ִ� ä�� : " + _itemStat.MaxHp + "\n";
        if (_itemStat.MaxMp != 0)
            _str += "�ִ� ���� : " + _itemStat.MaxMp + "\n";
        if (_itemStat.MaxPostion != 0)
            _str += "���� ���� ���� : " + _itemStat.MaxPostion + "\n";
        if (_itemStat.RecoveryHp != 0)
            _str += "HPȸ���� : " + _itemStat.RecoveryHp + "\n";
        if (_itemStat.RecoveryMp != 0)
            _str += "MPȸ���� : " + _itemStat.RecoveryMp + "\n";

        return _str;
            
    }
}
