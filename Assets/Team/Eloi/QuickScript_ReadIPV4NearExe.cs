using UnityEngine;
using UnityEngine.Events;

public class QuickScript_ReadIPV4NearExe : MonoBehaviour
{


    public string m_windowPathRelative=  "IVP4BROADCAST.txt";
    public string m_editorPathRelative=  "IVP4BROADCAST.txt";
    
    
    public UnityEvent<string> m_onReadIPV4;
    public void Awake()
    {
        string path = Application.dataPath + "/" + m_editorPathRelative;
        if (Application.isEditor == false)
        {
            path = Application.dataPath + "/../" + m_windowPathRelative;
        }
        if (!System.IO.File.Exists(path))
        {
            System.IO.File.WriteAllText(path, "12.0.0.1:8001");
        }
        if (System.IO.File.Exists(path))
        {
            string text = System.IO.File.ReadAllText(path);
            m_onReadIPV4.Invoke(text);
        }
       
    }
}
