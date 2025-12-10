using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyBaseState _currentState { get; private set; } // 현재 상태

    // 상태 초기화
    public void Init(EnemyBaseState state)
    {
        // 현재 상태 설정
        _currentState = state;
        _currentState?.Enter();
    }

    // 상태 변경
    public void ChangeState(EnemyBaseState changeState)
    {
        // 현재 상태와 변경 요청 상태가 같으면 실행하지 않음
        if(_currentState == changeState) return;

        _currentState?.Exit();   // 현재 상태 끝
        _currentState = changeState;    // 변경 요청 상태를 현재 상태로 변경
        _currentState?.Enter();  // 현재 상태 시작
    }

    // 상태 동작
    public void Update()
    {
        _currentState?.Update();
    }

    // 상태 물리 동작
    public void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }
}
