using UnityEngine;

public class QuickScript_LoadMapFromFile : MonoBehaviour
{

    public string m_subFolderPath ="Map";
    public string m_relativePathSolo="Main.png";
    public string m_relativePathTopLeft="TL.png";
    public string m_relativePathTopRight="TR.png";
    public string m_relativePathDownLeft="DL.png";
    public string m_relativePathDownRight="DR.png";

    public Material m_materialSolo;
    public Material m_materialTopLeft;
    public Material m_materialTopRight;
    public Material m_materialDownLeft;
    public Material m_materialDownRight;


    
       
}

public class QuickScript_MoveSpawnToCorner : MonoBehaviour{
        public Transform m_topLeftCorner;
        public Transform m_topRightCorner;
        public Transform m_downLeftCorner;
        public Transform m_downRightCorner;

        public RectTransform m_rectTransformTopLeft;
        public RectTransform m_rectTransformTopRight;
        public RectTransform m_rectTransformDownLeft;
        public RectTransform m_rectTransformDownRight;

}
