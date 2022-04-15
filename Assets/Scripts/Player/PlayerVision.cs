using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerVision : MonoBehaviour
{
    //Courbe changement vision
    [Header("Animation Curves")]
    [SerializeField, Tooltip("Script d'application du post process")] 
    public PostProcessApply m_postProcessScript;
    
    [Header("Animation Curves")]
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision au début de la compétence")] 
    public AnimationCurve m_curveVisionStart;
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision é la fin de la compétence")] 
    public AnimationCurve m_curveVisionFinish;
    [SerializeField, Tooltip("Courbe de pourcentage pour la transparence du matérial allé vers l'état modifié au début de la compétence")] 
    public AnimationCurve m_curveMatVisionStart;
    [SerializeField, Tooltip("Courbe de pourcentage pour la transparence du matérial allé vers l'état modifié à la fin de la compétence")] 
    public AnimationCurve m_curveMatVisionFinish;
    [SerializeField, Tooltip("Courbe de pourcentage pour le changement de la lut table lorsqu'il passe en vision flou")] 
    public AnimationCurve m_curveLutStart;
    [SerializeField, Tooltip("Courbe de pourcentage pour la changement de la lut table lorsqu'il passe en vision net / normal")] 
    public AnimationCurve m_curveLutFinish;

    [Header("Material")]
    [SerializeField, Tooltip("Material de flou pour le postprocess")] public Material m_matVision;
    [HideInInspector]public bool m_resetTimeVisionComp = false;
    [HideInInspector]public bool m_resetTimeVisionMat = false;
    [FormerlySerializedAs("m_readyEnd")][HideInInspector] public int m_isBlurVision = 1;

    //BV
    [Header("BV")]
    [SerializeField, Tooltip("BV visuel")] public Image m_uiBv;
    [SerializeField, Tooltip("La vitesse de consommation de la BV (en vision flou)")] public float m_speedDecreaseBV = 0.1f;
    [SerializeField, Tooltip("La vitesse de consommation de la BV (en vision flou)")] public float m_MultiplIncreaseBV = 2f;
    [SerializeField, Tooltip("le temps pendant lequel le joueur est aveugle")] public float m_blindTime = 10f;
    [SerializeField, Tooltip("Lorsque le joueur perd de la BV max aprés avoir été aveugle")] public float m_lessBvMax = 0.1f;
    [HideInInspector]public float m_BvMax = 1f;
    [HideInInspector]public float m_currentBvMax = 1f;
    [HideInInspector]public bool m_readyInitVision = true;

    [Header("Event")]
    [SerializeField, Tooltip("Event du sound")]
    public EventsTriggerPos m_soundEvent;

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

        if (m_postProcessScript == null)
        {
            m_postProcessScript = GetComponent<PostProcessApply>();
            if (m_postProcessScript == null)
            {
                Debug.LogError("Mettre le composent PostProcessApply dans le script Player Vision!!!", this);
            }
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

    private void DoSwitchLut(float p_time, AnimationCurve p_dir)
    {
        if (p_time > p_dir.keys[p_dir.length - 1].time) return;
        
        m_postProcessScript.m_lutTransition =  p_dir.Evaluate(p_time);
        m_postProcessScript.UpdateLutTable();
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
            
            DoChangeMaterial?.Invoke(matVisibilityValue);
        }
    }

    public void IncreaseOrDecreaseMat(float p_time)
    {
        if (m_isBlurVision == 0)
        {
            if (m_resetTimeVisionComp)
            {
                //DoSwitchView(allé)
                DoSwitchView(p_time, m_curveVisionStart);
            }

            if (m_resetTimeVisionMat)
            {
                //DoSwitchMaterial(allé)
                DoSwitchMaterial(p_time, m_curveMatVisionStart);
                DoSwitchLut(p_time, m_curveLutStart);
            }

            if (!m_resetTimeVisionComp && !m_resetTimeVisionMat)
            {
                //Lancement de la consommation de BV
                DecreaseBV();
                m_postProcessScript.UpdateLutTable();
            }
        }

        else if (m_isBlurVision == 1)
        {
            if (m_resetTimeVisionComp)
            {
                //DoSwitchView(retour)
                DoSwitchView(p_time, m_curveVisionFinish);
            }

            if (m_resetTimeVisionMat)
            {
                //DoSwitchMaterial(retour)
                DoSwitchMaterial(p_time, m_curveMatVisionFinish);
                DoSwitchLut(p_time, m_curveLutFinish);
            }
            
            if (!m_resetTimeVisionComp && !m_resetTimeVisionMat)
            {
                //Lancement de la recharge de BV
                IncreaseBV();
                m_postProcessScript.UpdateLutTable();
            }
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
        if (m_isVariableReady && !PlayerManager.Instance.m_isStartBlur)
        {
            if (m_uiBv.fillAmount > 0)
            {
                m_uiBv.fillAmount -= m_speedDecreaseBV * Time.deltaTime;
                m_postProcessScript.m_vignetteStrength += m_speedDecreaseBV * Time.deltaTime;
                m_postProcessScript.UpdateVignette();
                return;
            }

            BlindMoment();
        }
    }
    public void IncreaseBV()
    {
        if (m_isVariableReady)
        {
            if (m_uiBv.fillAmount <= m_currentBvMax && m_postProcessScript.m_vignetteStrength > 0.01f)
            {
                m_uiBv.fillAmount += m_speedDecreaseBV * Time.deltaTime * m_MultiplIncreaseBV;
                m_postProcessScript.m_vignetteStrength -= m_speedDecreaseBV * Time.deltaTime;
                m_postProcessScript.UpdateVignette();
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
            
            m_soundEvent.Raise(PlayerManager.Instance.transform.position);
            
            PlayerManager.Instance.InitVariableChangement();

            m_currentBvMax -= m_lessBvMax;
            m_uiBv.fillAmount = m_currentBvMax;
            m_postProcessScript.m_lutTransition = m_currentBvMax;

            PlayerManager.Instance.CheckCurrentKey(m_isBlurVision);

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
