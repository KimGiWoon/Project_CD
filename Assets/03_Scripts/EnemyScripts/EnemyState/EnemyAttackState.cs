using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float _attackDelay;

    public EnemyAttackState(EnemyController Contoller, EnemyStateMachine stateMachine, float attackDelay) : base(Contoller, stateMachine)
    {
        _attackDelay = attackDelay;
    }

    public override void Enter()
    {
        _enemyContoller.MoveStop();

        // 공격
        _enemyContoller.Attack();
    }

    public override void Update()
    {
        float timer = 0f;

        timer += Time.deltaTime;

        // 타겟이 없으면 PatrolState 상태로 전환
        if (!_enemyContoller.HasTarget)
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyPatrolState);
            return;
        }

        // 타겟이 있으나 공격 범위 밖이면 TraceState 상태로 전환
        if (!_enemyContoller.IsTracePossible())
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyTraceState);
            return;
        }

        // 쿨타임 경과 후 재공격
        if ( timer >= _attackDelay)
        {
            timer = 0f;

            // 공격
            _enemyContoller.Attack();
        }
    }

    public override void Exit()
    {
        
    }
}
