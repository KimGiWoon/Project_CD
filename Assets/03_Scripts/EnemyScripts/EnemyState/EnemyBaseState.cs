using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    protected EnemyController _enemyContoller;  // 적 컨트롤러
    protected EnemyStateMachine _enemyStateMachine; // 적 상태 머신

    // 상태가 생성될 때 적의 컨트롤러와 머신 전달
    protected EnemyBaseState(EnemyController Contoller, EnemyStateMachine stateMachine)
    {
        _enemyContoller = Contoller;
        _enemyStateMachine = stateMachine;
    }

    // 상태가 시작될 때
    public abstract void Enter();

    // 상태에서 동작 담당
    public abstract void Update();

    // 상태에서 물리 동작 담당
    public virtual void FixedUpdate() { }

    // 상태가 끝날 때
    public abstract void Exit();
}
