using System;
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

    private void OnSceneGUI()
    {
        MonsterSM targetMonster =  (MonsterSM)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(targetMonster.transform.position, Vector3.up, Vector3.forward,360,targetMonster.m_radiusVision);
    }
}