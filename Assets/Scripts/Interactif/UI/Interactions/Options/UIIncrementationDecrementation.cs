using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIIncrementationDecrementation : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Tooltip("True: Incrementation / False = Decrementation")]
    private bool m_isIncrementation;

    [SerializeField, Tooltip("True: Incrementation / False = Decrementation")]
    private int m_rangeValue;

    [SerializeField, Tooltip("Le scryptable objet qui contient la valeur, et qui sera lier au visuel UI")]
    private UIOptionValue m_optionValue;

    [SerializeField, Tooltip("Emitter du son Click")] private StudioEventEmitter m_clickEmitter;

    public void OnPointerClick(PointerEventData eventData)
    {
        m_clickEmitter.Play();
        
        if (m_isIncrementation)
        {
            m_optionValue.SetValue(m_rangeValue);
        }
        else
        {
            m_optionValue.SetValue(-m_rangeValue);
        }

        m_optionValue.OnUpdateText?.Invoke();
    }
}