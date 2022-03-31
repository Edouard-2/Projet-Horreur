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
        Debug.Log("CHASE");
    }

    public override void UpdateLogic()
    {
        
    }

    public override void UpdateFunction()
    {
        //LookAt joueur
        m_sm.transform.LookAt(PlayerManager.Instance.transform);
        if (PlayerManager.Instance.m_visionScript.m_isBlurVision == 0)
        {
            m_sm.NextState(m_sm.m_escape);
        }
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
    }
}
