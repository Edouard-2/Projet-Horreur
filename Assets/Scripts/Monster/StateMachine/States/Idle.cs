using UnityEngine;

public class Idle : BaseState
{
    private MonsterSM m_sm;
    public Idle(MonsterSM p_stateMachine) : base("Idle", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Idle");
    }

    public override void UpdateFunction()
    {
        m_sm.m_patrol.UpdateLogic();
    }
}