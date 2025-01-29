using System.Collections.Generic;
using UnityEngine;

public class PlayerInSceneTagMono : MonoBehaviour
{
    
    public static List<PlayerInSceneTagMono> allPlayerInScene = new List<PlayerInSceneTagMono>();
    public static List<PlayerInSceneTagMono> playerActiveInScene = new List<PlayerInSceneTagMono>();

    void Reset()
    {
        m_playerRoot = transform;
    }
    public Transform m_playerRoot;
    
    public void Awake()
    {
        allPlayerInScene.Add(this);
    }

    public void OnDestory()
    {
        allPlayerInScene.Remove(this);
    }
    public void OnEnable()
    {
        playerActiveInScene.Add(this);
    }
    public void OnDisable()
    {
        playerActiveInScene.Remove(this);
    }
}
