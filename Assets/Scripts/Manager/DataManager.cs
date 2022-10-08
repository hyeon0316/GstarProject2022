using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지속적인 연결이 필요한 데이터 관리
/// </summary>
public class DataManager : Singleton<DataManager>
{
    public JobType SelectJobType { get; set; }
    public Player CurPlayer { get; set; }
    
}
