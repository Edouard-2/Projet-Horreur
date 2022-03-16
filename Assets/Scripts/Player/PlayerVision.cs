using System;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    //Changement de vision
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision")] private AnimationCurve m_curveVision;
    [SerializeField, Tooltip("Courbe de pourcentage pour la transparence du matérial allé vers l'état modifié")] private AnimationCurve m_curveMatVisionVisible;
    [SerializeField, Tooltip("Courbe de pourcentage pour la transparence du matérial retour à l'état normal")] private AnimationCurve m_curveMatVisionInvisible;

    [SerializeField, Tooltip("Material de flou pour le postprocess")] private Material m_matVision;
    [SerializeField, Tooltip("Material des matérials Invisible en net et visible en flou")] private Material m_matInvisibleVisible;
    [SerializeField, Tooltip("Material des matérials Visible en net et Invisible en flou")] private Material m_matVisibleInvisible;
    private float m_timeVision;
    private bool m_resetTimeVisionComp = false;
    private bool m_resetTimeVisionMat = false;

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
            m_resetTimeVisionComp = true;
            m_resetTimeVisionMat = true;
        }
        float tTime = Time.time - m_timeVision;
        DoSwitchView(tTime);
        DoSwitchMaterial(tTime, m_curveMatVisionVisible);
    }
    
    private void DoSwitchView(float p_time)
    {
        if (p_time > m_curveVision.keys[m_curveVision.length-1].time)
        {
            m_resetTimeVisionComp = false;
            return;
        }
        
        /*if((int)p_time == (int)(m_curveVision.keys[m_curveVision.length - 1].time - m_curveMatVisionVisible.keys[m_curveMatVisionVisible.length - 1].time) && m_readyEnd == 0)
        {
            m_resetTimeVisionMat = true;
            m_readyEnd = 1;
            Debug.Log("CHangement");
        }*/

        if (m_resetTimeVisionComp)
        {
            float blurValue = m_curveVision.Evaluate(p_time);
            m_matVision.SetFloat("_BlurSize", blurValue);
        }
    }

    private void DoSwitchMaterial(float p_time, AnimationCurve p_dir)
    {
        if (p_time > p_dir.keys[p_dir.length - 1].time)
        {
            m_resetTimeVisionMat = false;
            return;
        }

        if (m_resetTimeVisionMat)
        {
            float matVisibilityValue = p_dir.Evaluate(p_time);
            m_matInvisibleVisible.SetFloat("_StepStrenght", matVisibilityValue);
            m_matVisibleInvisible.SetFloat("_StepStrenght", matVisibilityValue);
        }
    }
}
