using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // 대기 상태에서 이동 정지
        _playerController.StopMove();
    }

    public override void Update()
    {
        // 입력이 있으면 MoveState로 상태 전환
        if (_playerController.HasMoveInput)
        {
            _playerStateMachine.ChangeState(_playerController.PlayerMoveState);
        }
    }

    public override void FixedUpdate()
    {
        _playerController.StopMove();
    }

    public override void Exit()
    {
        
    }
}
