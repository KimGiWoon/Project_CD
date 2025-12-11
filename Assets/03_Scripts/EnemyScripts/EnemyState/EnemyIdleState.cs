using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyController Contoller, EnemyStateMachine stateMachine) : base(Contoller, stateMachine)
    {
    }

    public override void Enter()
    {
        _enemyContoller.MoveStop();
    }

    public override void Update()
    {
        // 죽으면 DeadState 상태로 전환
        if (_enemyContoller.IsDead)
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyDeadState);
            return;
        }

        // 타겟이 있고 추적이 가능하면 TraceState로 상태 전환
        if (_enemyContoller.HasTarget && _enemyContoller.IsTracePossible())
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyTraceState);
            return;
        }

        // 타겟이 없으면 PatrolState 상태로 전환
        _enemyStateMachine.ChangeState(_enemyContoller.EnemyPatrolState);
    }

    public override void Exit()
    {
        
    }
}
