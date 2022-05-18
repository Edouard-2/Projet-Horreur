using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIMainMenu_Over))]
public class UIMainMenu_Play : MonoBehaviour
{
    [SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private int m_sceneIndex;
    
    [SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private Animator m_fadeAnimator;
    
    [SerializeField, Tooltip("Emitter du son d'ambiance")] 
    private StudioEventEmitter m_emitter;

    private WaitForSeconds m_waitUntilLoadScene = new WaitForSeconds(0.5f);
    
    private void OnMouseDown()
    {
        Debug.Log("Over Text");
        //Lanacer l'animation du fond noir
        m_fadeAnimator.SetTrigger(Animator.StringToHash("FadeIn"));
        SoundManager.FadeOut(m_emitter, false);
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