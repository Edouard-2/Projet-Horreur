using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    //Changement de vision
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision")] private AnimationCurve m_curveVision;
    [SerializeField, Tooltip("Material de flou pour le postprocess")] private Material m_matVision;
    [SerializeField, Tooltip("Material des matérials Invisible en net et visible en flou")] private Material m_matInvisibleVisible;
    [SerializeField, Tooltip("Material des matérials Visible en net et Invisible en flou")] private Material m_matVisibleInvisible;
    private float m_timeVision;
    private bool m_resetTimeVision = false;

    private void OnEnable()
    {
        PlayerManager.Instance.DoSwitchViewHandler += IsInputDown;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.DoSwitchViewHandler -= IsInputDown;
    }

    private void Awake()
    {
        m_matVision.SetFloat("_BlurSize",0);
    }

    private void IsInputDown()
    {
        //Changement de vision
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_timeVision = Time.time;
            m_resetTimeVision = true;
            DoSwitchView();
        }
    }
    
    private void DoSwitchView()
    {
        float tTime = Time.time - m_timeVision;
        if (tTime > m_curveVision.keys[m_curveVision.length-1].time)
        {
            m_resetTimeVision = false;
            return;
        }
        
        if (m_resetTimeVision)
        {
            float blurValue = m_curveVision.Evaluate(tTime);
            m_matVision.SetFloat("_BlurSize",blurValue);
        }
    }
}
