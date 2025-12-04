using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Move Srtting")]
    [SerializeField] private float _moveSpeed = 5f; // 이동 속도

    private Rigidbody2D _rigid;
    private Vector2 _moveInput;
    private PlayerStateMachine _playerStateMachine;

    // 입력 방향 프로퍼티 
    public Vector2 MoveInput => _moveInput;
    // 이동 체크 프로퍼티
    public bool HasMoveInput => _moveInput.sqrMagnitude > 0.01f;

    // 상태 인스턴스 보관
    public PlayerIdleState PlayerIdleState { get; private set; }
    public PlayerMoveState PlayerMoveState { get; private set; }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();

        // 상태 머신 생성
        _playerStateMachine = new PlayerStateMachine();

        // 상태 인스턴스 생성하면서 데이터 넘겨주기
        PlayerIdleState = new PlayerIdleState(this, _playerStateMachine);
        PlayerMoveState = new PlayerMoveState(this, _playerStateMachine);
    }

    private void Start()
    {
        // 처음 Idle상태로 초기화
        _playerStateMachine.Init(PlayerIdleState);
    }

    private void Update()
    {
        ReadInput();

        _playerStateMachine.Update();
    }

    private void FixedUpdate()
    {
        _playerStateMachine.FixedUpdate();
    }

    // 키보드 입력
    private void ReadInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // 입력 벡터 정규화
        _moveInput = new Vector2(x, y).normalized;
    }

    // 실제 이동
    public void MoveVelocity()
    {
        _rigid.velocity = _moveInput * _moveSpeed;
    }

    // 이동 정지
    public void StopMove()
    {
        _rigid.velocity = Vector2.zero;
    }
}
