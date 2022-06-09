using System.Collections;
using FMODUnity;
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
    
    private MeshRenderer m_renderer;
    
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
        
        if(m_newVideoClip != null) m_waitUntilReplaceVideo = new WaitForSeconds((float)m_newVideoClip.length);
        
        m_renderer = GetComponent<MeshRenderer>();
        if(m_videoPlayer != null) m_waitUntilReplaceVideo = new WaitForSeconds((float)m_videoPlayer.clip.length);
    }

    private void SwitchVideo(bool p_start = true)
    {
        if (p_start && !m_start)
        {
            if (m_newVideoClip != null)
            {
                m_videoPlayer.clip = m_newVideoClip;
                m_newVideoClip = null;
                m_waitUntilReplaceVideo = new WaitForSeconds((float)m_videoPlayer.clip.length);
                m_videoPlayer.frame = 0;
                //m_videoPlayer.Play();
                //StartCoroutine(ReplaceVideo());
            }
        }
        
        if (p_start && m_start)
        {
            m_start = false;
            m_videoPlayer.frame = 0;
            m_videoPlayer.Play();
            StartCoroutine(ReplaceVideo());
        }
    }

    IEnumerator ReplaceVideo()
    {
        yield return m_waitUntilReplaceVideo;
        m_videoPlayer.frame = 0;
        
        m_event.Raise(false);
    }
}