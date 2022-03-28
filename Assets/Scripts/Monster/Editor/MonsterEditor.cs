using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonsterManager))]
public class MonsterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        if (GUILayout.Button("Create Waypoint"))
        {
            MonsterManager.Instance.CreateWayPoint();
        }
        base.OnInspectorGUI();
    }
}
