using UnityEngine;

public class Pause : BaseState
{
    private MonsterSM m_sm;

    public Pause(MonsterSM p_stateMachine) : base("Pause", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("PAUSE");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (GameManager.Instance.State == GameManager.States.LOADING)
        {
            m_sm.NextState(m_sm.m_lastState);
        }
    }

    public override void UpdatePhysics()
    {
        
    }
}
