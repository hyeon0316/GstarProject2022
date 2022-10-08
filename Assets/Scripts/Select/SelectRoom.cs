using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum JobType
{
    Archer,
    Mage
}
public class SelectRoom : MonoBehaviour
{
    private JobType _selectJobType;
    
    [Header("캐릭터 상세 정보")]
    [SerializeField] private TextMeshProUGUI _jobNameText;
    [SerializeField] private TextMeshProUGUI _jobDescriptionText;

    /// <summary>
    /// 선택한 캐릭터에 대한 정보를 보여줌
    /// </summary>
    public void SetCharacterInfo(SelectCharacter character)
    {
        _jobNameText.text = character.JobName;
        _jobDescriptionText.text = character.JobDescription;

        _selectJobType = character.CurJobType;
    }


    /// <summary>
    /// 최종선택한 캐릭터에 대한 데이터를 저장
    /// </summary>
    public void SaveCharacterInfo()
    {
        DataManager.Instance.SelectJobType = _selectJobType;
    }
    
}
