using UnityEngine;

[CreateAssetMenu(fileName = "BasicEvent", menuName = "Event/PosEvent", order = 0)]
public class EventsTriggerPos : ScriptableObject
{
    public delegate void TriggerDelegate(Vector3 p_pos);

    public event TriggerDelegate OnTrigger;

    public void Raise(Vector3 p_pos)
    {
        OnTrigger?.Invoke(p_pos);
    }
}