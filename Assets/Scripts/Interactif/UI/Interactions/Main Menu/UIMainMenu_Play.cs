using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIMainMenu_Over))]
public class UIMainMenu_Play : MonoBehaviour
{
    [SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private GameObject m_noCol;
    
    [SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private int m_sceneIndex;
    
    [SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private Animator m_fadeAnimator;
    
    [SerializeField, Tooltip("Emitter du son d'ambiance")] 
    private StudioEventEmitter m_emitter;

    private WaitForSeconds m_waitUntilLoadScene = new WaitForSeconds(0.5f);
    
    private void OnMouseDown()
    {
        GameManager.Instance.SetState(GameManager.States.LOADING);
        
        //Lanacer l'animation du fond noir
        m_emitter.Stop();
        m_noCol.SetActive(true);
        m_fadeAnimator.gameObject.SetActive(true);
        m_fadeAnimator.SetTrigger(Animator.StringToHash("FadeIn"));
        StartCoroutine(WaitUntilLaunchLoadingScene());
    }

    IEnumerator WaitUntilLaunchLoadingScene()
    {
        yield return m_waitUntilLoadScene;
        
        //Lancement de la scene principale
        StartCoroutine(LoadAsynScene(m_sceneIndex));
    }
    
    IEnumerator LoadAsynScene(int p_sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(p_sceneIndex);
        
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}