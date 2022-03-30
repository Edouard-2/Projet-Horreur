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
        Debug.Log("PAUSE");
    }
}