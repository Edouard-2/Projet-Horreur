using System.Collections;
using UnityEngine;

public class Defense : BaseState
{
    private MonsterSM m_sm;
    private bool m_canHook;
    private bool m_canPatrol;
    private WaitForSeconds m_waitSecondHook = new WaitForSeconds(5f);
    private WaitForSeconds m_waitSecondPatrol = new WaitForSeconds(5f);

    public Defense(MonsterSM p_stateMachine) : base("Defense", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }
    
     public override void Enter()
    {
        Debug.Log("Defense");
        
        //Init variables
        m_canPatrol = false;
        m_canHook = false;
        
        m_sm.m_radiusDetection /= 1.5f;
        m_sm.m_navMeshAgent.speed *= 2;
        //Arrete de bouger
        m_sm.m_navMeshAgent.SetDestination(m_sm.transform.position);

        m_sm.StartCoroutine(StartHook());
        m_sm.StartCoroutine(StartPatrol());
    }

    IEnumerator StartHook()
    {
        yield return m_waitSecondHook;
        m_canHook = true;
    }
    IEnumerator StartPatrol()
    {
        yield return m_waitSecondPatrol;
        m_canPatrol = true;
    }

    public override void UpdateLogic()
    {
        if (m_canPatrol && PlayerManager.Instance.m_visionScript.m_isBlurVision == 1)
        {
            m_sm.NextState(m_sm.m_patrol);
        }
    }

    public override void UpdateFunction()
    {
        if (m_canHook && PlayerManager.Instance.m_visionScript.m_isBlurVision == 1)
        {
            m_sm.m_patrol.UpdateLogic();
        }
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
        m_sm.m_navMeshAgent.speed /= 2;
        m_sm.m_radiusDetection *= 1.5f;
    }
}