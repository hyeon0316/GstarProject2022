using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지속적인 연결이 필요한 데이터 관리
/// </summary>
public class DataManager : Singleton<DataManager>
{
    public JobType SelectJobType { get; set; } = JobType.Mage;
    public Player Player { get; set; } //todo: 캐릭터 선택할때 같이 적용
    public int Gold=0;
    public Transform[] NpcTransForm;
    public Transform[] MapTransfom;
    public Transform[] EnemyTransfom;

    public NpcData TargetNpc;

    public Transform GetNpcData(int _id)
    {
        for(int i=0;i<NpcTransForm.Length;i++)
        {
            if(NpcTransForm[i].GetComponent<NpcData>().ID == _id)
            {
                return NpcTransForm[i];
            }
        }
        return null;
    }
    public Transform GetEnemySpwan(int _id)
    {
        Debug.Log(_id);
        switch(_id)
        {
            case 4:
               return EnemyTransfom[1];
            case 5:
                return EnemyTransfom[0];
        }
        return null;
    }    
    
}
