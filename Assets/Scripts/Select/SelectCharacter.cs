using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectCharacter : MonoBehaviour, IPointerDownHandler
{
    public JobType CurJobType;
    public string JobName;
    [TextArea]
    public string JobDescription;

    public SelectCamera SelectCamera;


    public void OnPointerDown(PointerEventData eventData)
    {
        SelectCamera.LookCharacter(this);
    }
}
