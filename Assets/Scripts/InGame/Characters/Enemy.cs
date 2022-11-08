using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public enum EnemyType
{
    Spider,
    FrightFly,
    ForestGolem,
    SpecialGolem,
    GoblinWarrior,
    GoblinArcher,
    Goblin
}

public abstract class Enemy : Creature
{
    public BoxCollider SpawnArea { get; set; }
    
    [Header("스폰지점으로 돌아가기 전 까지 거리")]
    [SerializeField] private float _backDistance;

    [Header("스폰이나 사망때 사용 될 디졸브")]
    [SerializeField] private Dissolve _dissolve;
    
    /// <summary>
    /// 스폰 지점에서 나왔을때의 위치
    /// </summary>
    private Vector3 _outVector; 

    /// <summary>
    /// 스폰 지점에서 나왔는지
    /// </summary>
    private bool _isOutArea; 
    
    /// <summary>
    /// 되돌아 가는 중인지
    /// </summary>
    private bool _isGoBack; 
    
    public EnemyType _curEnemyType;
    
    /// <summary>
    /// 플레이어를 추적해야 하는지에 대한 변수
    /// </summary>
    protected bool _isFollow; 
    
    protected bool _isAttack;

    /// <summary>
    /// 플레이어가 공격하지 않았을때 상태
    /// </summary>
    private bool _isWait; 

    protected virtual void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        _isFollow = false;
        _isAttack = false;
        _isGoBack = false;
        _isOutArea = false;
        _isWait = true;
    }
    
    protected override void Awake()
    {
        base.Awake();
        Stat = new Stat(_curEnemyType);
    }
    
    
    private void Start()
    {
        _targets.Add(DataManager.Instance.Player.transform);
    }

    private void Update()
    {
        if (!IsDead)
        {
            if (_isOutArea)
            {
                if (_backDistance < Vector3.Distance(transform.position, _outVector)) //일정 거리 이상 스폰지역 밖으로 나왔을때
                {
                    StartCoroutine(BackToArea());
                }
                else //추적하던 적이 사라졌을때
                {
                    if (IsNullPlayer())
                    {
                        StartCoroutine(BackToArea());
                    }
                }
            }

            if (_isWait)
            {
                SetRandomMove();
                _isWait = false;
            }
        }
    }
    
    

    private void FixedUpdate()
    {
        if (!IsDead)
        {
            DoPattern();
            FreezeVelocity();
        }
    }

    /// <summary>
    /// 대기상태일때 랜덤 패턴 설정(이동, 멈춤)
    /// </summary>
    private void SetRandomMove()
    {
        int state = Random.Range(1, 3);
        switch (state)
        {
            case 1:
                _nav.isStopped = true;
                SetAnimations(0, 0, 0, 0,0,0);
                Invoke("SetRandomMove", Random.Range(3, 7));
                break;
            case 2:
                StartCoroutine(NextMoveCo());
                break;
        }
    }

    private IEnumerator NextMoveCo()
    {
        _nav.isStopped = false;
        _nav.SetDestination(RandomBackPos());
        SetAnimations(3, 1, 1, 1,1,1);
        while (true)
        {
            if (_nav.remainingDistance <= 0.1f)
            {
                Invoke("SetRandomMove", 1);
                SetAnimations(0, 0, 0, 0,0,0);
                break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 스폰지점 안의 범위에서 랜덤 위치 반환
    /// </summary>
    private Vector3 RandomBackPos()
    {
        Vector3 originPos = SpawnArea.transform.position;
        float width = SpawnArea.bounds.size.x;
        float height = SpawnArea.bounds.size.z;

        float randomX = Random.Range((width / 2) * -1, width / 2);
        float randomZ = Random.Range((height / 2) * -1, height / 2);

        Vector3 randomPos = new Vector3(randomX, 0, randomZ);

        Vector3 backPos = randomPos + originPos;
        return backPos;
    }

    /// <summary>
    /// 스폰지역에서 멀어질때 OR 적이 없어질때 다시 제자리로 돌아감
    /// </summary>
    private IEnumerator BackToArea()
    {
        _isFollow = false;
        _isOutArea = false;
        _nav.SetDestination(RandomBackPos()); 
        _nav.isStopped = false;
        _isGoBack = true;
        while (true)
        {
            if (_nav.remainingDistance <= 0.5f) //도착했을때
            {
                if(_curEnemyType is EnemyType.ForestGolem or EnemyType.SpecialGolem)
                    _animator.SetInteger(Global.EnemyAttackInteger, -1);

                SetAnimations(0, 0, 0, 0,0,0);
                _isWait = true;
                _nav.isStopped = true;
                _isGoBack = false;
                Stat.Hp = Stat.MaxHp; //복귀시 체력 전체회복
                break;
            }

            if (IsDead) //도착하기 전에 죽게되는 경우
            {
                _nav.isStopped = true;
                break;
            }

            SetAnimations(3, 0, 1, 1,1,1);
            yield return null;
        }
    }
    
    /// <summary>
    /// Rigidbody의 Speed나 다른 값의 변화 방지
    /// </summary>
    private void FreezeVelocity()
    {
        _rigid.angularVelocity = Vector3.zero;
        _rigid.velocity = Vector3.zero;
    }

    private void DoPattern()
    {
        if (!_isAttack) //공격중이 아닐때에만 행동
        {
            if (_isFollow)
            {
                transform.LookAt(_targets[0]);
                if (_attackRadius < Vector3.Distance(transform.position, _targets[0].position)) //타겟이 공격사거리 밖에있을때
                {
                    SetAnimations(1, 1, 1, 1,1,1);
                    _nav.isStopped = false;
                    _nav.SetDestination(_targets[0].transform.position);
                }
                else
                {
                    SetAnimations(0, 0, 0, 0,0,0);
                    _nav.isStopped = true;
                    Attack();
                }
            }
        }
    }

    /// <summary>
    /// 플레이어가 없는지 체크
    /// </summary>
    private bool IsNullPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRadius * 6, LayerMask.GetMask("Player"));
        if (colliders.Length == 0)
        {
            return true;
        }

        return false;
    }
    protected abstract void Attack();
  

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (!_isFollow && !_isGoBack) //피격 당했을 때, 되돌아 가는 중이 아닐 때 추적 시작
        {
            CancelInvoke("SetRandomMove");
            _isFollow = true;
        }
    }

    protected override void Die()
    {
        base.Die();
        _nav.isStopped = true;
        _animator.SetTrigger(Global.EnemyDeadTrigger);
        QuestManager.Instance.CheckEnemyQuest(_curEnemyType);
        DataManager.Instance.Player.Targets.Remove(this.transform);
    }

    /// <summary>
    /// 죽었을때 풀링 반환
    /// </summary>
    public abstract void DisableEnemy();

    public void ActiveDeadEffect()
    {
        _dissolve.FadeOut();
        Invoke("DisableEnemy", 1);
    }

    /// <summary>
    /// 각 적 타입마다 애니메이션 셋팅
    /// </summary>
    private void SetAnimations(int spider, int frightFly, int forestGolem, int specialGolem, int goblinWarrior, int goblinArcher)
    {
        switch (_curEnemyType)
        {
            case EnemyType.Spider:
                _animator.SetInteger(Global.EnemyStateInteger,spider);
                break;
            case EnemyType.FrightFly:
                _animator.SetInteger(Global.EnemyStateInteger,frightFly);
                break;
            case EnemyType.ForestGolem:
                _animator.SetInteger(Global.EnemyStateInteger,forestGolem);
                break;
            case EnemyType.SpecialGolem:
                _animator.SetInteger(Global.EnemyStateInteger,specialGolem);
                break;
            case EnemyType.GoblinWarrior:
            case EnemyType.Goblin:
                _animator.SetInteger(Global.EnemyStateInteger,goblinWarrior);
                break;
            case EnemyType.GoblinArcher:
                _animator.SetInteger(Global.EnemyStateInteger,goblinWarrior);
                break;
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpawnArea")) //스폰 지역에서 벗어났을때
        {
            _isOutArea = true;
            _outVector = transform.position;
        }
    }
    

    
}
