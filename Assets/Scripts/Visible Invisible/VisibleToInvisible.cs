using UnityEngine;

public class VisibleToInvisible : MonoBehaviour
{

    private MeshRenderer m_meshRenderer;

    private enum ColliderChoise
    {
        Box,
        Sphere,
        Mesh
    }

    ColliderChoise m_colliderChoise;

    [SerializeField, Tooltip("Mettre le collider de l'obj si c'est un Box Collider")]private BoxCollider m_boxCollider;
    [SerializeField, Tooltip("Mettre le collider de l'obj si c'est une Sphere Collider")] private SphereCollider m_sphereCollider;
    [SerializeField, Tooltip("Mettre le collider de l'obj si c'est un Mesh Collider")] private MeshCollider m_meshCollider;

    private void Awake()
    {
        if(m_boxCollider != null)
        {
            m_colliderChoise = ColliderChoise.Box;
        }
        else if ( m_sphereCollider != null )
        {
            m_colliderChoise = ColliderChoise.Sphere;
        }
        else if( m_meshCollider != null )
        {
            m_colliderChoise = ColliderChoise.Mesh;
        }
        else
        {
            Debug.LogError("Remplit le collider Gros Chien !!!", this);
        }
    }

    private void OnEnable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler += DoVisibleToInvisible;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler -= DoVisibleToInvisible;
    }

    void DoVisibleToInvisible(bool p_start)
    {
        if (p_start)
        {
            //  Enlever ombre

            switch (m_colliderChoise)
            {
                case ColliderChoise.Box:

                    break;

                case ColliderChoise.Sphere:

                    break;

                case ColliderChoise.Mesh:

                    break;
            }
            //  Enlever Collider

        }
        else
        {
            //  Mettre ombre
            //  Mettre Collider

        }
    }
}
