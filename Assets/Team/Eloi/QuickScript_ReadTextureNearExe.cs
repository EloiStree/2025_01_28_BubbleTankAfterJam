using UnityEngine;
using UnityEngine.Events;

public class QuickScript_ReadTextureNearExe : MonoBehaviour
{


    public string m_windowPathRelative=  "Map.png";
    public string m_editorPathRelative=  "Map.png";
    
    public UnityEvent<Texture2D> m_onReadIPV4;
    public void Awake()
    {
        string path = Application.dataPath + "/" + m_editorPathRelative;
        if (Application.isEditor == false)
        {
            path = Application.dataPath + "/../" + m_windowPathRelative;
        }
        if (!System.IO.File.Exists(path))
        {
            Texture2D texture2D = new Texture2D(1920/4, 1080/4);
            for (int x = 0; x < texture2D.width; x++)
            {
                for (int y = 0; y < texture2D.height; y++)
                {
                    texture2D.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
            System.IO.File.WriteAllBytes(path, texture2D.EncodeToPNG());
        }
        if (System.IO.File.Exists(path))
        {

            byte[] fileData = System.IO.File.ReadAllBytes(path);
            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(fileData);
            m_onReadIPV4.Invoke(texture2D);
        }
       
    }
}
