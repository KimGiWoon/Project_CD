using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // TODO: Move 애니메이션 호출
    }   

    public override void Update()
    {
        // 이동 입력이 없으면 IdleState로 상태 전환
        if (!_playerController.HasMoveInput)
        {
            _playerStateMachine.ChangeState(_playerController.PlayerIdleState);
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
        // 실제 이동 
        _playerController.PlayerMove();
    }

    public override void Exit()
    {
        // IdleState 상태로 전환하면 이동 금지
        _playerController.StopMove();
    }
}
