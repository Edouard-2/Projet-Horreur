using UnityEngine;

public class Chase : BaseState
{
    private MonsterSM m_sm;

    public Chase(MonsterSM p_stateMachine) : base("Chase", p_stateMachine)
    {
        m_sm = p_stateMachine;
        
    }
    
     public override void Enter()
    {
        
    }

    public override void UpdateLogic()
    {
        Debug.Log("CHASE LOL");
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
    }
}
