using UnityEngine;

[CreateAssetMenu(fileName = "BasicEvent", menuName = "Event/BasicEvent", order = 0)]
public class EventsTrigger : ScriptableObject
{
    public delegate void TriggerDelegate(bool p_isStart = true);

    public event TriggerDelegate OnTrigger;

    public void Raise(bool p_isStart = true)
    {
        OnTrigger?.Invoke(p_isStart);
    }
}
