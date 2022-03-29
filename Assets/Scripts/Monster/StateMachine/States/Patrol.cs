using UnityEngine;

public class Patrol : BaseState
{
    private MonsterSM m_sm;

    public Patrol(MonsterSM p_stateMachine) : base("Patrol", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Debug.Log("PATROL LOL");
        if (Time.time > 5)
        {
            m_sm.NextState(m_sm.m_chase);
        }
    }

    public override void UpdatePhysics()
    {
        
    }
}
