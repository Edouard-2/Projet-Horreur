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


    public void OnPointerClick(PointerEventData eventData)
    {
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