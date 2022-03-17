using System;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    //Changement de vision
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision")] private AnimationCurve m_curveVision;
    [SerializeField, Tooltip("Courbe de pourcentage pour la transparence du matérial allé vers l'état modifié")] private AnimationCurve m_curveMatVision;

    [SerializeField, Tooltip("Material de flou pour le postprocess")] private Material m_matVision;
    [SerializeField, Tooltip("Material des matérials Invisible en net et visible en flou")] private Material m_matInvisibleVisible;
    [SerializeField, Tooltip("Material des matérials Visible en net et Invisible en flou")] private Material m_matVisibleInvisible;
    private float m_timeVision;
    private bool m_resetTimeVisionComp = false;
    private bool m_resetTimeVisionMat = false;
    private int m_readyEnd = -1;

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
            m_readyEnd = 0;
            m_timeVision = Time.time;
            m_resetTimeVisionComp = true;
            m_resetTimeVisionMat = true;
        }
        
        float tTime = Time.time - m_timeVision;

        float startTimeSecMat = (m_curveVision.keys[m_curveVision.length - 1].time - m_curveMatVision.keys[m_curveMatVision.length - 1].time);
        //Vision joueur
        DoSwitchView(tTime);

        //Chanagement obj en invisible dans le flou
        if (m_readyEnd == 0)
        {
            //Debug.Log("hey mat 1");
            DoSwitchMaterial(tTime, m_curveMatVision, 0);
        }

        //Chanagement obj en Visible dans le flou
        if ((int)tTime == (int)startTimeSecMat && m_resetTimeVisionComp)
        {
            //Debug.Log("hey mat 2");
            m_readyEnd = 1;
            m_resetTimeVisionMat = true;
            DoSwitchMaterial(tTime, m_curveMatVision, startTimeSecMat);
        }
    }
    
    private void DoSwitchView(float p_time)
    {
        if (p_time > m_curveVision.keys[m_curveVision.length-1].time && m_resetTimeVisionComp)
        {
            m_readyEnd = -1;
            PlayerManager.Instance.LaunchDelegate();
            m_resetTimeVisionComp = false;
            return;
        }

        if (m_resetTimeVisionComp)
        {
            float blurValue = m_curveVision.Evaluate(p_time);
            m_matVision.SetFloat("_BlurSize", blurValue);
        }
    }

    private void DoSwitchMaterial(float p_time, AnimationCurve p_dir, float p_minusTime)
    {
        float time = Mathf.Abs(p_time - p_minusTime);
        
        if (time > p_dir.keys[p_dir.length - 1].time && m_resetTimeVisionMat)
        {
            Debug.Log("Finish Material");
            PlayerManager.Instance.LaunchDelegate();
            m_resetTimeVisionMat = false;
            return;
        }

        if (m_resetTimeVisionMat)
        {
            float matVisibilityValue = p_dir.Evaluate(time);
            m_matInvisibleVisible.SetFloat("_StepStrenght", matVisibilityValue);
            m_matVisibleInvisible.SetFloat("_StepStrenght", matVisibilityValue);
        }
    }
}
