using System;
using System.Collections;
using UnityEngine;

public class Defense : BaseState
{
    private MonsterSM m_sm;
    private bool m_canPatrol;
    
    private WaitForSeconds m_waitSecondPatrol = new WaitForSeconds(10f);

    public Defense(MonsterSM p_stateMachine) : base("Defense", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }
    
     public override void Enter()
    {
        Debug.Log("Defense");
        
        //Init variables
        m_canPatrol = false;
        m_sm.m_patrolEmitter.Stop();
        if(m_sm.m_defenseEmitter != null) m_sm.m_defenseEmitter.Play();
        m_sm.m_collider.radius /= 2;
        
        m_sm.SetNewAnimation(m_sm.m_retractHash);
        
        //Arrete de bouger
        m_sm.m_navMeshAgent.SetDestination(m_sm.transform.position);
        
        m_sm.StartCoroutine(StartPatrol());
        
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

    public override void Exit()
    {
        m_sm.m_lastState = this;
        m_sm.m_collider.radius *= 2;
    }
}