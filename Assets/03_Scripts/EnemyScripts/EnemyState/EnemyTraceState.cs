using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraceState : EnemyBaseState
{
    public EnemyTraceState(EnemyController Contoller, EnemyStateMachine stateMachine) : base(Contoller, stateMachine)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        // 죽으면 DeadState 상태로 전환
        if (_enemyContoller.IsDead)
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyDeadState);
            return;
        }

        // 추적하던 타겟이 없거나 추적을 할 수 없으면 PatrolState 상태로 전환
        if (!_enemyContoller.HasTarget || !_enemyContoller.IsTracePossible())
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyPatrolState);
            return;
        }

        // 공격 범위안에 타겟이 들어오면 AttackState 상태로 전환
        if (_enemyContoller.IsAttackPossible())
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyAttackState);
            return;
        }

        // 플레이어 추적
        _enemyContoller.TargetMovement();
    }

    public override void Exit()
    {

    }
}
