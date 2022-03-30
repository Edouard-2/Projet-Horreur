using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : BaseState
{
    private List<Transform> m_wayPointsList = new List<Transform>();
    private NavMeshAgent m_navMeshAgent;
    private Transform m_prevWayPoint;
    private Transform m_currentWayPoint;
    private MonsterSM m_sm;

    public Patrol(MonsterSM p_stateMachine, List<Transform> p_wayPointsList, NavMeshAgent p_navMeshAgent) : base("Patrol", p_stateMachine)
    {
        m_sm = p_stateMachine;
        m_wayPointsList = p_wayPointsList;
        m_navMeshAgent = p_navMeshAgent;
    }

    public override void Enter()
    {
        Debug.Log("¨PATROL");
        
        if (m_currentWayPoint == null)
        {
            m_currentWayPoint = GetRandomWayPoint();
        }
        
        m_navMeshAgent.SetDestination(m_currentWayPoint.position);
    }

    public override void UpdateLogic()
    {
        //Mettre le m_ms.NextState(BaseState); Dans cette Update
    }

    public override void UpdateFunction()
    {
        if (m_wayPointsList.Count != 0)
        {
            //Debug.Log(Vector3.Distance(m_navMeshAgent.transform.position, m_currentWayPoint.position));
            if (Vector3.Distance(m_navMeshAgent.transform.position, m_currentWayPoint.position) < 1)
            {
                if (m_currentWayPoint != m_prevWayPoint)
                {
                    m_prevWayPoint = m_currentWayPoint;
                }
                m_currentWayPoint = GetRandomWayPoint();
                
                m_navMeshAgent.SetDestination(m_currentWayPoint.position);
            }
        }
    }

    public override void Exit()
    {
        m_navMeshAgent.SetDestination(m_sm.transform.position);
    }
    
    private Transform GetRandomWayPoint()
    {
        Transform wayPoint;
        
        List<Transform> newListWayPoint = new List<Transform>();

        for (int i = 0; i < m_wayPointsList.Count; i++)
        {
            newListWayPoint.Add(m_wayPointsList[i]);
        }
        
        if (m_currentWayPoint != null && m_wayPointsList.Count != 1 )
        {
            newListWayPoint.Remove(m_currentWayPoint);
            
            if( m_prevWayPoint != null && m_wayPointsList.Count != 2 )
            {
                newListWayPoint.Remove(m_prevWayPoint);
            }
        }
        
        wayPoint = m_wayPointsList[Random.Range(0, m_wayPointsList.Count)];

        return wayPoint;
    }
}