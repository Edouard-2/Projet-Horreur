using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerVision : MonoBehaviour
{
    //Courbe changement vision
    [Header("Post Process")]
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

    [Header("VFX")]
    [SerializeField, Tooltip("VFX de chaleur du monstre")] 
    public VisualEffect m_chaleurVFX;        
    
    [Header("Material")]
    [SerializeField, Tooltip("Material de flou pour le postprocess")] 
    public Material m_matVision;
    [SerializeField, Tooltip("Material de la tête du monstre")] 
    public Material m_matMonsterHead;
    [SerializeField, Tooltip("Material des pattes du monstre")] 
    public Material m_matMonsterSpeaks;
    [HideInInspector]public bool m_resetTimeVisionComp = false;
    [HideInInspector]public bool m_resetTimeVisionMat = false;
    [FormerlySerializedAs("m_readyEnd")][HideInInspector] 
    public int m_isBlurVision = 1;

    //BV
    [Header("BV")]
    [SerializeField, Tooltip("BV visuel")] public Image m_uiBv;
    [Range(0,1),SerializeField, Tooltip("La vitesse de consommation de la BV (en vision flou)")] 
    public float m_speedDecreaseBV = 0.1f;
    [Range(0.3f,10),SerializeField, Tooltip("le temps pendant lequel le joueur est aveugle")] 
    public float m_blindTime = 10f;
    [Range(0,0.5f),SerializeField, Tooltip("Lorsque le joueur perd de la BV max aprés avoir été aveugle (C'est aussi la valeur qui est utiliser pour faire le cran d'utilisation à chaque utilisation)")] 
    public float m_lessBvMax = 0.1f;
    [HideInInspector]public float m_BvMax = 1f;
    [HideInInspector]public float m_currentBvMax = 1f;
    [HideInInspector]public bool m_readyInitVision = true;

    [Header("Event")]
    [SerializeField, Tooltip("Event du sound")]
    public EventsTriggerPos m_soundEvent;
    
    [Header("Sound")]
    [SerializeField, Tooltip("Emitter du blind moment")]
    public StudioEventEmitter m_blindEmitter;
    
    [SerializeField, Tooltip("Emitter du stop blind moment")]
    public StudioEventEmitter m_emitterStopBlind;

    public delegate void ChangeMaterial(float p_time);
    public ChangeMaterial DoChangeMaterial;

    private bool m_isVariableReady = true;

    [HideInInspector] public float m_timeLaunchBlind;
    [HideInInspector] public float m_timeStopBlind;

    private WaitForSeconds m_waitDepthVignette = new WaitForSeconds(0.01f);
    private Coroutine m_blindCoroutine;
    private List<Coroutine> m_listCoroutine;

    [HideInInspector]public bool m_blindActive;
    
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
            m_matVision.SetFloat("_BlurSize", p_curve.keys[p_curve.length - 1].value);
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
            
            float value = matVisibilityValue;
            
            if (m_isBlurVision == 1 && value > 0) value = matVisibilityValue - 0.798179f;
            
            m_matMonsterHead.SetFloat("_DistortionStrength", value);
            m_matMonsterSpeaks.SetFloat("_DistortionStrength", value);
        }
    }

    public void IncreaseOrDecreaseMat(float p_time)
    {
        //Optimiser les m_isBlurVision
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
            m_uiBv.fillAmount -= m_lessBvMax;
            m_postProcessScript.m_vignetteStrength += m_lessBvMax * 0.18f;
            m_postProcessScript.UpdateVignette();
        }
    }

    public void DecreaseBV()
    {
        if (m_isVariableReady && !PlayerManager.Instance.m_isStartBlur)
        {
            if (m_uiBv.fillAmount > 0)
            {
                m_uiBv.fillAmount -= m_speedDecreaseBV * Time.deltaTime;
                if (m_postProcessScript.m_vignetteStrength < m_postProcessScript.m_vignetteStepMax)
                {
                    m_postProcessScript.m_vignetteStrength += m_speedDecreaseBV * Time.deltaTime * m_postProcessScript.m_vignetteStepMultiplier;
                    m_postProcessScript.UpdateVignette();
                }
                return;
            }
            BlindMoment();
        }
    }
    
    public void IncreaseBV()
    {
        if (m_isVariableReady)
        {
            if (m_uiBv.fillAmount <= m_currentBvMax )
            {
                m_uiBv.fillAmount += m_speedDecreaseBV * Time.deltaTime;
                
                if( m_postProcessScript.m_vignetteStrength > m_postProcessScript.m_vignetteStartValue)
                {
                    m_postProcessScript.m_vignetteStrength -= m_speedDecreaseBV * Time.deltaTime * m_postProcessScript.m_vignetteStepMultiplier;
                    m_postProcessScript.UpdateVignette();
                }
            }
        }
    }

    public void ResetCurrentBV()
    {
        m_currentBvMax = m_BvMax;
        m_postProcessScript.m_vignetteStartValue = m_postProcessScript.m_vignetteInitValue;
        m_postProcessScript.UpdateVignette();
    }

    public void BlindMoment()
    {
        if (m_isVariableReady)
        {
            m_blindActive = true;
            
            m_blindEmitter.Play();
            
            Debug.Log(m_blindActive);
            
            m_soundEvent.Raise(PlayerManager.Instance.transform.position);
            
            PlayerManager.Instance.InitVariableChangement();

            m_currentBvMax -= m_lessBvMax;
            
            //Augmenter la vignette de la BV en fonction de la BV max
            float t = Mathf.InverseLerp(0,1,1 - m_currentBvMax);
            float value = Mathf.Lerp(m_postProcessScript.m_vignetteInitValue,m_postProcessScript.m_vignetteStepMax,t);
            
            m_postProcessScript.m_vignetteStartValue = value;
            
            m_uiBv.fillAmount = m_currentBvMax;
            m_postProcessScript.m_lutTransition = m_currentBvMax;

            PlayerManager.Instance.CheckCurrentKey(m_isBlurVision);

            m_blindCoroutine = StartCoroutine(WaitStopBlind(m_blindTime));
            
            PlayerManager.Instance.DoSwitchLayer(true);
            LaunchCoroutineEffects(1, 0.01f);
        }
    }

    public void StopOrStartBlindEffects()
    {
        if (!m_readyInitVision)
        {
            if (m_timeStopBlind - m_timeLaunchBlind < m_blindTime)
            {
                Debug.Log("Setp 1");
                LaunchCoroutineEffects(1, 0.02f);
                StartCoroutine(WaitStopBlind(m_timeStopBlind - m_timeLaunchBlind));
                return;
            }
            Debug.Log("Setp 2");
            LaunchCoroutineEffects(-1, 0.0008f);
        }
    }

    public void LaunchCoroutineEffects(float p_dir, float p_stepDepth)
    {
        StartCoroutine(ActiveBlindEffectDepth(p_dir, p_stepDepth));
        StartCoroutine(ActiveBlindEffectVignette(-p_dir));
    }

    IEnumerator WaitStopBlind(float p_time)
    {
        m_readyInitVision = false;
        if (m_timeLaunchBlind == 0)
        {
            m_timeLaunchBlind = Time.time;
        }
        yield return new WaitForSeconds(p_time);
        LaunchCoroutineEffects(-1,0.0008f);
    }

    public IEnumerator ActiveBlindEffectDepth(float p_dir, float p_step)
    {
        yield return m_waitDepthVignette;
        
        if (m_postProcessScript.m_depthStrenght < 0.9f && p_dir > 0 
            ||m_postProcessScript.m_depthStrenght > 0.25f && p_dir < 0 )
        {
            m_postProcessScript.m_depthStrenght += p_step * 2 * p_dir;
            m_postProcessScript.UpdateDepth();
            StartCoroutine(ActiveBlindEffectDepth(p_dir, p_step));
        }
        else if( m_postProcessScript.m_depthStrenght <= 0.25f )
        {
            Debug.Log("RemmettreTout");
            m_emitterStopBlind.Play();
            m_blindActive = false;
            PlayerManager.Instance.DoSwitchLayer(false);
            m_postProcessScript.m_depthStrenght = 0;
            m_postProcessScript.UpdateDepth();
            m_timeLaunchBlind = 0;
            m_readyInitVision = true;
            Debug.Log(m_blindActive);
        }
    }

    IEnumerator ActiveBlindEffectVignette(float p_dir)
    {
        yield return m_waitDepthVignette;
        
        if (m_postProcessScript.m_vignetteStrength > m_postProcessScript.m_vignetteStartValue)
        {
            m_postProcessScript.m_vignetteStrength += m_speedDecreaseBV * 0.02f * p_dir;
            m_postProcessScript.UpdateVignette();
            StartCoroutine(ActiveBlindEffectVignette(p_dir));
        }
    }
}