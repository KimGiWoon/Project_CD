using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyController Contoller, EnemyStateMachine stateMachine) : base(Contoller, stateMachine)
    {
    }

    public override void Enter()
    {
        // TODO: 사망 애니메이션
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
