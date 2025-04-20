using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine        
{
    // 当前激活状态（只读属性，外部只能获取不能直接修改）
    public PlayerState currentState{ get; private set; }

    public void Initialize(PlayerState _startstate)
    {
        currentState = _startstate;     // 设置初始状态
        currentState.Enter();       // 执行初始状态的进入逻辑
    }

    public void ChangeState(PlayerState _newstate)
    {
        currentState.Exit();        // 退出当前状态
        currentState = _newstate;       // 更新当前状态引用
        currentState.Enter();       // 执行新状态的进入逻辑
    }
}
