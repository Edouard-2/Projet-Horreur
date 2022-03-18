using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerLook))]
public class PlayerManager : Singleton<PlayerManager>
{
    //Changement de vision
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision au début de la compétence")] private AnimationCurve m_curveVisionStart;
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision à la fin de la compétence")] private AnimationCurve m_curveVisionFinish;
    [SerializeField, Tooltip("Courbe de pourcentage pour la transparence du matérial allé vers l'état modifié au début de la compétence")] private AnimationCurve m_curveMatVisionStart;
    [SerializeField, Tooltip("Courbe de pourcentage pour la transparence du matérial allé vers l'état modifié à la fin de la compétence")] private AnimationCurve m_curveMatVisionFinish;

    [SerializeField, Tooltip("Material de flou pour le postprocess")] private Material m_matVision;
    [SerializeField, Tooltip("Material des matérials Invisible en net et visible en flou")] private Material m_matInvisibleVisible;
    [SerializeField, Tooltip("Material des matérials Visible en net et Invisible en flou")] private Material m_matVisibleInvisible;
    private float m_timeVision;
    private bool m_resetTimeVisionComp = false;
    private bool m_resetTimeVisionMat = false;
    private int m_readyEnd = 1;
    
    //Constante
    private const float m_gravity = -9.81f;

    public float Gravity
    {
        get => m_gravity;
    }

    public delegate void DoAction();
    public DoAction DoMouvmentHandler;
    public DoAction DoCursorHandler;

    public delegate void DoVisionSwitch();
    public DoVisionSwitch DoVisibleToInvisibleHandler;
    
    private void Awake()
    {
        m_matVision.SetFloat("_BlurSize",0);
    }
    
    private void Update()
    {
        //Mouvement du Joueur
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            DoMouvmentHandler?.Invoke();
        }

        //Mouvement de la camera
        DoCursorHandler?.Invoke();
        
        //Input Blur Effect
        //IsInputDown();
    }
    
    private void IsInputDown()
    {


        float tTime = Time.time - m_timeVision;

        //Changement de vision
        if (Input.GetKeyDown(KeyCode.Space) && !m_resetTimeVisionComp && !m_resetTimeVisionMat)
        {

            m_timeVision = Time.time;
            m_readyEnd = Mathf.Abs(m_readyEnd - 1);

            m_resetTimeVisionComp = true;
            m_resetTimeVisionMat = true;
        }

        /*if (m_readyEnd == 0)
        {
            if (m_resetTimeVisionComp)
            {
                Debug.Log("Start");

                //DoSwitchView(allé)
                //DoSwitchView(tTime, m_curveVisionStart);
            }
            if (m_resetTimeVisionMat)
            {
                //DoSwitchMaterial(allé)
                //DoSwitchMaterial(tTime, m_curveMatVisionStart);

            }

            //DoVisibleToInvisibleHandler?.Invoke();

        }
        else if (m_readyEnd == 1)
        {
            if (m_resetTimeVisionComp)
            {
                Debug.Log("Fin");
                //DoSwitchView(retour)
                //DoSwitchView(tTime, m_curveVisionFinish);
            }
            if (m_resetTimeVisionMat)
            {
                //DoSwitchMaterial(retour)
                //DoSwitchMaterial(tTime, m_curveMatVisionFinish);
            }

            //DoVisibleToInvisibleHandler?.Invoke();
        }*/


        //Vision joueur
        //DoSwitchView(tTime);

        /*//Chanagement obj en invisible dans le flou
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
        }*/
    }
    
    private void DoSwitchView(float p_time, AnimationCurve p_curve)
    {
        if (p_time > p_curve.keys[p_curve.length-1].time && m_resetTimeVisionComp)
        {
            //DoVisibleToInvisibleHandler?.Invoke();
            m_resetTimeVisionComp = false;
            return;
        }

        if (m_resetTimeVisionComp)
        {
            float blurValue = p_curve.Evaluate(p_time);
            m_matVision.SetFloat("_BlurSize", blurValue);
        }
    }

    private void DoSwitchMaterial(float p_time, AnimationCurve p_dir)
    {
        if (p_time > p_dir.keys[p_dir.length - 1].time && m_resetTimeVisionMat)
        {
            Debug.Log("Finish Material");
            //DoVisibleToInvisibleHandler?.Invoke();
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

    protected override string GetSingletonName()
    {
        return "PlayerManager";
    }
}