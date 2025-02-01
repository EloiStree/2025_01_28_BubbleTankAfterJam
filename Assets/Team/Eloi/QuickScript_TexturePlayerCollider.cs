using UnityEngine;

public class QuickScript_TexturePlayerCollider : MonoBehaviour
{

    public Texture2D m_mapTexture;

    public void SetMapTexture(Texture2D texture)
    {
        m_mapTexture = texture;
    }

    public bool IsColliding(float percentXLeft2Right,float percentYDown2Top){

        if(m_mapTexture==null)
            return false;

        int x = Mathf.FloorToInt(m_mapTexture.width * percentXLeft2Right);
        int y = Mathf.FloorToInt(m_mapTexture.height * percentYDown2Top);

        if(x<0 || x>=m_mapTexture.width || y<0 || y>=m_mapTexture.height)
            return false;

        Color color = m_mapTexture.GetPixel(x,y);
        return color.a>0.5f;

    }

}
