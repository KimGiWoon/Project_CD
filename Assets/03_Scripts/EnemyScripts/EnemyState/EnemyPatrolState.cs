using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    // 패트롤 세팅
    private Vector2 _originPos;
    private Vector2 _patrolTarget;
    private float _patrolRadius = 3f;

    public EnemyPatrolState(EnemyController Contoller, EnemyStateMachine stateMachine) : base(Contoller, stateMachine)
    {
    }

    public override void Enter()
    {
        // 기준점 위치 설정
        _originPos = _enemyContoller.transform.position;

        // 이동 위치 선정
        RandomPatrolTarget();
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

        // 패트롤 목표 지점 이동
        Vector2 currentPos = _enemyContoller.transform.position;
        Vector2 dir = (_patrolTarget - currentPos).normalized;
        _enemyContoller.PatrolMovement(dir);

        // 이동 위치에 근접하면 다른 이동 지점 선정
        if ((_patrolTarget - currentPos).sqrMagnitude < 0.1f * 0.1f)
        {
            RandomPatrolTarget();
        }
    }

    public override void Exit()
    {
        _enemyContoller.MoveStop();
    }

    // 랜덤으로 이동 위치 선정
    private void RandomPatrolTarget()
    {
        // 반경안의 랜덤 위치 설정
        Vector2 targetPos = Random.insideUnitCircle * _patrolRadius;

        // 타겟 지정
        _patrolTarget = _originPos + targetPos;
    }
}
