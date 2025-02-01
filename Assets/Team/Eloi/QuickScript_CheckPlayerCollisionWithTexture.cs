using System.Collections.Generic;
using UnityEngine;

public class QuickScript_CheckPlayerCollisionWithTexture: MonoBehaviour{

    public QuickScript_TextureColliderMap m_collider;

    public List<PlayerInSceneTagMono> m_playersInGame;

    
    public void RefreshPlayerList(){
        m_playersInGame = PlayerInSceneTagMono.allPlayerInScene;
    }    

    public void CheckCollision(){

        RefreshPlayerList();
        for(int i =0; i< m_playersInGame.Count; i++){
            if (m_playersInGame[i]==null || ! m_playersInGame[i].gameObject.activeInHierarchy)
                continue;
            
            if(m_collider.IsColliding(m_playersInGame[i].transform))
               m_playersInGame[i].gameObject.SetActive(false);
            
            

        }

    }


}
