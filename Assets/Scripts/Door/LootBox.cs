using System;
using System.Collections;
using UnityEngine;

public class LootBox : MonoBehaviour, ILootBox
{
    [Tooltip("La clé du coffre")]
    public KeyType m_key;

    [SerializeField, Tooltip("L'animator du coffre")]
    private Animator m_doorAnimator;

    [SerializeField, Tooltip("Le nom du trigger de l'animation")]
    private string m_openName;

    private int m_openHash;
    private bool m_dir;
    private float m_upFunction;

    private void OnEnable()
    {
        PlayerManager.Instance.DoRotateKeys += RotateSelf;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.DoRotateKeys -= RotateSelf;
    }

    private void Awake()
    {
        if (m_doorAnimator == null)
        {
            m_doorAnimator = GetComponent<Animator>();
            if (m_doorAnimator == null)
            {
                Debug.Log("Gros chien l'animator", this);
            }
        }

        m_openHash = Animator.StringToHash(m_openName);

        if (m_key)
        {
            transform.GetChild(0).GetComponent<Renderer>().material = m_key.m_keyMat;
            //transform.GetChild(0).GetComponent<Renderer>().material.color = m_key.m_keyColor;
        }
    }

    private void RotateSelf()
    {
        transform.Rotate(0, 30f * Time.deltaTime, 0);
    }

    public bool OpenChest(out KeyType o_key)
    {
        bool keyFounded = false;
        //Loot de la clé m_key 
        if (m_key == null)
        {
            Debug.Log("Pas de clé déso bb");
        }
        else
        {
            keyFounded = true;
            Debug.Log($"Tu a recu la clé {m_key}");
        }

        o_key = m_key;
        //m_doorAnimator.SetTrigger(m_openHash);

        StartCoroutine(DestroySelf());

        return keyFounded;
    }

    public IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0f);
        transform.position = new Vector3(10000, 10000, 10000);
    }
}