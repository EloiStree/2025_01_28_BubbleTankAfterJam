using UnityEngine;
using UnityEngine.Events;

public class QuickScriptUI_TimeLeftInMilliseconds : MonoBehaviour
{
    public int m_timeLeftInMilliseconds;
    public string m_format= "{0:00}:{1:00}";
    public UnityEvent<string> m_onTimeLeftChanged;
    public void SetMillisecondsLeft(int timeInMS)
    {
        m_timeLeftInMilliseconds = timeInMS;
        m_onTimeLeftChanged.Invoke(string.Format(m_format, timeInMS / 60000, (timeInMS % 60000) / 1000));

    }

}