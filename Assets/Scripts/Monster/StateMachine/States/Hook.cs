using UnityEngine;
public class Hook : BaseState
{
    private MonsterSM m_sm;

    public Hook(MonsterSM p_stateMachine) : base("Hook", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }
    
     public override void Enter()
    {
        Debug.Log("HOOK");
    }

    public override void UpdateLogic()
    {
        if (PlayerManager.Instance.m_visionScript.m_isBlurVision == 0)
        {
            m_sm.NextState(m_sm.m_escape);
            return;
        }
        
        Vector3 vectorPlayerMonster = (PlayerManager.Instance.transform.position - m_sm.transform.position).normalized;
        Ray ray = new Ray(m_sm.transform.position,vectorPlayerMonster);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_sm.m_radiusVision))
        {
            if((m_sm.m_layerPlayer.value & (1 << hit.collider.gameObject.layer)) <= 0)
            {
                m_sm.NextState(m_sm.m_chase);
                Debug.DrawRay(m_sm.transform.position,vectorPlayerMonster * Vector3.Distance(hit.point,m_sm.transform.position),Color.red,3);
            }
            Debug.DrawRay(m_sm.transform.position,vectorPlayerMonster * Vector3.Distance(hit.point,m_sm.transform.position),Color.blue);
        }
    }

    public override void UpdateFunction()
    {
        
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
    }
}