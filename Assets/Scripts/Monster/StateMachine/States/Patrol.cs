using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : BaseState
{
    private List<Transform> m_wayPointsList = new List<Transform>();
    private NavMeshAgent m_navMeshAgent;
    private Transform m_prevWayPoint;
    private Transform m_currentWayPoint;
    private float m_radiusVision;
    private LayerMask m_layerPlayer;
    private MonsterSM m_sm;

    /// <summary>
    /// Constructeur pour le script de patrol du monstre
    /// </summary>
    /// <param name="p_stateMachine"> State Machine du monstre </param>
    /// <param name="p_wayPointsList"> List des différents waypoints </param>
    /// <param name="p_navMeshAgent"></param>
    /// <param name="p_radiusVision"> Radius de vision générale </param>
    /// <param name="p_layerPlayer"> Layer du joueur </param>
    public Patrol(MonsterSM p_stateMachine, List<Transform> p_wayPointsList, NavMeshAgent p_navMeshAgent, float p_radiusVision, LayerMask p_layerPlayer) : base("Patrol", p_stateMachine)
    {
        m_sm = p_stateMachine;
        m_wayPointsList = p_wayPointsList;
        m_navMeshAgent = p_navMeshAgent;
        m_radiusVision = p_radiusVision;
        m_layerPlayer = p_layerPlayer;
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
        
        //Si le joueur regarde le monstre => Hook ou Fuite / => Raycast 
        
        float angleHorizontale = Vector3.Angle(PlayerManager.Instance.transform.forward, -(PlayerManager.Instance.transform.position - m_sm.transform.position).normalized);

        if (Mathf.Abs(angleHorizontale) < 20)
        {
            float angleVertical =Vector3.Angle(PlayerManager.Instance.m_camera.transform.up, m_sm.transform.up);
            if (Mathf.Abs(angleVertical) < 20)
            {
                
                Ray ray = new Ray(m_sm.transform.position,(PlayerManager.Instance.transform.position - m_sm.transform.position).normalized);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, m_radiusVision))
                {
                    if((m_layerPlayer.value & (1 << hit.collider.gameObject.layer)) > 0)
                    {
                        Debug.Log("Je te voix");
                        Debug.DrawRay(m_sm.transform.position,(PlayerManager.Instance.transform.position - m_sm.transform.position).normalized * Vector3.Distance(hit.point,m_sm.transform.position),Color.blue);
                        return;
                    }
                    Debug.Log("Je te voix");
                    Debug.DrawRay(m_sm.transform.position,(PlayerManager.Instance.transform.position - m_sm.transform.position).normalized * Vector3.Distance(hit.point,m_sm.transform.position),Color.red);
                    return;
                }
            }
        }
        Debug.DrawRay(m_sm.transform.position,m_sm.transform.forward * m_radiusVision,Color.white);

        //Si le joueur est dans le champs de vision du monstre => Chase ou Fuite /=> RayCast + verif distance
        //Si le joueur est à côté de nous (très proche = zone de perception) / => Raycast + verif distance
        
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