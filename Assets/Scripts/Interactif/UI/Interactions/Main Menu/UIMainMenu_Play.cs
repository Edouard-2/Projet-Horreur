using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIMainMenu_Over))]
public class UIMainMenu_Play : MonoBehaviour
{
    [SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private int m_sceneIndex;
    
    private void OnMouseDown()
    {
        Debug.Log("Over Text");
        SceneManager.LoadSceneAsync(m_sceneIndex);
    }
}