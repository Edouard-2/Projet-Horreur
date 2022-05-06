using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : BaseState
{
    public List<Transform> m_wayPointsList = new List<Transform>();
    
    private Transform m_prevWayPoint;
    public Transform m_currentWayPoint;
    
    private float m_angleVertical;
    private float m_angleHorizontal;
    
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
    /// <param name="p_angleHorizontal"> Angle horizontal de detection du joueur si le monstre est regardé par le joueur </param>
    /// <param name="p_angleVertical"> Angle Vertical de detection du joueur si le monstre est regardé par le joueur </param>
    public Patrol(MonsterSM p_stateMachine,List<Transform> p_wayPointsList, 
        LayerMask p_layerPlayer, float p_angleHorizontal, float p_angleVertical) : base("Patrol", p_stateMachine)
    {
        m_sm = p_stateMachine;
        m_wayPointsList = p_wayPointsList;
        m_layerPlayer = p_layerPlayer;
        m_angleVertical = p_angleVertical;
        m_angleHorizontal = p_angleHorizontal;
    }

    public override void Enter()
    {
        Debug.Log("¨PATROL");
        
        if (m_currentWayPoint == null)
        {
            m_currentWayPoint = GetRandomWayPoint();
        }
        
        m_sm.SetNewAnimation(m_sm.m_movingHash);
        
        m_sm.m_navMeshAgent.SetDestination(m_currentWayPoint.position);
        
    }

    public override void UpdateLogic()
    {
        float angleHorizontale = Vector3.Angle(PlayerManager.Instance.transform.forward, -(PlayerManager.Instance.transform.position - m_sm.transform.position).normalized);
        
        if (Mathf.Abs(angleHorizontale) < m_angleHorizontal)
        {
            float angleVertical =Vector3.Angle(PlayerManager.Instance.m_camera.transform.up, m_sm.transform.up);
            
            if (Mathf.Abs(angleVertical) < m_angleVertical)
            {
                Vector3 vectorPlayerMonster = (PlayerManager.Instance.transform.position - m_sm.transform.position).normalized;

                Ray ray = new Ray(m_sm.transform.position,vectorPlayerMonster);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, m_sm.m_radiusVision))
                {
                    if((m_layerPlayer.value & (1 << hit.collider.gameObject.layer)) > 0)
                    {
                        Debug.Log("Je te voix");
                        
                        if(PlayerManager.Instance.m_visionScript.m_isBlurVision == 0)
                        {
                            m_sm. NextState(m_sm.m_defense);
                            return;
                        }
                        m_sm. NextState(m_sm.m_hook);
                        
                        Debug.DrawRay(m_sm.transform.position,vectorPlayerMonster * Vector3.Distance(hit.point,m_sm.transform.position),Color.blue,1);
                        return;
                    }
                    Debug.DrawRay(m_sm.transform.position,vectorPlayerMonster * Vector3.Distance(hit.point,m_sm.transform.position),Color.red);
                    return;
                }
                Debug.DrawRay(m_sm.transform.position,vectorPlayerMonster * Vector3.Distance(hit.point,m_sm.transform.position),Color.white);

            }
        }
    }

    public override void UpdateFunction()
    {
        if (m_wayPointsList.Count != 0)
        {
            //Debug.Log(Vector3.Distance(m_sm.m_navMeshAgent.transform.position, m_currentWayPoint.position));
            if (Vector3.Distance(m_sm.m_navMeshAgent.transform.position, m_currentWayPoint.position) < 1)
            {
                if (m_currentWayPoint != m_prevWayPoint)
                {
                    m_prevWayPoint = m_currentWayPoint;
                }
                m_currentWayPoint = GetRandomWayPoint();
                
                m_sm.m_navMeshAgent.SetDestination(m_currentWayPoint.position);
                Debug.Log(m_sm.m_navMeshAgent.CalculatePath(m_currentWayPoint.position,m_sm.m_navMeshAgent.path));
            }
        }
        //LookAt joueur
        m_sm.transform.LookAt(PlayerManager.Instance.transform);
    }

    public override void Exit()
    {
        m_sm.m_navMeshAgent.SetDestination(m_sm.transform.position);
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