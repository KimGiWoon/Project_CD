using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float _attackDelayTime;

    public PlayerAttackState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        _attackDelayTime = 0f;

        // 총알 발사
        _playerController.Fire();

        // TODO: Attack 애니메이션 호출
    }

    public override void Update()
    {
        _attackDelayTime += Time.deltaTime;

        // 공격 딜레이 시간이 지나면 상태 전환
        if (_attackDelayTime >= _playerController.AttackDelay)
        {
            // 이동 입력이 있으면 MoveState로 상태 전환
            if (_playerController.HasMoveInput)
            {
                _playerStateMachine.ChangeState(_playerController.PlayerMoveState);
            }
            else // 이동 입력이 없으면 IdleState로 상태 전환
            {
                _playerStateMachine.ChangeState(_playerController.PlayerIdleState);
            }
        }
    }

    public override void FixedUpdate()
    {
        // AttackState에서 이동할 수 있도록
        _playerController.PlayerMove();
    }

    public override void Exit()
    {
        
    }
}
