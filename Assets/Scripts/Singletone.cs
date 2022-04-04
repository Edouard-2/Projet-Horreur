using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_instance = null;

    protected static bool m_reInstance = true;

    [SerializeField][Tooltip("Ne pas détruire au chargement")] private bool m_dontdestroyOnLoad = false;
    
    public static T Instance
    {
        get
        {
            if (m_instance == null && m_reInstance)
            {
                FindOrCreateInstance();
            }

            return m_instance;
        }
    }

    protected static void FindOrCreateInstance()
    {
        m_instance = FindObjectOfType<T>();
        if (m_instance != null)
        {
            (m_instance as Singleton<T>).Setup();
            return;
        }

        GameObject go = new GameObject();
        m_instance = go.AddComponent<T>();
        (m_instance as Singleton<T>).Setup(true);
    }

    protected virtual void Setup(bool p_needRename = false)
    {
        if (m_dontdestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        if (p_needRename)
        {
            gameObject.name = GetSingletonName();
        }
    }

    protected abstract string GetSingletonName();

    private void Awake()
    {
        if (m_instance != null)
        {
            if (m_instance != this)
                Destroy(this.gameObject);
            return;
        }
        FindOrCreateInstance();
    }
}