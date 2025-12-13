using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _timer;
    private Vector2 _dashDir;

    public PlayerDashState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        _timer = 0f;

        // 플레이어의 이동 방향을 대쉬 방향에 대입
        if (_playerController.HasMoveInput)
        {
            _dashDir = _playerController.MoveInput;
        }

        // 대쉬 시작, 쿨타임 적용
        _playerController.StartDash(_dashDir);

        // 대쉬 상태 전환
        _playerController.DashStateChange();

        // TODO: 대쉬 애니메이션 추가
    }

    public override void Update()
    {
        _timer += Time.deltaTime;

        // 대쉬 지속 시간 경과 후 상태 전환
        if (_timer >= _playerController.DashDuration)
        {
            // 대쉬 후 이동 입력이 있으면 MoveState 상태로 전환, 아니면 IdleState 전환
            if (_playerController.HasMoveInput)
            {
                _playerStateMachine.ChangeState(_playerController.PlayerMoveState);
            }
            else
            {
                _playerStateMachine.ChangeState(_playerController.PlayerIdleState);
            }
        }
    }

    public override void FixedUpdate()
    {
        // 대쉬 이동
        _playerController.MoveDash(_dashDir);
    }

    public override void Exit()
    {
        _playerController.StopMove();
    }
}
