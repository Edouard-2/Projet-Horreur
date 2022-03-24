using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVision : MonoBehaviour
{
    //Courbe changement vision
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision au début de la compétence")] public AnimationCurve m_curveVisionStart;
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision é la fin de la compétence")] public AnimationCurve m_curveVisionFinish;
    [SerializeField, Tooltip("Courbe de pourcentage pour la transparence du matérial allé vers l'état modifié au début de la compétence")] public AnimationCurve m_curveMatVisionStart;
    [SerializeField, Tooltip("Courbe de pourcentage pour la transparence du matérial allé vers l'état modifié à la fin de la compétence")] public AnimationCurve m_curveMatVisionFinish;

    [SerializeField, Tooltip("Material de flou pour le postprocess")] public Material m_matVision;
    [SerializeField, Tooltip("Material des matérials Invisible en net et visible en flou")] public Material m_matInvisibleVisible;
    [SerializeField, Tooltip("Material des matérials Visible en net et Invisible en flou")] public Material m_matVisibleInvisible;
    [HideInInspector]public float m_timeVision;
    [HideInInspector]public bool m_resetTimeVisionComp = false;
    [HideInInspector]public bool m_resetTimeVisionMat = false;
    public int m_readyEnd = 1;

    //BV
    [SerializeField, Tooltip("BV visuel")] public Image m_uiBv;
    [SerializeField, Tooltip("La vitesse de consommation de la BV (en vision flou)")] public float m_speedDecreaseBV = 0.1f;
    [SerializeField, Tooltip("La vitesse de consommation de la BV (en vision flou)")] public float m_MultiplIncreaseBV = 2f;
    [SerializeField, Tooltip("le temps pendant lequel le joueur est aveugle")] public float m_blindTime = 10f;
    [SerializeField, Tooltip("Lorsque le joueur perd de la BV max aprés avoir été aveugle")] public float m_lessBvMax = 0.1f;
    [HideInInspector]public float m_BvMax = 1f;
    [HideInInspector]public float m_currentBvMax = 1f;
    [HideInInspector]public bool m_readyInitVision = true;

    [HideInInspector]public float tTime;

    public delegate void ChangeMaterial(float p_time);
    public ChangeMaterial DoChangeMaterial;

    private bool m_isVariableReady = true;

    private void Awake()
    {
        if (m_uiBv == null)
        {
            Debug.LogError("Faut mettre l'ui de la BV", this);
            m_isVariableReady = false;
        }
    }


    public void DoSwitchView(float p_time, AnimationCurve p_curve)
    {
        if (p_time > p_curve.keys[p_curve.length - 1].time && m_resetTimeVisionComp)
        {
            m_resetTimeVisionComp = false;
            return;
        }

        if (m_resetTimeVisionComp)
        {
            float blurValue = p_curve.Evaluate(p_time);
            m_matVision.SetFloat("_BlurSize", blurValue);
        }
    }

    public void DoSwitchMaterial(float p_time, AnimationCurve p_dir)
    {
        if (p_time > p_dir.keys[p_dir.length - 1].time && m_resetTimeVisionMat)
        {
            m_resetTimeVisionMat = false;
            return;
        }

        if (m_resetTimeVisionMat)
        {
            float matVisibilityValue = p_dir.Evaluate(p_time);
            //m_matInvisibleVisible.SetFloat("_StepStrenght", matVisibilityValue);
            //m_matVisibleInvisible.SetFloat("_StepStrenght", matVisibilityValue);
            
            DoChangeMaterial?.Invoke(matVisibilityValue);
        }
    }

    public void AddStepBV()
    {
        if (m_isVariableReady)
        {
            m_uiBv.fillAmount -= 0.1f;
        }
    }

    public void DecreaseBV()
    {
        if (m_isVariableReady)
        {
            if (m_uiBv.fillAmount > 0)
            {
                m_uiBv.fillAmount -= m_speedDecreaseBV * Time.deltaTime;
                return;
            }

            BlindMoment();
        }
    }
    public void IncreaseBV()
    {
        if (m_isVariableReady)
        {
            if (m_uiBv.fillAmount <= m_currentBvMax)
            {
                m_uiBv.fillAmount += m_speedDecreaseBV * Time.deltaTime * m_MultiplIncreaseBV;
            }
        }
    }

    public void ResetCurrentBV()
    {
        m_currentBvMax = m_BvMax;
    }

    public void BlindMoment()
    {
        if (m_isVariableReady)
        {
            Debug.Log("BlindMoment");
            PlayerManager.Instance.InitVariableChangement();

            m_currentBvMax -= m_lessBvMax;
            m_uiBv.fillAmount = m_currentBvMax;

            PlayerManager.Instance.CheckCurrentKey(m_readyEnd);

            StartCoroutine(WaitStopBlind());
        }
    }

    public IEnumerator WaitStopBlind()
    {
        m_readyInitVision = false;
        yield return new WaitForSeconds(m_blindTime);
        m_readyInitVision = true;
    }
}
