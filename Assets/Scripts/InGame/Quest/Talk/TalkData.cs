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
        talkData.Add(1, new string[] { "벨라","땅끝마을에 새로운 손님이 찾아오셨네요",
            "안녕하세요 마법사님.\n 저는 마을 안내원 벨라 라고 합니다.", 
            "혹시 저희 마을을 도와주실수 있으신가요? \n 보상은 충분히 드리겠습니다." ,
            "감사합니다 마법사님!\n 우선 장비를 드리겠습니다!. \n 마을상인에게 가주세요!"});
        talkData.Add(2, new string[] { "릴리","지금 마을앞에 몬스터들이 친입했습니다 !..",
            "몬스터무리를 처치하시고 루이스에게 가시면 좋은 아이템을 드리겠습니다." });
        talkData.Add(3, new string[] { "루이스","고생하셨습니다 마법사님",
            "이앞은 고블린이 들어와있습니다. 몬스터가 강력하니 조심하시길..",});
        talkData.Add(4, new string[] { "헬라3","들어가기에는 너무 깊어2 보인다.",
            "들어가기에는 너무 깊어 보인3다2.", "들어가기에는 너4무 깊어 보인다3." });
        talkData.Add(101, new string[] { "마을상인","안녕 하십니까 마법사님.",
            "벨라에게 이야기는 들었습니다 여기 약속드린 포션입니다!.", "앞으로 포션이 필요하시면 저한테 오시면 됩니다!" });


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
