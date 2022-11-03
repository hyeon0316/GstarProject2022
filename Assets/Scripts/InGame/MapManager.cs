using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public Transform[] NpcTransForm;
    public Transform[] MapTransfom;
    public Transform[] EnemyTransfom;

    public NpcData TargetNpc { get; set; }

    public Transform GetNpcData(int _id)
    {
        for (int i = 0; i < NpcTransForm.Length; i++)
        {
            if (NpcTransForm[i].GetComponent<NpcData>().ID == _id)
            {
                return NpcTransForm[i];
            }
        }
        return null;
    }
    public Transform GetEnemySpwan(int _id)
    {
        Debug.Log(_id);
        switch (_id)
        {
            case 4:
                return EnemyTransfom[1];
            case 5:
                return EnemyTransfom[0];
        }
        return null;
    }
}
