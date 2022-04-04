using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Tooltip("La clé du coffre")]
    public KeyType m_key;

    private Vector3 m_initPos;

    private void OnEnable()
    {
        PlayerManager.Instance.DoRotateKeys += RotateSelf;
        PlayerManager.Instance.UpdateFirstPos += RotateSelf;
    }

    private void Awake()
    {
        m_initPos = transform.position;
        
        if (m_key)
        {
            transform.GetChild(0).GetComponent<Renderer>().material = m_key.m_keyMat;
            m_key.m_doorMat.SetFloat("_isAim", 0);
            m_key.m_keyMat.SetFloat("_isAim", 0);
        }
    }

    private void RotateSelf()
    {
        transform.Rotate(0, 30f * Time.deltaTime, 0);
    }

    private void SetInitPos()
    {
        transform.position = m_initPos;
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