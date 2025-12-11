using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerController _playerController;   // 플레이어 컨트롤러
    protected PlayerStateMachine _playerStateMachine;   // 플레이어 상태 머신

    // 상태가 생성될 때 플레이어의 컨트롤러와 머신 전달
    protected PlayerBaseState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        _playerController = controller;
        _playerStateMachine = stateMachine;
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
