using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("Stats Setting")]
    [SerializeField] private PlayerStats _playerStats;

    [Header("Gun Setting")]
    [SerializeField] private GunController _gunController;

    private Rigidbody2D _rigid;
    private Vector2 _moveInput;
    private PlayerStateMachine _playerStateMachine;

    // 입력 프로퍼티 
    public Vector2 MoveInput => _moveInput;
    public bool HasMoveInput => _moveInput.sqrMagnitude > 0.01f;
    public bool AttackInput { get; private set; }

    // 상태 인스턴스 보관
    public PlayerIdleState PlayerIdleState { get; private set; }
    public PlayerMoveState PlayerMoveState { get; private set; }
    public PlayerAttackState PlayerAttackState { get; private set; }

    // 무기의 공격 딜레이
    public float AttackDelay => _gunController.AttackDelay;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();

        // 스텟 초기화
        if ( _playerStats != null)
        {
            _playerStats.Init();
        }

        // 상태 머신 생성
        _playerStateMachine = new PlayerStateMachine();

        // 상태 인스턴스 생성하면서 데이터 넘겨주기
        PlayerIdleState = new PlayerIdleState(this, _playerStateMachine);
        PlayerMoveState = new PlayerMoveState(this, _playerStateMachine);
        PlayerAttackState = new PlayerAttackState(this, _playerStateMachine);
    }

    private void Start()
    {
        // 처음 Idle상태로 초기화
        _playerStateMachine.Init(PlayerIdleState);
    }

    private void Update()
    {
        PlayerMoveInput();

        _playerStateMachine.Update();
    }

    private void FixedUpdate()
    {
        _playerStateMachine.FixedUpdate();
    }

    // 플레이어 이동 입력
    private void PlayerMoveInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // 입력 벡터 정규화
        _moveInput = new Vector2(x, y).normalized;

        // 공격 입력
        AttackInput = Input.GetMouseButtonDown(0);
    }

    // 플레이어 이동
    public void PlayerMove()
    {
        _rigid.velocity = _moveInput * _playerStats.MoveSpeed;
    }

    // 이동 정지
    public void StopMove()
    {
        _rigid.velocity = Vector2.zero;
    }

    // 총알 발사 처리
    public void Fire()
    {
        _gunController?.BulletFireInput();
    }

    // 데미지 받음
    public void TakeDamage(int damage)
    {
        _playerStats.CurrentHp -= damage;

        // 현재 체력이 0이면 죽음
        if (_playerStats.CurrentHp <= 0)
        {
            _playerStats._isDead = true;
            Death();
        }
    }

    // 죽음
    private void Death()
    {

    }
}
