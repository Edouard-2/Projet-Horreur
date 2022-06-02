using System.Collections;
using UnityEngine;

public class AlertSound : BaseState
{
    private MonsterSM m_sm;
    private WaitForSeconds m_waitPatrol = new WaitForSeconds(3f);
    public Vector3 m_FirstPos;
    
    public AlertSound(MonsterSM p_stateMachine) : base("AlertSound", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Alert");
        m_sm.m_navMeshAgent.speed *= 1.5f;
        
        m_sm.m_screamSound.Play();
        
        m_sm.SetNewAnimation(m_sm.m_bruitHash);
        m_sm.SetNewAnimation(m_sm.m_movingHash);
        
        m_sm.m_navMeshAgent.SetDestination(m_FirstPos);
    }

    public override void UpdateFunction()
    {
        if (Vector3.Distance(m_FirstPos, m_sm.transform.position) < 1.5f)
        {
            m_sm.StartCoroutine(LaunchPatrol());
        }
        
        m_sm.m_patrol.UpdateLogic();
    }

    IEnumerator LaunchPatrol()
    {
        yield return m_waitPatrol;
        m_sm.NextState(m_sm.m_patrol);
    }

    public override void Exit()
    {
        m_sm.m_navMeshAgent.speed /= 1.5f;
    }
}