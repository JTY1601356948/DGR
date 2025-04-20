using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected float xInput;
    protected float yInput;

    protected Rigidbody2D rb;

    private string animBoolName;
    protected float stateTimer;
    public bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;       // ע����Ҷ���
        this.stateMachine = _stateMachine;       // ע��״̬��
        this.animBoolName = _animBoolName;       // ��¼����������
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName,true);     // ��������������磺�������״̬ʱ���ſ��ж�����
        rb=player.rb;       //����rb�Ի���
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer-= Time.deltaTime;
        
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName,false);        // �رն������������磺�뿪����״̬ʱֹͣ���ж�����

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled =true;
    }
    
}
