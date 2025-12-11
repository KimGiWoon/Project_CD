using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    [Header("Enemy Data SO")]
    [SerializeField] private EnemyDataSO _enemyDataSO;

    [Header("Trace Setting")]
    [SerializeField] private Transform _traceTarget;
    [SerializeField] private float _traceInterval = 0.1f;

    [Header("Attack Setting")]
    [SerializeField] private float _attackCooltime = 1f;

    // 스탯 데이터
    private int _currentHp;
    private float _moveSpeed;
    private float _attackDamage;
    private float _attackDuration;
    private float _attackRange;
    private float _traceRange;

    private Rigidbody2D _rigid;
    private EnemyStateMachine _enemyStateMachine;
    private float _traceTimer;

    // 죽음 여부
    public bool IsDead => _currentHp <= 0;
    // 타겟 여부
    public bool HasTarget => _traceTarget != null;

    // 제곱 거리 비교 프로퍼티
    public float AttackRangeSqr => _attackRange * _attackRange;
    public float TraceRangeSqr => _traceRange * _traceRange;

    public Transform Target => _traceTarget;
    public float AttackCoolTime => _attackCooltime;

    // 상태 인스턴스 보관
    public EnemyIdleState EnemyIdleState { get; private set; }
    public EnemyTraceState EnemyTraceState { get; private set; }
    public EnemyAttackState EnemyAttackState { get; private set; }
    public EnemyDeadState EnemyDeadState { get; private set; }
    public EnemyPatrolState EnemyPatrolState { get; private set; }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();

        // 타겟이 없으면 찾기
        if (_traceTarget == null)
        {
            // 플레이어를 타겟으로 지정
            GameObject target = GameObject.FindGameObjectWithTag("Player");

            if (target != null)
            {
                _traceTarget = target.transform;
            }
        }

        // 스탯 초기화
        InitEnemyState();
        // 상태 머신 초기화
        InitStateMachine();
    }

    // 스탯 초기화
    private void InitEnemyState()
    {
        if (_enemyDataSO == null)
        {
            Debug.Log("EnemyDataSO가 참조 되어 있지 않습니다.");

            // 기본값 적용
            _currentHp = 1;
            _moveSpeed = 1f;
            _attackDamage = 1f;
            _attackDuration = 2f;
            _attackRange = 1f;
            _traceRange = 1f;

            return;
        }

        _currentHp = _enemyDataSO.MaxHp;
        _moveSpeed = _enemyDataSO.MoveSpeed;
        _attackDamage = _enemyDataSO.AttackDamage;
        _attackDuration = _enemyDataSO.AttackDuration;
        _attackRange = _enemyDataSO.AttackRange;
        _traceRange = _enemyDataSO.TraceRange;
    }

    // 상태 머신 초기화
    private void InitStateMachine()
    {
        // 상태 머신 생성
        _enemyStateMachine = new EnemyStateMachine();

        // 상태 인스턴스 생성하면서 데이터 넘겨주기
        EnemyIdleState = new EnemyIdleState(this, _enemyStateMachine);
        EnemyTraceState = new EnemyTraceState(this, _enemyStateMachine);
        EnemyAttackState = new EnemyAttackState(this, _enemyStateMachine, _attackDuration);
        EnemyDeadState = new EnemyDeadState(this, _enemyStateMachine);
        EnemyPatrolState = new EnemyPatrolState(this, _enemyStateMachine);
    }

    private void Start()
    {
        // 처음 Idle상태로 초기화
        _enemyStateMachine.Init(EnemyIdleState);
        // 상태 판단 시간 초기화
        _traceTimer = _traceInterval;
    }

    private void Update()
    {
        if (IsDead) return;

        _traceTimer -= Time.deltaTime;

        if (_traceTimer <= 0f)
        {
            // 판단 시간 초기화
            _traceTimer = _traceInterval;

            _enemyStateMachine?.Update();
        }
    }

    private void FixedUpdate()
    {
        if (IsDead) return;

        _enemyStateMachine?.FixedUpdate();
    }

    // 움직임 정지
    public void MoveStop()
    {
        _rigid.velocity = Vector2.zero;
    }

    // 타겟의 방향
    public Vector2 GetTargetPos()
    {
        if (_traceTarget == null) return Vector2.zero;

        return (Vector2)((_traceTarget.position - transform.position).normalized);
    }

    // 타겟 방향으로 이동
    public void TargetMovement()
    {
        // 타겟이 없으면 정지
        if (_traceTarget == null)
        {
            MoveStop();
            return;
        }

        // 타겟으로 이동
        Vector2 traceDir = GetTargetPos();
        _rigid.velocity = traceDir * _moveSpeed;
    }

    // 임의의 방향으로 이동
    public void PatrolMovement(Vector2 dir)
    {
        _rigid.velocity = dir * _moveSpeed;
    }

    // 추적 가능 체크
    public bool IsTracePossible()
    {
        // 타겟이 없으면 추적 불가
        if (_traceTarget == null) return false;

        Vector2 diff = _traceTarget.position - transform.position;
        return diff.sqrMagnitude <= TraceRangeSqr;
    }

    // 공격 가능 체크
    public bool IsAttackPossible()
    {
        // 타겟이 없으면 공격 불가
        if (_traceTarget == null) return false;

        Vector2 diff = _traceTarget.position - transform.position;
        return diff.sqrMagnitude <= AttackRangeSqr;
    }

    // 공격
    public void Attack()
    {

    }

    // 데미지 받음
    public void TakeDamage(int damage)
    {
        // 현재 체력이 -로 떨어지지 않게 조정
        _currentHp = Mathf.Max(_currentHp - damage, 0);
        Debug.Log($"몬스터가 공격을 받았습니다! 남은 체력 : {_currentHp}/{_enemyDataSO.MaxHp}");

        if (IsDead)
        {
            Death();
        }
    }

    // 죽음
    private void Death()
    {
        // DeadState 상태로 전환
        _enemyStateMachine.ChangeState(EnemyDeadState);
    }

}
