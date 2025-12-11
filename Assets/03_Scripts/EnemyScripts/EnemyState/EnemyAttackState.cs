using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyAttackState : EnemyBaseState
{
    private float _attackDelay;
    private float _attackTime;

    public EnemyAttackState(EnemyController Contoller, EnemyStateMachine stateMachine, float attackDelay) : base(Contoller, stateMachine)
    {
        _attackDelay = attackDelay;
    }

    public override void Enter()
    {
        _attackTime = 0f;
    }

    public override void Update()
    {
        // 죽으면 DeadState 상태로 전환
        if (_enemyContoller.IsDead)
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyDeadState);
            return;
        }

        // 타겟이 없으면 PatrolState 상태로 전환
        if (!_enemyContoller.HasTarget)
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyPatrolState);
            return;
        }

        // 타겟과의 현재 거리
        float distSqr = _enemyContoller.GetTargetDistanceSqr();

        // 타겟이 있으나 공격 범위 밖이면 TraceState 상태로 전환
        if (!_enemyContoller.IsAttackPossible())
        {
            _enemyStateMachine.ChangeState(_enemyContoller.EnemyTraceState);
            return;
        }

        _attackTime += Time.deltaTime;

        // 플레이어 추적
        _enemyContoller.TargetMovement();

        // 쿨타임 경과 후 재공격
        if (_attackTime >= _attackDelay)
        {
            _attackTime = 0f;

            // 공격
            _enemyContoller.Attack();
        }
    }

    public override void Exit()
    {
        
    }
}
