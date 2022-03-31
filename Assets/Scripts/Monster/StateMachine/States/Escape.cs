using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Escape : BaseState
{
    private MonsterSM m_sm;
    private bool m_isInvisible;
    private Vector3 m_remoteWaypoint;
    private float m_radiusDetectionPrev;
    private CapsuleCollider m_collider;
    private WaitForSeconds m_waitSecond = new WaitForSeconds(3f);

    public Escape(MonsterSM p_stateMachine) : base("Escape", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }
    
     public override void Enter()
    {
        Debug.Log("ESCAPE");
        m_isInvisible = false;
        m_sm.m_radiusDetection /= 1.5f;
        m_sm.m_navMeshAgent.speed *= 2;

        m_collider = m_sm.GetComponent<CapsuleCollider>();
        
        //Chercher le waypoint le plus éloigner
        m_remoteWaypoint = LocateRemoteWaypoint(PlayerManager.Instance.transform.position);
        m_sm.m_navMeshAgent.SetDestination(m_remoteWaypoint);
    }

    public override void UpdateFunction()
    {
        //Si la vision entre le monstre et le joueur est caché je disparait
        if (Vector3.Distance(m_sm.transform.position, m_remoteWaypoint) < 5 && !m_isInvisible)
        {
            float m_distPlayer = Vector3.Distance(m_remoteWaypoint, PlayerManager.Instance.transform.position);
            Ray ray = new Ray(PlayerManager.Instance.transform.position, (m_remoteWaypoint - PlayerManager.Instance.transform.position).normalized * m_distPlayer);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, m_distPlayer))
            {
                m_isInvisible = true;
                HideMonster();
            }
            Debug.DrawRay(PlayerManager.Instance.transform.position,(m_remoteWaypoint - PlayerManager.Instance.transform.position).normalized * m_distPlayer,Color.red);
        }
    }

    private void HideMonster()
    {
        Debug.Log("disable monster");
        m_radiusDetectionPrev = m_sm.m_radiusDetection;
        
        m_sm.m_radiusDetection = 0;
        
        m_collider.enabled = false;
        
        m_sm.transform.GetChild(0).transform.gameObject.SetActive(false);
        
        m_sm.StartCoroutine(DisplayeMonster());
    }
    
    IEnumerator DisplayeMonster()
    {
        yield return m_waitSecond;
        Debug.Log("enable monster");
        
        //Se TP
        m_collider.enabled = true;
        
        m_sm.m_radiusDetection = m_radiusDetectionPrev;
        
        m_sm.transform.GetChild(0).transform.gameObject.SetActive(true);
        
        m_sm.transform.position = LocateRemoteWaypoint(PlayerManager.Instance.transform.position);
        
        //Retour en patrol
        m_sm.NextState(m_sm.m_patrol);
    }

    private Vector3 LocateRemoteWaypoint(Vector3 p_targetRemote, bool p_isRaycast = false )
    {
        Vector3 chooseWayPoint = p_targetRemote;
        Vector3 remotePos = new Vector3();

        for (int i = 0; i < m_sm.m_waypointsArray.Count; i++)
        {
            float distPlayer = Vector3.Distance(chooseWayPoint,p_targetRemote);
            float newDistPlayer = Vector3.Distance(m_sm.m_waypointsArray[i].transform.position,p_targetRemote);
            
            if (newDistPlayer > distPlayer)
            {
                remotePos = m_sm.m_waypointsArray[i].transform.position;
                
                if (p_isRaycast)
                {
                    Ray ray = new Ray(m_sm.m_waypointsArray[i].transform.position,
                        (m_sm.m_waypointsArray[i].transform.position - p_targetRemote).normalized * newDistPlayer);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, newDistPlayer))
                    {
                        if ((m_sm.m_layerPlayer.value & (1 << hit.collider.gameObject.layer)) <= 0)
                        {
                            chooseWayPoint = m_sm.m_waypointsArray[i].transform.position;
                        }
                        else if(chooseWayPoint == p_targetRemote)
                        {
                            chooseWayPoint = remotePos;
                        }
                    }
                }
                else
                {
                    chooseWayPoint = remotePos;
                }
            }
        }
        return chooseWayPoint;
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
        m_sm.m_navMeshAgent.speed /= 2;
        m_sm.m_radiusDetection *= 1.5f;
    }
}