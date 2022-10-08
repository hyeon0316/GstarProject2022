using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// 카메라 이동 담당
/// </summary>
public class CameraMove : MonoBehaviour
{
    [Range(0.1f,1)]
    [SerializeField] private float _lerpSpeed;
    
    private GameObject _target;
    private Vector3 _offset;

    private void Awake()
    {
        _offset = transform.position + Vector3.down;
    }

    private void Start()
    {
        _target = DataManager.Instance.CurPlayer.gameObject;
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }
   
    private void FollowTarget()
    {
        transform.position = Vector3.Lerp(transform.position, _target.transform.position, _lerpSpeed);
    }

}
