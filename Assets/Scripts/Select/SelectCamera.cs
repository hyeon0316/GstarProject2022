using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SelectCamera : MonoBehaviour
{
   private Camera _camera;
   private Vector3 _originRotation;//기존 카메라의 rotation 값

   private float _originView; //기존 카메라의 Field Of View 값

   [Header("카메라 이동속도")]
   [Range(0.1f,10)]
   [SerializeField] private float _lookSpeed;

   [Header("확대 View 값")]
   [SerializeField] private int _transView;

   [Space(10)]
   [SerializeField] private SelectRoom _selectRoom;
   
   [SerializeField] private GameObject _selectUI;

   private void Awake()
   {
      _camera = GetComponent<Camera>();
      _originRotation = _camera.transform.rotation.eulerAngles;
      _originView = _camera.fieldOfView;
   }

   
   /// <summary>
   /// 선택한 캐릭터에게 카메라 이동 및 정보 표시
   /// </summary>
   public void LookCharacter(SelectCharacter selectCharacter)
   {
      transform.DOLookAt(selectCharacter.transform.position, _lookSpeed);
      _camera.DOFieldOfView(_transView, _lookSpeed);
      
      _selectRoom.SetCharacterInfo(selectCharacter);
      _selectUI.SetActive(true);
   }
   

   /// <summary>
   /// 캐릭터 선택 전 기존 뷰로 설정
   /// </summary>
   public void SetOriginView()
   {
      transform.DORotate(_originRotation, _lookSpeed);
      _camera.DOFieldOfView(_originView, _lookSpeed);
      
      _selectUI.SetActive(false);
   }

  
}
