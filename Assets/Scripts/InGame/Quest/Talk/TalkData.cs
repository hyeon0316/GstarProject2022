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
    }
    void GenerateData()
    {
        talkData.Add(1, new string[] { "벨라1","들어가기에는 너무 깊어 보인다.", 
            "들어가기에는 너무 깊어 보인다2.", "들어가기에는 너무 깊어 보인다3." });
        talkData.Add(2, new string[] { "헬라2","들어가기에는 너무 깊어2 보인다.",
            "들어가기에는 너무 깊어 보인3다2.", "들어가기에는 너4무 깊어 보인다3." });
        talkData.Add(3, new string[] { "헬라3","들어가기에는 너무 깊어2 보인다.",
            "들어가기에는 너무 깊어 보인3다2.", "들어가기에는 너4무 깊어 보인다3." });
        talkData.Add(4, new string[] { "헬라3","들어가기에는 너무 깊어2 보인다.",
            "들어가기에는 너무 깊어 보인3다2.", "들어가기에는 너4무 깊어 보인다3." });
        talkData.Add(101, new string[] { "헬라3","들어가기에는 너무 깊어2 보인다.",
            "들어가기에는 너무 깊어 보인3다2.", "들어가기에는 너4무 깊어 보인다3." });


    }
    public string[] GetStr(int _s)
    {
        return talkData[_s];
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
