using System;
using UnityEngine;
using UnityEngine.Events;

public class QuickScript_TextureColliderMap : MonoBehaviour
{

    public Texture2D m_mapTexture;
    public Transform m_downLeftCorner;
    public Transform m_topRightCorner;

    public UnityEvent<Texture2D> m_onApplyTextureChanged;

    public Renderer [] m_renderers;

    public void SetMapTexture(Texture2D texture)
    {
        m_mapTexture = texture;
        m_onApplyTextureChanged.Invoke(m_mapTexture);
        foreach (var r in m_renderers)
        if (r!=null && r.material!=null)
            r.material.mainTexture= texture;
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

    internal bool IsColliding(Transform target)
    {

        if(m_downLeftCorner==null || m_topRightCorner==null)
            return false;

        Eloi.RelocationUtility.GetWorldToLocal_DirectionalPoint
        (
            target.position,
            target.rotation,
            m_downLeftCorner,
            out Vector3 localPos,
            out Quaternion rotatoin
        );
        Eloi.RelocationUtility.GetWorldToLocal_DirectionalPoint
        (
            m_topRightCorner.position,
            m_topRightCorner.rotation,
            m_downLeftCorner,
            out Vector3 localPosCorner,
            out Quaternion rotatoinCorner
        );

        float percentXLeft2Right = localPos.x / localPosCorner.x;
        float percentYDown2Top = localPos.y / localPosCorner.y;
        return IsColliding(percentXLeft2Right,percentYDown2Top);        
    }
}
