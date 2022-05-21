using UnityEngine;

public class EventRaise : MonoBehaviour
{
    [SerializeField,Tooltip("L'event qui va être déclencher lorsque l'obj est trigger")]
    private EventsTrigger m_event;

    [SerializeField,Tooltip("True : Apparistion / False :  Disparission")]
    private bool m_bool;

    private void OnTriggerEnter(Collider other)
    {
        if (m_event == null) return;
        
        m_event.Raise(m_bool);
        gameObject.SetActive(false);
    }
}