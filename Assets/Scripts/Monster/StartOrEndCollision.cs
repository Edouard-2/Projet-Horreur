using UnityEngine;

public class StartOrEndCollision : MonoBehaviour
{
    [SerializeField, Tooltip("Layer avec lequel le player est")] private LayerMask m_layerPlayer;
    
    [SerializeField, Tooltip("True: Collider pour le déclenchement de l'IA du monstre / False: Collider pour l'arret de l'IA du monstre")] 
    private bool m_isStart;

    private bool m_readyActiveMonster = true;
    private void OnTriggerEnter(Collider other)
    {
        //Joueur qui collide ?
        //Lancer le foncitiomment de l'IA
        if ((m_layerPlayer.value & (1 << other.gameObject.layer)) > 0 && m_readyActiveMonster)
        {
            m_readyActiveMonster = false;
            if (m_isStart)
            {
                MonsterManager.Instance.StartIA();
                return;
            }
            MonsterManager.Instance.EndIA();
        }
    }
}
