using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerStatUI : MonoBehaviour
{
    private string _str;
    public TextMeshProUGUI PlayerStatText;
    private void Start()
    {
        _str = "";
    }
    void Update()
    {
        UpdateStat(DataManager.Instance.Player.Stat);
        PlayerStatText.text = _str;
    }
    private void UpdateStat(Stat _stat)
    {
        _str = "";
        _str += "--------���� ------\n";
        _str += "���ݷ� : " + _stat.Attack + "\n";
        _str += "���� : " + _stat.HitPercent + "\n";
        _str += "��ų������ : " + _stat.SkillDamage + "%\n";
        _str += "��絥���� : " + _stat.AllDamge + "%\n";
        _str += "--------��� ------\n";
        _str += "���� : " + _stat.Defense + "\n";
        _str += "ȸ�� : " + _stat.Dodge + "\n";
        _str += "�޴� ��� ������ ���� : " + _stat.ReduceDamage + "%\n";
        _str += "--------��Ÿ ------\n";
        _str += "�ִ� ä�� : " + _stat.MaxHp + "\n";
        _str += "�ִ� ���� : " + _stat.MaxMp + "\n";
        _str += "���� ���� ���� : " + _stat.MaxPostion + "\n";
        _str += "HPȸ���� : " + _stat.RecoveryHp + "\n";
        _str += "MPȸ���� : " + _stat.RecoveryMp + "\n";
    }
}
