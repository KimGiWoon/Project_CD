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

        // TODO: Idle 애니메이션 호출
    }

    public override void Update()
    {
        // 이동 입력이 있으면 MoveState로 상태 전환
        if (_playerController.HasMoveInput)
        {
            _playerStateMachine.ChangeState(_playerController.PlayerMoveState);
            return;
        }

        // 공격 입력이 있으면 AttackState로 상태 전환
        if (_playerController.AttackInput)
        {
            _playerStateMachine.ChangeState(_playerController.PlayerAttackState);
            return;
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
