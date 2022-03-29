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
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}
