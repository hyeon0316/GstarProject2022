using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TalkUI : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Talktext;

    public bool isAnim;
    string targetMsg;
    int index;

    private string[] strs;
    private int strInt;
    public void SetText(string[] _str)
    {
        strs = _str;
        Name.text = _str[0];
        strInt = 1;
        isAnim = false;
        Debug.Log(_str[1]);
        SetMsg(_str[1]);


    }
    public void OnsButton()
    {

        if (isAnim)
        {
            EffectEnd();
        }
        else
        {
            strInt++;
            if (strInt < strs.Length)
                SetMsg(strs[strInt]);
            else
            {
                gameObject.SetActive(false);
            }
        }
    }


    public void SetMsg(string msg)
    {
        if (isAnim)
        {
            CancelInvoke();//실행중이던 Invoke함수 캔슬
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }
    void EffectStart()//대화창의 텍스트가 한글자씩 출력
    {
        isAnim = true;
        Talktext.text= "";
        index = 0;
        Invoke("Effecting", 0.05f);
    }
    void Effecting()//텍스트 출력 진행 중
    {
        if (Talktext.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        Talktext.text += targetMsg[index];
        index++;
        Invoke("Effecting", 0.05f);//재귀함수
    }
    void EffectEnd()//텍스트 모두 출력
    {
        Talktext.text = targetMsg;
        isAnim = false;
    }
}
