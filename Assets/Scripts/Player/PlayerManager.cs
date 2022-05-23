using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerManager : Singleton<PlayerManager>
{
    //Constante
    private const float m_gravity = -9.81f;

    //Camera
    [Header("Camera")]
    [SerializeField, Tooltip("Camera Principale")]
    public Camera m_camera;
    
    //Event
    [Header("End Cinematique")]
    [SerializeField, Tooltip("Event qui va déclencher la cinématique")]
    public EventsTrigger m_endCinematique;
    [SerializeField, Tooltip("Event qui va déclencher la cinématique")]
    public PlayableDirector m_endTimeline;

    //LayerMask
    [Header("LayerMask")]
    [SerializeField, Tooltip("Layer pour les keyInvisible")]
    private LayerMask m_keyLayerInvisible;

    [SerializeField, Tooltip("Layer pour les doorInvisible")]
    private LayerMask m_doorLayerInvisible;

    [SerializeField, Tooltip("Layer pour les transvaseurInvisibles")]
    private LayerMask m_transvaseurLayerInvisible;

    //Scripts
    [Header("Scripts")]
    [SerializeField, Tooltip("Script player controller")]
    public PlayerController m_controllerScript;

    [SerializeField, Tooltip("Script player look")]
    public PlayerLook m_lookScript;

    [SerializeField, Tooltip("Script player vision")]
    public PlayerVision m_visionScript;

    [SerializeField, Tooltip("Script player door")]
    public PlayerInteractions m_interactionsScript;
    
    //Death Screen
    [Header("Death Screen")]
    [SerializeField, Tooltip("Animator du fade out / in")]
    private Animator m_fadeAnimator;
    
    [SerializeField, Tooltip("Ui du death screen")]
    private GameObject m_deathUI;

    private int m_fadeIn;
    private int m_fadeOut;
    
    //Other
    [Header("Other")]
    [SerializeField, Tooltip("True: Le joueur commence en flou")]
    public bool m_isStartBlur;
    
    [Range(0,100), Tooltip("Radius de vision du joueur")]
    public float m_radiusVision;
    
    [Range(0,20), SerializeField, Tooltip("Temps avant de relancer le jeu apres la mort")]
    public float m_DeathWaitingTime;
    
    [SerializeField, Tooltip("Emitter d'ambiance Blur Effect")]
    private StudioEventEmitter m_blurAmbiance;
    
    [SerializeField, Tooltip("Emitter d'activation du Blur Effect")]
    private StudioEventEmitter m_blurActiveEmitter;
    
    [SerializeField, Tooltip("Emitter de désactivation du Blur Effect")]
    private StudioEventEmitter m_blurDesactiveEmitter;
    
    
    private WaitForSeconds m_waitFade = new WaitForSeconds(0.5f);
    private WaitForSeconds m_waitDeath;
    
    [HideInInspector] public bool m_isHooked;
    
    private float m_timeVision;
    private float tTime;
    
    private bool m_end;
    
    private Vector3 m_checkPointPos;

    private VCA m_vcaController;

    //Sécurité pour lire une seul fois une fonction
    private bool m_prevStateReady = true;

    public float Gravity 
    {
        get => m_gravity;
    }

    public delegate void RotateKeys();
    public RotateKeys DoRotateKeys;

    public delegate void DoVisionSwitch(bool p_start = false);
    public DoVisionSwitch DoVisibleToInvisibleHandler;
    
    public delegate void SwitchLayer(bool p_start);
    public SwitchLayer DoSwitchLayer;

    public delegate void FirstKeyPos();
    public event FirstKeyPos UpdateFirstPos;

    private void OnEnable()
    {
        m_endCinematique.OnTrigger += ActiveEndCinematique;
    }
    
    private void OnDisable()
    {
        m_endCinematique.OnTrigger -= ActiveEndCinematique;
    }
    
    private void Awake()
    {
        m_vcaController = RuntimeManager.GetVCA("vca:/BlurDownVolume");
        
        m_checkPointPos = transform.position;
        
        m_waitDeath = new WaitForSeconds(m_DeathWaitingTime);
        
        //Initialisation des variables d'animation
        m_fadeIn = Animator.StringToHash("FadeIn");
        m_fadeOut = Animator.StringToHash("FadeOut");

        //Désactivation de l'UI DEath
        if (m_deathUI != null)
        {
            m_deathUI.SetActive(false);
        }
        
        //State jouable
        GameManager.Instance.SetState(GameManager.States.PLAYING);

        //Cacher le curseur
        Cursor.lockState = CursorLockMode.Locked;

        //Verifier que les variables de scripts ne son pas vide
        if (m_controllerScript == null)
            m_controllerScript = GetComponent<PlayerController>();

        if (m_lookScript == null)
            m_lookScript = GetComponent<PlayerLook>();

        if (m_visionScript == null)
            m_visionScript = GetComponent<PlayerVision>();

        if (m_interactionsScript == null)
            m_interactionsScript = GetComponent<PlayerInteractions>();
    }

    private void Start()
    {
        //Commencer avec vision flou
        if (m_isStartBlur)
        {
            m_blurAmbiance.Play();
            
            m_vcaController.setVolume(0.125f);
            
            m_visionScript.m_matVision.SetFloat("_BlurSize", 0.35f);
            m_visionScript.m_chaleurVFX.gameObject.SetActive(true);
            m_visionScript.m_matMonsterHead.SetFloat("_DistortionStrength",1);
            m_visionScript.m_matMonsterSpeaks.SetFloat("_DistortionStrength",1);
            m_visionScript.m_isBlurVision = Mathf.Abs(m_visionScript.m_isBlurVision - 1);
            DoVisibleToInvisibleHandler?.Invoke();
            m_visionScript.DoChangeMaterial(1.8f);
        }
        //Commencer sans vision flou
        else if (m_visionScript != null)
        {
            m_blurAmbiance.Stop();
            
            SoundManager.Instance.m_ambiance1.Play();
            SoundManager.Instance.m_ambiance2.Play();
            
            m_visionScript.m_matVision.SetFloat("_BlurSize", 0);
            m_visionScript.m_chaleurVFX.gameObject.SetActive(false);
            m_visionScript.m_matMonsterHead.SetFloat("_DistortionStrength",0);
            m_visionScript.m_matMonsterSpeaks.SetFloat("_DistortionStrength",0);
        }
    }

    private void Update()
    {
        //Si c'est la cinématique de fin
        if (m_end) return;
        
        //Mettre le jeu en pause
        if (Input.GetKeyDown(KeyCode.Escape) 
            && ( GameManager.Instance.State == GameManager.States.PLAYING 
            || GameManager.Instance.State == GameManager.States.PAUSE))
        {
            if (m_controllerScript.m_speedMove != m_controllerScript.m_baseSpeed)
            {
                m_controllerScript.m_speedMove = m_controllerScript.m_baseSpeed;
            }
            GameManager.Instance.SwitchPauseGame();
        }

        //Faire les fonctions si le jeux est en play
        if (GameManager.Instance != null && GameManager.Instance.State == GameManager.States.PLAYING)
        {
            //Mouvement du Joueur
            m_controllerScript.Mouvement();

            //Interaction avec des objets
            if (Input.GetKeyDown(KeyCode.E))
            {
                UpdateCheckFeedbackOrInteract(false);
            }

            //FeedBack D'Interaction
            UpdateCheckFeedbackOrInteract();

            //Changement de vision
            VisionUpdate();

            m_lookScript.CursorMouvement();

            DoRotateKeys?.Invoke();
        }

        //Reset de variable
        else if (GameManager.Instance.State == GameManager.States.PAUSE)
        {
            m_prevStateReady = true;
        }
    }

    public void RemoveAllPostProcess()
    {
        m_visionScript.m_postProcessScript.m_lutTransition = 0;
        m_visionScript.m_postProcessScript.m_vignetteStrength = m_visionScript.m_postProcessScript.m_vignetteInitValue;
        m_visionScript.m_postProcessScript.m_depthStrenght = 0;
        m_visionScript.m_postProcessScript.UpdateVignette();
        m_visionScript.m_postProcessScript.UpdateDepth();
        m_visionScript.m_postProcessScript.UpdateLutTable();
    }
    
    private void UpdateCheckFeedbackOrInteract(bool p_feedBack = true)
    {
        RaycastHit hitInteract;
        Ray rayInteract = m_camera.ScreenPointToRay(Input.mousePosition);

        //Changement de materiaux si l'obj est interactif et visé par le joueur
        if (Physics.Raycast(rayInteract, out hitInteract, m_radiusVision))
        {
            //Verification si le joueur est en vision flou
            if (m_visionScript.m_isBlurVision == 0)
            {
                //Si oui est ce que l'obj est visible (net) en mode flou
                if ((m_keyLayerInvisible.value & (1 << hitInteract.transform.gameObject.layer)) > 0
                    || (m_doorLayerInvisible.value & (1 << hitInteract.transform.gameObject.layer)) > 0
                    || (m_transvaseurLayerInvisible.value & (1 << hitInteract.transform.gameObject.layer)) > 0
                    || (m_interactionsScript.m_layerDoor.value & (1 << hitInteract.transform.gameObject.layer)) > 0)
                {
                    if (p_feedBack)
                    {
                        m_interactionsScript.VerifyFeedbackInteract(hitInteract.transform);
                    }
                    else
                    {
                        m_interactionsScript.VerifyLayer(hitInteract.transform);
                    }
                }
            }
            else
            {
                if (p_feedBack)
                {
                    m_interactionsScript.VerifyFeedbackInteract(hitInteract.transform);
                }
                else
                {
                    m_interactionsScript.VerifyLayer(hitInteract.transform);
                }
            }
        }
        
        //Sinon on remet le materiaux de base
        else if (p_feedBack)
        {
            m_interactionsScript.ResetFeedbackInteract();
        }
    }

    public void InitVariableChangement(bool p_next = true)
    {
        if (m_visionScript.m_readyInitVision || !p_next)
        {
            m_timeVision = Time.time;
            tTime = Time.time - m_timeVision;

            m_visionScript.m_resetTimeVisionComp = true;
            m_visionScript.m_resetTimeVisionMat = true;

            if (p_next)
            {
                m_visionScript.m_isBlurVision = Mathf.Abs(m_visionScript.m_isBlurVision - 1);
                if (m_visionScript.m_isBlurVision > 0)
                {
                    //Enlever le son d'ambiance BV
                    m_blurAmbiance.Stop();
                    SoundManager.Instance.m_ambiance1.Play();
                    SoundManager.Instance.m_ambiance2.Play();
                    m_blurDesactiveEmitter.Play();
                    m_vcaController.setVolume(1f);
                    
                    m_visionScript.m_chaleurVFX.gameObject.SetActive(true);
                }
                else
                {
                    //Mettre le son d'ambiance BV
                    m_blurAmbiance.Play();
                    SoundManager.Instance.m_ambiance1.Stop();
                    SoundManager.Instance.m_ambiance2.Stop();
                    m_blurActiveEmitter.Play();
                    m_vcaController.setVolume(0.125f);
                    
                    m_visionScript.m_chaleurVFX.gameObject.SetActive(false);
                }
                
                DoVisibleToInvisibleHandler?.Invoke();
            }
            else
            {
                DoVisibleToInvisibleHandler?.Invoke(true);
            }

            if (m_interactionsScript.m_keyObject != null)
            {
                CheckCurrentKey(m_visionScript.m_isBlurVision);
            }
        }
    }

    private void VisionUpdate()
    {
        tTime = Time.time - m_timeVision;

        //Changement de vision si le jeu était en pause et revien en play
        if (GameManager.Instance.PrevState == GameManager.States.PAUSE && m_prevStateReady && m_visionScript.m_resetTimeVisionMat)
        {
            m_prevStateReady = false;

            InitVariableChangement(false);

            if (m_visionScript.m_isBlurVision == 0)
            {
                Debug.Log("hey");
                //Ajout du cran sur la BV
                m_visionScript.AddStepBV();
            }
        }

        //Input changement de vision
        if (Input.GetKeyDown(KeyCode.Space) 
            && !m_visionScript.m_resetTimeVisionComp 
            && !m_visionScript.m_resetTimeVisionMat)
        {
            m_isStartBlur = false;
            
            InitVariableChangement();

            if (m_visionScript.m_isBlurVision == 0)
            {
                //Ajout du cran sur la BV
                m_visionScript.AddStepBV();
            }
        }

        m_visionScript.IncreaseOrDecreaseMat(tTime);
    }
    
    public void CheckCurrentKey(int p_readyEnd)
    {
        if (m_interactionsScript.m_trousseauKey != null)
        {
            if (p_readyEnd == 0)
            {
                if ((m_interactionsScript.m_layerKeyVisible & (1 << m_interactionsScript.m_keyObject.layer)) > 0)
                {
                    Debug.Log("Jeter la clé");
                    m_interactionsScript.EjectKey();
                }
            }
            else if (p_readyEnd == 1)
            {
                if ((m_interactionsScript.m_layerKeyInvisible & (1 << m_interactionsScript.m_keyObject.layer)) > 0)
                {
                    Debug.Log("Jeter la clé");
                    m_interactionsScript.EjectKey();
                }
            }
        }
    }

    public void SetCheckPoint(Vector3 p_pos)
    {
        if (m_checkPointPos != p_pos)
        {
            m_checkPointPos = p_pos;
        }
    }

    public void Death()
    {
        m_visionScript.ResetCurrentBV();
        
        //Fade in
        Debug.Log("Fade In Death");
        
        m_fadeAnimator.ResetTrigger(m_fadeOut);
        m_fadeAnimator.SetTrigger(m_fadeIn);

        //Bloquer les mouvement du joueur => curseur + movements
        GameManager.Instance.SetState(GameManager.States.DEATH);

        StartCoroutine(ResetLevel());
    }

    private void ActiveEndCinematique(bool p_start = true)
    {
        m_end = true;
        m_controllerScript.m_animator.ResetTrigger(m_controllerScript.m_moveHash);
        m_controllerScript.m_animator.SetTrigger(m_controllerScript.m_idleHash);
        m_endTimeline.Play();
    }
    
    IEnumerator ResetLevel()
    {
        yield return m_waitFade;
        
        //Death Screen
        m_deathUI.SetActive(true);
        
        //Vider son inventaire
        m_interactionsScript.EjectKey();
        
        //Mettre le joueur à la position du dernier checkpoint
        transform.position = m_checkPointPos;
        
        StartCoroutine(WaitUntilReset());
    }
    
    IEnumerator WaitUntilReset()
    {
        yield return m_waitDeath;
        
        m_deathUI.SetActive(false);
        
        m_fadeAnimator.ResetTrigger(m_fadeIn);
        m_fadeAnimator.SetTrigger(m_fadeOut);
        
        //Mettre les clés et le monstre à leurs emplacements de base
        UpdateFirstPos?.Invoke();
        
        StartCoroutine(AllowMovementPlayer());
    }
    
    IEnumerator AllowMovementPlayer()
    {
        yield return m_waitFade;
        
        //Remettre les movements au joueur
        GameManager.Instance.SetState(GameManager.States.PLAYING);
    }
    
    protected override string GetSingletonName()
    {
        return "PlayerManager";
    }
}