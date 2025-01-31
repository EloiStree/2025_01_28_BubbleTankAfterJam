using UnityEngine;
using UnityEngine.Events;

public class QuickScript_BooleanSpliter: MonoBehaviour
{


    public bool m_value;
    public UnityEvent m_onTrue;
    public UnityEvent m_onFalse;

    public UnityEvent<bool> m_onValueChanged;

    void Start()
    {
        PushIn(m_value);
    }
    public void PushIn(bool value)
    {
        if (m_value != value){
            m_value = value;
            if (m_value)
                m_onTrue.Invoke();
            else
                m_onFalse.Invoke();
            
            m_onValueChanged.Invoke(m_value);
        }
    }
}