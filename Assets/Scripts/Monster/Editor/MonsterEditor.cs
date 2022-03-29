using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonsterSM))]
public class MonsterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MonsterSM targetMonster =  (MonsterSM)target;
        
        if (GUILayout.Button("Create Waypoint"))
        {
            targetMonster.CreateWayPoint();
        }
        base.OnInspectorGUI();
    }
}