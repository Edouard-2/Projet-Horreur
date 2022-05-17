using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField, Tooltip("Le script Door Parent")]
    private Door m_door;
    
    private void OnTriggerEnter(Collider other)
    {
        if ((m_door.m_layerMonstre.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            return;
        }
        m_door.CloseDoor(other.transform);
    }
}
