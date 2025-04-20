using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    // Start is called before the first frame update
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }       // 调用基类构造函数

    public override void Enter()
    {
        base.Enter();        // 执行基类动画初始化逻辑

        player.ZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        //if (Input.GetKey(KeyCode.Mouse0))
            //stateMachine.ChangeState(player.primaryAttack);

        if (xInput == player.facingDir && player.IsWallDetected())
            return;
        if (xInput!=0)
            stateMachine.ChangeState(player.moveState);
    }

    public override void Exit()
    {
        base.Exit();        // 执行基类动画清理逻辑


    }
}
