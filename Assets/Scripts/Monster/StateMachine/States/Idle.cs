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
        m_sm.SetNewAnimation(m_sm.m_retractHash);
    }

    public override void UpdateFunction()
    {
        m_sm.m_patrol.UpdateLogic();
    }

    public override void Exit()
    {
        m_sm.m_navMeshAgent.SetDestination(m_sm.transform.position);
    }
}