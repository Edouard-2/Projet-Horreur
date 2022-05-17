using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    [Header("Composent")] 
    [SerializeField, Tooltip("Premier Texte pour les subtitle")]
    private TextMeshProUGUI m_firstText;

    [SerializeField, Tooltip("Deuxième Texte pour les subtitle")]
    private TextMeshProUGUI m_secondText;
    
    [SerializeField, Tooltip("Animator du premier textMeshPro")]
    private Animator m_firstAnimator;

    [SerializeField, Tooltip("Animator du deuxième textMeshPro")]
    private Animator m_secondAnimator;

    [SerializeField, Tooltip("Les différents dialogues")]
    private TextScriptable m_dialogues;

    [Header("Event")] [SerializeField, Tooltip("L'event qui va déclencher le sous Titre")]
    private EventsTrigger m_eventRaise;

    [Header("Stats")] [SerializeField, Tooltip("Frequance à laquelle les phrases défilent")]
    private float m_frequenceSwitchText;

    [SerializeField, Tooltip("Longueur des phrases qui seront affichées")]
    private int m_sentenceLength;

    private int m_indexTextmeshpro = -1;
    private int m_indexDialogue = 0;

    private int m_currentSentenceLenght;

    private struct CurrentObj
    {
        public TextMeshProUGUI text;
        public Animator anim;
    }

    private CurrentObj m_firstObj;
    private CurrentObj m_secondObj;
    
    private CurrentObj m_firstCurrent;
    private CurrentObj m_secondCurrent;
    
    private int m_displayHash;
    private int m_transHash;
    private int m_hideHash;
    
    private string m_currentDialogueArray;

    private WaitForSeconds m_waitUntilTransition;
    private WaitForSeconds m_waitUntilEndTransition;

    private void OnEnable()
    {
        m_eventRaise.OnTrigger += LaunchSubtitle;
    }

    private void OnDisable()
    {
        m_eventRaise.OnTrigger -= LaunchSubtitle;
    }

    private void Awake()
    {
        m_waitUntilTransition = new WaitForSeconds(m_frequenceSwitchText);
        m_waitUntilEndTransition = new WaitForSeconds(m_frequenceSwitchText - 1);

        InitStruct();

        InitAnim();
    }

    private void InitAnim()
    {
        m_hideHash = Animator.StringToHash("Hide");
        m_transHash = Animator.StringToHash("Transition");
        m_displayHash = Animator.StringToHash("Display");
    }

    private void InitStruct()
    {
        //Init Value
        m_firstObj.text = m_firstText;
        m_firstObj.anim = m_firstAnimator;
        
        m_secondObj.text = m_secondText;
        m_secondObj.anim = m_secondAnimator;

        //Init Current
        SetCurrentText(1);
    }

    private void LaunchSubtitle(bool p_isstart = false)
    {
        if (p_isstart)
        {
            m_currentDialogueArray = m_dialogues.m_listDialogues[m_indexDialogue];
            FirstTransition(m_firstCurrent);
            return;
        }
        SwitchCurrentText();
        FirstTransition(m_firstCurrent);
        SecondTransition(m_secondCurrent.anim);
        StartCoroutine(EndTransition(m_secondCurrent.anim));
    }

    //Changement des current text
    private void SwitchCurrentText()
    {
        if (m_firstCurrent.text == m_firstObj.text)
        {
            SetCurrentText(2);
            return;
        }
        SetCurrentText(1);
    }

    private void SetCurrentText(int p_cur)
    {
        if (p_cur == 1)
        {
            m_firstCurrent = m_firstObj;
            m_secondCurrent = m_secondObj;
            return;
        }
        m_firstCurrent = m_secondObj;
        m_secondCurrent = m_firstObj;
    }

    private string GetCurrentTextDialogue()
    {
        string dialogueArray = SetNewDialogueArray(m_currentSentenceLenght);
        
        m_currentSentenceLenght += m_sentenceLength;

        return dialogueArray;
    }

    private string SetNewDialogueArray(int p_nbr)
    {
        if(m_currentSentenceLenght == 0) return m_dialogues.m_listDialogues[m_indexDialogue];
        
        m_currentDialogueArray = "";
        
        for (int i = 0; i < m_dialogues.m_listDialogues[m_indexDialogue].Length; i++)
        {
            if (m_currentSentenceLenght <= i)
            {
                m_currentDialogueArray += m_dialogues.m_listDialogues[m_indexDialogue][i];
            }
        }

        return m_currentDialogueArray;
    }

    //Faire apparaitre le texte
    private void FirstTransition(CurrentObj p_text)
    {
        string text = GetCurrentTextDialogue();
        char space = ' ';
        p_text.text.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (i > m_sentenceLength - 1 && text[i].GetHashCode() == space.GetHashCode()) break;
            p_text.text.text += text[i];
        }

        p_text.anim.ResetTrigger(m_hideHash);
        p_text.anim.SetTrigger(m_displayHash);
        
        StartCoroutine(WaitForNewTransition(2, p_text.text));
    }

    //Faire l'animation de monté du text
    private void SecondTransition(Animator p_anim)
    {
        p_anim.ResetTrigger(m_displayHash);
        p_anim.SetTrigger(m_transHash);
    }

    //Faire l'animation de disparition du text
    IEnumerator EndTransition(Animator p_anim)
    {
        yield return m_waitUntilEndTransition;
        p_anim.ResetTrigger(m_transHash);
        p_anim.SetTrigger(m_hideHash);
    }

    IEnumerator WaitForNewTransition(int p_trans, TextMeshProUGUI p_text)
    {
        yield return m_waitUntilTransition;
        LaunchSubtitle();
    }
}