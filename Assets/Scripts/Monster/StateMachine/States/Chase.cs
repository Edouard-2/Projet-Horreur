using System.Collections;
using System.Xml;
using UnityEngine;

public class Chase : BaseState
{
    private MonsterSM m_sm;
    private bool m_isChasing;
    WaitForSeconds m_waitSecond = new WaitForSeconds(3f);
    
    
    public Chase(MonsterSM p_stateMachine) : base("Chase", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }
    
     public override void Enter()
    {
        Debug.Log("CHASE");
        m_sm.m_navMeshAgent.SetDestination(PlayerManager.Instance.transform.position);
        m_sm.m_navMeshAgent.speed *= 1.2f;
    }

    public override void UpdateLogic()
    {
        if (PlayerManager.Instance.m_visionScript.m_isBlurVision == 0)
        {
            m_sm.NextState(m_sm.m_defense);
        }
    }

    public override void UpdateFunction()
    {
        //LookAt joueur
        m_sm.transform.LookAt(PlayerManager.Instance.transform);
        
        Vector3 vectorPlayerMonster = (PlayerManager.Instance.transform.position - m_sm.transform.position).normalized;

        //Si joueur dans champs de vision 
        Ray ray = new Ray(m_sm.transform.position,vectorPlayerMonster);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_sm.m_radiusVision))
        {
            if ((m_sm.m_layerPlayer.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                //aller vers lui
                m_sm.m_navMeshAgent.SetDestination(PlayerManager.Instance.transform.position);
                m_isChasing = true;
            }
        }
        //Sinon Aller a la dernière position
        else
        {
            m_isChasing = false;
            
            //attendre x sec pour voir si le joueur sort
            m_sm.StartCoroutine(WaitPlayerBeforePatrol());
        }
        
        //Si le joueur est Hookable alors on change de state (Hook)
        m_sm.m_patrol.UpdateLogic();
    }

    IEnumerator WaitPlayerBeforePatrol()
    {
        yield return m_waitSecond;
        
        //partir en mode patrol
        if (!m_isChasing)
        {
            m_sm.NextState(m_sm.m_patrol);
        }
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
        m_sm.m_navMeshAgent.SetDestination(m_sm.transform.position);
        m_sm.m_navMeshAgent.speed /= 1.2f;
    }
}
