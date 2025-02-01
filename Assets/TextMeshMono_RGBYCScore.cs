using System;
using TMPro;
using UnityEngine;
public class TextMeshMono_SetText : MonoBehaviour
{
    void Reset()
    {
        m_textMesh = GetComponents<TMP_Text>();
    }
    public TMP_Text [] m_textMesh;
    public string m_lastReceived;
    public void PushIn(string text){
        m_lastReceived = text;
        for (int i = 0; i < m_textMesh.Length; i++)
        {
            if (m_textMesh[i] == null)
                continue;
            m_textMesh[i].text = text;
        }
    }
    public void PushInIID(int integer){
        PushIn(integer.ToString());
    }
    public void PushInIID(int index, int integer, string format="{0} {1}"){
        PushIn(string.Format(format, index, integer));
    }
    public void PushInIID(int index, int integer, ulong date, string format="{0} {1} {2}"){
        PushIn(string.Format(format, index, integer, date));
    }
    public void PushInIID(int index, int integer, DateTime date, string format="{0} {1} {2}", string dateFormat="yyyy-MM-dd HH:mm:ss"){
        PushIn(string.Format(format, index, integer, date.ToString(dateFormat)));
    }
}

