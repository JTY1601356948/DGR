using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity     //中心管理代码
{
    //[Header("Attack details")]
    //public Vector2[] attackMovement;
    //public bool isBusy {  get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {  get; private set; }
    

    public PlayerStateMachine stateMachine { get; private set; }        //公开只读的状态机属性，用于管理状态切换
    public PlayerIdleState idleState { get; private set; }      //公开的空闲状态和移动状态属性，预定义n种基础状态
    public PlayerMoveState moveState { get; private set; }

    public PlayerDashState dashState { get; private set; }
    //public PlayerPrimaryAttackState primaryAttack { get; private set; }

    protected override void Awake()         //public任何地方都可以访问（无限制）     protected仅允许当前类?或其派生类??访问（受保护）
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();        //创建状态机实例
        idleState =new PlayerIdleState(this,stateMachine,"Idle");       //初始化n种状态
        moveState = new PlayerMoveState(this, stateMachine, "Move");

        dashState = new PlayerDashState(this, stateMachine, "Dash");
        //primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");

    }
    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(idleState);     //设置初始状态为空闲状态

    }

    

    protected override void Update()       //尽量减少update中的事件，因为每帧调用会降低性能
    {
        base.Update();


        stateMachine.currentState.Update();     //更新当前状态机状态

        CheckForDashInput();
        //StartCoroutine("BusyFor", .1f);
    }

    //public IEnumerator BusyFor(float _seconds)
    //{
    //    isBusy = true;
    //    yield return new WaitForSeconds(_seconds);
    //    isBusy=false;
    //}

    public void AnimationTrigger()=>stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        
        dashUsageTimer -= Time.deltaTime;

        if(IsWallDetected()) 
            return;

        if(Input.GetKey(KeyCode.LeftShift) &&dashUsageTimer<0)
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if(dashDir == 0)
                dashDir=facingDir;

            stateMachine.ChangeState(dashState);
        }
    }
    


}
