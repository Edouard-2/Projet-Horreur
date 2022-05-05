using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SubtitleData", fileName = "SubtitleData", order = 3)]
public class TextScriptable : ScriptableObject
{
    [TextArea]
    public List<String> m_listDialogues;
}
