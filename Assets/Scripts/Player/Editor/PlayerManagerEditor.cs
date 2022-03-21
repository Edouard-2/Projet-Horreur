using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerManager))]
public class PlayerManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        PlayerManager myPlayerManager = (PlayerManager)target;
        
        Handles.color = Color.red;
        Handles.DrawWireDisc(myPlayerManager.transform.position,Vector3.up, myPlayerManager.m_radiusVision);
    }
}
