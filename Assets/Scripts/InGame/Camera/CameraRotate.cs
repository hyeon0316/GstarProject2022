using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 카메라 상하좌우 회전 담당
/// </summary>
public class CameraRotate : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField]
    private Transform _cameraArm;
    
    [SerializeField] private float _rotateSpeed;

    [Header("카메라 x축 최대 회전값")]
    [SerializeField] private int _xRotateMax;
    [Header("카메라 x축 최소 회전값")]
    [SerializeField] private int _xRotateMin;

    private Vector3 _beginPos;
    private Vector3 _dragPos;
    private float _xAngle;
    private float _yAngle;
    private float _xAngleTemp;
    private float _yAngleTemp;

    private void Start()
    {
        _xAngle = _cameraArm.rotation.eulerAngles.x;
        _yAngle = _cameraArm.rotation.eulerAngles.y;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _beginPos = eventData.position;

        _xAngleTemp = _xAngle;
        _yAngleTemp = _yAngle;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragPos = eventData.position;

        _yAngle = _yAngleTemp + (_dragPos.x - _beginPos.x) * 180 / Screen.width * _rotateSpeed;
        _xAngle = _xAngleTemp + (_dragPos.y - _beginPos.y) * 90 / Screen.height * _rotateSpeed;
        
        if (_xAngle > _xRotateMax) 
            _xAngle = _xRotateMax;
        if (_xAngle < _xRotateMin) 
            _xAngle = _xRotateMin;

        _cameraArm.rotation = Quaternion.Euler(-_xAngle, _yAngle, 0);
    }


}
