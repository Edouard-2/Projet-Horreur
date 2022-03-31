using UnityEngine;
public class Escape : BaseState
{
    private MonsterSM m_sm;

    public Escape(MonsterSM p_stateMachine) : base("Escape", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }
    
     public override void Enter()
    {
        Debug.Log("ESCAPE");
    }

    public override void UpdateLogic()
    {
        if (PlayerManager.Instance.m_visionScript.m_isBlurVision == 1)
        {
            m_sm.NextState(m_sm.m_patrol);
        }
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
    }
}