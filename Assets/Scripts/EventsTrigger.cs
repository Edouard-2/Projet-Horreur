using UnityEngine;

[CreateAssetMenu(fileName = "BasicEvent", menuName = "Event/BasicEvent", order = 0)]
public class EventsTrigger : ScriptableObject
{
    public delegate void TriggerDelegate();

    public event TriggerDelegate OnTrigger;

    public void Raise()
    {
        OnTrigger?.Invoke();
    }
}
