using System.Collections;
using UnityEngine;

public class OpenDoorMonster : MonoBehaviour
{
    [SerializeField] private Door m_door; 
    [SerializeField] private GameObject m_otherDoor; 
    [SerializeField] private int m_dir; 
    
    private WaitForSeconds m_waitCloseDoorMonster = new WaitForSeconds(1.2f);
    private WaitForSeconds m_waitMonsterAnim = new WaitForSeconds(0.5f);

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Open avec monstre");
        if ( !m_door.m_isOpen &&  (m_door.m_layerMonstre.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            m_otherDoor.SetActive(false);
            m_door.OpenDoor(null);
            MonsterSM.Instance.m_animator.SetTrigger(MonsterSM.Instance.m_retractHash);
            StartCoroutine(CloseDoorAfterMonster());
        }
    }

    IEnumerator CloseDoorAfterMonster()
    {
        yield return m_waitMonsterAnim;
        
        MonsterSM.Instance.m_animator.ResetTrigger(MonsterSM.Instance.m_retractHash);
        MonsterSM.Instance.m_animator.SetTrigger(MonsterSM.Instance.m_movingHash);
        
        yield return m_waitCloseDoorMonster;
        
        m_door.CloseDoor(null);
        m_otherDoor.SetActive(true);
    }
}
