using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolBahaviour : StateMachineBehaviour
{
    public string boolName; // ������ �ִϸ������� bool ���� �̸�
    public bool updateOnState; // ���� ����/���� �� bool ���� ������Ʈ���� ����
    public bool updateOnStateMachine; // ������Ʈ �ӽ� ����/���� �� bool ���� ������Ʈ���� ����
    public bool valueOnEnter, valueOnExit; // ���³� ������Ʈ �ӽſ� ����/������ �� ������ bool ��

    // ���¿� ������ �� ȣ��Ǵ� �޼���
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            // updateOnState�� true�� �� ���� ���� �� bool ���� valueOnEnter�� ����
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // ���¸� ������Ʈ�� �� ȣ��Ǵ� �޼��� (���� ��Ȱ��ȭ��)
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // ���¿��� ���� �� ȣ��Ǵ� �޼���
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            // updateOnState�� true�� �� ���� ���� �� bool ���� valueOnExit���� ����
            animator.SetBool(boolName, valueOnExit);
        }
    }

    // ���¿��� �̵��� �� ȣ��Ǵ� �޼��� (���� ��Ȱ��ȭ��)
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // IK ��� �� ȣ��Ǵ� �޼��� (���� ��Ȱ��ȭ��)
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // ������Ʈ �ӽſ� ������ �� ȣ��Ǵ� �޼���
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine)
            // updateOnStateMachine�� true�� �� ������Ʈ �ӽ� ���� �� bool ���� valueOnEnter�� ����
            animator.SetBool(boolName, valueOnEnter);
    }

    // ������Ʈ �ӽſ��� ���� �� ȣ��Ǵ� �޼���
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine)
            // updateOnStateMachine�� true�� �� ������Ʈ �ӽ� ���� �� bool ���� valueOnExit���� ����
            animator.SetBool(boolName, valueOnExit);
    }
}
