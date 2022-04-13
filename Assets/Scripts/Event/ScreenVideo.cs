using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class ScreenVideo : MonoBehaviour
{
    [SerializeField, Tooltip("Event vidéo qu'on qui nous déclenchera")]
    private EventsTrigger m_event;

    [SerializeField, Tooltip("Le component vidéo de l'obj")]
    private VideoPlayer m_videoPlayer;
    
    [SerializeField, Tooltip("La vidéo qui sera afficher lorque le trigger est déclenché")]
    private VideoClip m_newVideoClip;

    private VideoClip m_baseVideoClip;
    
    private WaitForSeconds m_waitUntilReplaceVideo;

    private bool m_start = true;
    
    private void OnEnable()
    {
        m_event.OnTrigger += SwitchVideo;
    }

    private void OnDisable()
    {
        m_event.OnTrigger -= SwitchVideo;
    }

    private void Awake()
    {
        m_baseVideoClip = m_videoPlayer.clip;
        
        m_waitUntilReplaceVideo = new WaitForSeconds((float)m_newVideoClip.length);
    }

    private void SwitchVideo(bool p_start = true)
    {
        if (p_start && m_start)
        {
            m_start = false;
            m_videoPlayer.clip = m_newVideoClip;
            m_videoPlayer.frame = 0;
            StartCoroutine(ReplaceVideo());
        }
    }

    IEnumerator ReplaceVideo()
    {
        yield return m_waitUntilReplaceVideo;
        m_videoPlayer.clip = m_baseVideoClip;
        m_videoPlayer.frame = 0;
    }
}
