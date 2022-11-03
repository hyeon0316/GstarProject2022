using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
        string[] _s;
        _s = talkData[100];
        for(int i=0;i<_s.Length;i++)
        {
            Debug.Log(_s[i]);
        }    
    }
    void GenerateData()
    {
        talkData.Add(100, new string[] { "들어가기에는 너무 깊어 보인다.", "들어가기에는 너무 깊어 보인다2.", "들어가기에는 너무 깊어 보인다3." });
    }
    /*
    public string GetTalk(int id, int talkIndex)
    {
        Debug.Log(id + "," + talkIndex);
        if (!talkData.ContainsKey(id))
        {
            //해당 퀘스트 진행중 대사가 없을때 진행순서
            //퀘스트 맨처음 대사 가지고옴
            if (talkData.ContainsKey(id - id % 10))
            {
                return GetTalk(id - id % 10, talkIndex);
            }
            else
            {
                //퀘스트 맨처음 대사
                return GetTalk(id - id % 100, talkIndex);
            }
        }
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
    */
}
