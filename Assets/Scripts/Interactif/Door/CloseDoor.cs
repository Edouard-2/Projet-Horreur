using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField, Tooltip("Le script Door Parent")]
    private Door m_door;
    
    private void OnTriggerEnter(Collider other)
    {
        m_door.CloseDoor(other.transform);
    }
}
