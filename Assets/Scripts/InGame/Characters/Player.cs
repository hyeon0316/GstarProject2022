using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Player : Creature
{
    [SerializeField] private PlayerStatData _playerStatData;
    /// <summary>
    /// 공격을 하고 있는 상태인지에 대한 bool값(연속터치 방지)
    /// </summary>
    public bool IsAttack { get;  set; }

    protected bool _isAutoHunt;

    protected List<UseActionType> _useSkills = new List<UseActionType>();
    
    private int _comboCount;
    
    [Header("스킬 쿨타임 모음")]
    [SerializeField] protected CoolDown[] _skiilCoolDown;
    
    [Header("적 탐색 범위")]
    [SerializeField] protected float _searchRadius;

    [Header("오토모드일때 탐색 범위에 곱해지는 값")]
    [SerializeField] protected float _autoModeSearch;

    [Header("캐릭터가 이동할때마다 발자국을 남길 오브젝트")]
    [SerializeField] private FootPrinter[] _foots;
    
    [SerializeField] private GameObject _autoCancelButton;

    [SerializeField] private VariableJoystick _joystick;

    [SerializeField] private Transform _cameraArm;

    [SerializeField] private Fade _fade;

    [SerializeField] private TargetPanel _targetPanel;

    /// <summary>
    /// 다음단계의 기본공격이 가능한지에 대한 bool값
    /// </summary>
    private bool _canNextNormalAttack;

    /// <summary>
    /// 기본공격 콤보가 끊어지는 시간
    /// </summary>
    private float _normalAttackCancelDelay;

    /// <summary>
    /// 어떤 코루틴을 stop할것인지 알기 위한 할당 변수
    /// </summary>
    protected IEnumerator _moveCo;

    protected delegate void UseActionType();
    protected Queue<UseActionType> _autoSkill = new Queue<UseActionType>();
    
    /// <summary>
    /// 오토 모드일때 스킬들의 사용 간격을 나누기 위한 변수
    /// </summary>
    protected bool _isNextPattern;

    /// <summary>
    /// 퀘스트를 자동 진행중인지 확인
    /// </summary>
    public bool IsQuest { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        Stat = new Stat();
        DataManager.Instance.Player = this;
    }

    protected virtual void Start()
    {
        Stat.SetPlayerStat(_playerStatData);
        _nav.enabled = false; //충돌이 활성화 되기 때문에 꺼줌, 사용할때만 활성화
    }

    protected virtual void Update()
    {
        if (_canNextNormalAttack)
        {
            CheckInitCombo(); //코루틴 대신 사용
        }
        TouchGetTarget(); 
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    /// <summary>
    /// Queue에서 꺼낸 함수를 사용
    /// </summary>
    private void UseAutoSkill(UseActionType useActionType)
    {
        useActionType();
    }
    
    /// <summary>
    /// 오토모드때 우선적으로 사용할 스킬들을 셋팅
    /// </summary>
    private void SetPrioritySkill()
    {
        for (int i = 0; i < _useSkills.Count; i++)
        {
            if(_skiilCoolDown[i].IsCoolDown) //쿨다운 중인 스킬은 담지 않는다.
                continue;
            
            if(!_autoSkill.Contains(_useSkills[i])) //중복 방지
                _autoSkill.Enqueue(_useSkills[i]);
        }
    }

    /// <summary>
    /// 자동사냥 실행
    /// </summary>
    private IEnumerator AutoHuntCo()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        while (true)
        {
            if (!_isAutoHunt)
                break;

            if (!IsAttack)
            {
                SetPrioritySkill();
                if (_autoSkill.Count == 0) //사용할 스킬이 없을때는 일반공격 사용
                {
                    UseNormalAttack();
                }
                else
                {
                    UseAutoSkill(_autoSkill.Dequeue());
                }
                yield return delay;
            }
            else
            {
                yield return null;
            }
        }
    }
    
    private void Move()
    {
        if (!IsDead && !IsAttack)
        {
            float x = _joystick.Horizontal;
            float z = _joystick.Vertical;

            Vector3 moveVec = new Vector3(x, 0, z);
            transform.Translate(Vector3.forward * _joystick.Direction.magnitude * Stat.MoveSpeed * Time.fixedDeltaTime);

            if (moveVec.sqrMagnitude == 0)
                return;

            Vector3 camAngle = _cameraArm.transform.rotation.eulerAngles;
            Vector3 camDirAngle = Quaternion.LookRotation(moveVec).eulerAngles;
            Vector3 resultAngle = Vector3.up * (camAngle.y + camDirAngle.y);
            transform.rotation = Quaternion.Euler(resultAngle);
            
            SetMoveAnim(_joystick.Direction.magnitude);
        }
    }

    public void SetMoveAnim(float blend)
    {
        _animator.SetFloat(Global.MoveBlend, blend);
    }
    
    /// <summary>
    /// 버튼을 눌렀을때 실행될 자동사냥 모드 셋팅
    /// </summary>
    public void SetAutoHunt()
    {
        if (!_isAutoHunt)
        {
            IsAttack = false;
            _searchRadius *= _autoModeSearch;
            _isAutoHunt = true;
            _isNextPattern = true;
            ActiveAutoCancelButton(true);
            StartCoroutine(AutoHuntCo());
        }
        else
        {
            CancelAutoHunt();
        }
    }

    /// <summary>
    /// 오토모드 취소 버튼
    /// </summary>
    public void CancelAutoHunt()
    {
        if (_isAutoHunt)
        {
            SetMoveAnim(0);
            ActiveAutoCancelButton(false);
            _searchRadius /= _autoModeSearch;
            _isAutoHunt = false;
            _autoSkill.Clear();
        }
        StopMoveCo();
    }

    /// <summary>
    /// 터치를 연속적으로 했을때에 대한 방지기능
    /// </summary>
    public void TouchContinue()
    {
        if (_isAutoHunt)
        {
            CancelAutoHunt();
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 직접 선택하여 타겟지정
    /// </summary>
    private void TouchGetTarget()
    {
        if (!_isAutoHunt)
        {
            if (Input.GetMouseButtonDown(0)) //PC
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000, LayerMask.GetMask("Enemy")))
                {
                    if (_searchRadius > Vector3.Distance(transform.position, hit.transform.position))
                    {
                        _targets.Clear();
                        _targets.Add(hit.transform);
                        _targetPanel.SetTargetBox(_targets[0]);
                        Debug.Log(hit.transform.gameObject.name);
                    }
                }
            }
            else if (Input.touchCount > 0) //Mobile
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) 
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                    if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000, LayerMask.GetMask("Enemy")))
                    {
                        if (_searchRadius > Vector3.Distance(transform.position, hit.transform.position))
                        {
                            _targets.Clear();
                            _targets.Add(hit.transform);
                            _targetPanel.SetTargetBox(_targets[0]);
                            Debug.Log(hit.transform.gameObject.name);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _searchRadius);
    }
    
    /// <summary>
    /// 공격할 우선순위 타겟들을(searchCount 수 만큼) 지정
    /// </summary>
    protected void CheckAttackRange(int searchCount, UseActionType useActionType)
    {
        SetMoveAnim(0);
        Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius, LayerMask.GetMask("Enemy"));

        _targets.Clear();
        if (colliders.Length != 0)
        {
            var searchList = colliders.OrderBy(col => Vector3.Distance(transform.position, col.transform.position))
                .ToList(); //가까운 순으로 정렬

            for (int i = 0; i < searchCount; i++)
            {
                if (i == searchList.Count) //찾고자 하는 타겟 수가 실제 존재하는 타겟 수 보다 적을 경우
                    break;

                _targets.Add(searchList[i].transform);
            }
            ActionFromDistance(useActionType , _targets[0]);
        }
        else
        {
            IsAttack = false;
            CancelAutoHunt();
        }
        
    }

    /// <summary>
    /// 거리 상관없이 퀘스트 자동진행
    /// </summary>
    public void SetAutoQuest(Transform target)
    {
        _targets.Clear();
        CancelAutoHunt(); //중간에 다른 행동을 하고 있었을때 캔슬
        if (!IsDead)
        {
            if (!IsQuest)
            {
                QuestFromDistance(target.gameObject.layer == LayerMask.NameToLayer("NPC") ? TalkNpc : SetAutoHunt, target);
                IsQuest = true;
            }
            else //퀘스트 자동진행 중에 한번더 클릭 될 경우 진행 취소
            {
                StopMoveCo();
                IsQuest = false; 
            }
            QuestManager.Instance.SetAniQuest(IsQuest);
        }
    }

    private void TalkNpc()
    {
        QuestManager.Instance.CheckNpcQuest(MapManager.Instance.TargetNpc.ID);
    }

    /// <summary>
    /// 기본 공격 사용
    /// </summary>
    public void UseNormalAttack()
    {
        if (!IsAttack && !IsDead)
        {
            if (_targets.Count != 0)
            {
                ActionFromDistance(NormalAttack, _targets[0]);
            }
            else
            {
                CheckAttackRange(1, NormalAttack);
            }
        }
    }

    /// <summary>
    /// 거리 상관없이 퀘스트 자동진행
    /// </summary>
    private void QuestFromDistance(UseActionType useActionType, Transform target)
    {
        MoveTowardTarget(useActionType, target);
    }

    /// <summary>
    /// 타겟(NPC or Enemy)과의 거리에 따른 행동패턴
    /// </summary>
    protected void ActionFromDistance(UseActionType useActionType, Transform target)
    {
        if (Math.Pow(_attackRadius, 2) < (transform.position -target.position).sqrMagnitude) //타겟이 공격사거리 밖에있을때
        {
            MoveTowardTarget(useActionType, target);
        }
        else //공격 사거리 안에 있을때
        {
            useActionType();
        }
    }

    private void MoveTowardTarget(UseActionType useActionType, Transform target)
    {
        if (_moveCo == null) //버튼이 여러번 눌렸을때 코루틴 중복 방지
        {
            _moveCo = MoveTowardTargetCo(useActionType, target);
            StartCoroutine(_moveCo);
        } 
    }

    private void StopMoveCo()
    {
        IsAttack = false;
        if (_moveCo != null)
        {
            SetMoveAnim(0);
            _nav.isStopped = false;
            _nav.enabled = false;
            StopCoroutine(_moveCo);
            _moveCo = null;
        }
    }

    /// <summary>
    /// 탐색 사거리 안의 적이 공격사거리 보다 클 때 공격 사거리 안에 들어올때까지 해당 적에게 이동
    /// </summary>
    protected IEnumerator MoveTowardTargetCo(UseActionType useActionType, Transform target)
    {
        float goalRadius;

        if (target.gameObject.layer == LayerMask.NameToLayer("NPC")) //NPC가 도착목표일 경우 좀더 가깝게 잡음
        {
            goalRadius = 2;
        }
        else
        {
            goalRadius = _attackRadius;
        }
        
        _nav.enabled = true;
        SetMoveAnim(1);
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        WaitForFixedUpdate delay = new WaitForFixedUpdate();
        while (true)
        {
            if (goalRadius >= Vector3.Distance(transform.position, target.position)) //공격 사거리 안에 들어왔을때
            {
                useActionType();
                SetMoveAnim(0);
                _nav.isStopped = true;
                _nav.enabled = false;
                _moveCo = null;
                break;
            }
            _nav.SetDestination(target.transform.position);
            yield return delay;
        }
    }


    /// <summary>
    /// 기본공격을 사용될때 실제로 동작되는 기능
    /// </summary>
    private void NormalAttack()
    {
        if (_targets.Count != 0)
        {
            IsAttack = true;
            transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
            _animator.SetInteger(Global.NormalAttackInteger, _comboCount++ % Global.MaxComboAttack);
            _canNextNormalAttack = false;
        }
    }


    /// <summary>
    /// 기본 공격의 애니메이션 단계를 처음으로 초기화
    /// </summary>
    public void InitNormalAttack()
    {
        _animator.SetInteger(Global.NormalAttackInteger, Global.InitAttackCount);
        InitAttack();

        _normalAttackCancelDelay = 1f;
        _canNextNormalAttack = true;
    }

    /// <summary>
    /// 공격 가능한 상태로 초기화
    /// </summary>
    public void InitAttack()
    {
        IsAttack = false;
    }

    /// <summary>
    /// 콤보를 초기화
    /// </summary>
    private void CheckInitCombo()
    {
        _normalAttackCancelDelay -= Time.deltaTime;
        if (_normalAttackCancelDelay <= 0)
        {
            _comboCount = 0;
            _canNextNormalAttack = false;
        }
    }

    public void CancelAutoQuest()
    {
        if (IsQuest)
        {
            IsQuest = false;
            QuestManager.Instance.SetAniQuest(IsQuest);
        }
    }

    public void ActiveFootPrinters(bool active)
    {
        foreach (var foot in _foots)
        {
            foot.ActiveFoot(active);
        }
    }

    protected override void Die()
    {
        base.Die();
        _animator.SetTrigger(Global.DeadTrigger);
        //todo: 플레이어가 죽었을때 처리 애니메이션 뒤에 넣어주는게 좋을듯(페이드인 아웃 ->마을 부활, 체력 회복 등)
    }

    public void DeleteTarget(Transform target)
    {
        if (_targets.Contains(target))
            _targets.Remove(target);
    }
    
    /// <summary>
    /// 자동사냥 취소 버튼 생성
    /// </summary>
    private void ActiveAutoCancelButton(bool isActive)
    {
        _autoCancelButton.SetActive(isActive);
    }
}