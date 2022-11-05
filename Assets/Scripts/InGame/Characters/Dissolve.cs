using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private SkinnedMeshRenderer meshRenderer;
    
    [Range(0.1f,1)]
    [SerializeField] private float _doSpeed;

    private void Start(){
        meshRenderer = this.GetComponent<SkinnedMeshRenderer>();
    }
    

    /// <summary>
    /// 물체를 서서히 나타낸다.
    /// </summary>
    public void FadeIn()
    {
        meshRenderer.material.DOFloat(0, "_Cutoff", _doSpeed);
    }

    /// <summary>
    /// 물체를 서서히 지운다.
    /// </summary>
    public void FadeOut()
    {
        meshRenderer.material.DOFloat(1, "_Cutoff", _doSpeed);
    }
}