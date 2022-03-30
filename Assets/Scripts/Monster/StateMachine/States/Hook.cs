using UnityEngine;
public class Hook : BaseState
{
    private MonsterSM m_sm;

    public Hook(MonsterSM p_stateMachine) : base("Hook", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }
    
     public override void Enter()
    {
        
    }

    public override void UpdateLogic()
    {
        
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
    }
}
