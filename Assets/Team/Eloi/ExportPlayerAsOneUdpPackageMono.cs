using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;

public class ExportPlayerAsOneUdpPackageMono : MonoBehaviour
{

    public UnityEvent<string> m_onUdpPushAsUTF8;
    public UnityEvent<byte[]> m_onUdpPushAsByte;


    public float timeBetweenCoroutinePush = 0.2f;
   

    public List<PlayerInSceneTagMono> m_previousPlayer;
    public List<PlayerInSceneTagMono> m_currentPlayerActive;
    public List<PlayerInSceneTagMono> m_newPlayerActive;
    public List<PlayerInSceneTagMono> m_destroyPlayer;
    

    public string m_lineFormat_id_team_position_rotation = "{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}";


    // [System.Serializable]
    // public class PlayerToExport
    // {
    //     public int m_playerIndexId;
    //     public Vector3 m_localPosition;
    //     public Vector3 m_localEulerRotation;  

    //     public PlayerTeamIdRelayMono m_playerTeamIdRelayMono;
    //     public PlayerGamepadRelayMono m_playerGamepadRelayMono;
    // }


    IEnumerator Start(){
        yield return  PushPlayerAsOneUdpPackage();
    }

    public string m_textSent;
    public int m_lenghtInBytes;
    public float m_percentOfUdpMaxSize;
    private IEnumerator PushPlayerAsOneUdpPackage()
    {
        while (true)
        {
            Debug.Log("Tests");
            yield return new WaitForSeconds(timeBetweenCoroutinePush);
            yield return new WaitForEndOfFrame();
            m_previousPlayer = m_currentPlayerActive.ToList();
            m_currentPlayerActive= PlayerInSceneTagMono.playerActiveInScene.ToList();
            m_newPlayerActive = m_currentPlayerActive.Except(m_previousPlayer).ToList();
            m_destroyPlayer = m_previousPlayer.Except(m_currentPlayerActive).ToList();


            if (m_newPlayerActive.Count > 0 || m_destroyPlayer.Count > 0)
            {
                if (m_newPlayerActive.Count > 0)
                {
                    Debug.Log("New player active: " + m_newPlayerActive.Count);
                    foreach (var player in m_newPlayerActive)
                    {
                        Debug.Log("New player active: " + player.name);
                    }
                }

                if (m_destroyPlayer.Count > 0)
                {
                    Debug.Log("Destroy player active: " + m_destroyPlayer.Count);
                    foreach (var player in m_destroyPlayer)
                    {
                        Debug.Log("Destroy player active: " + player.name);
                    }
                }
            }

            StringBuilder sb=  new StringBuilder();
            sb.AppendLine("ID:TEAM:PX:PY:PZ:RX:RY:RZ");

            foreach(var playerTag in PlayerInSceneTagMono.playerActiveInScene)
            {
                // DRITY CODE
                PlayerGamepadRelayMono playerGamepadRelayMono = playerTag.GetComponent<PlayerGamepadRelayMono>();
                PlayerTeamIdRelayMono playerTeamIdRelayMono = playerTag.GetComponent<PlayerTeamIdRelayMono>();
                if(playerGamepadRelayMono==null || playerTeamIdRelayMono==null)
                    continue;
                
                int playerId = playerGamepadRelayMono.m_userIntegerId;
                int teamId = playerTeamIdRelayMono.m_teamId;

                sb.AppendLine(string.Format(
                    m_lineFormat_id_team_position_rotation,
                    playerId,
                     teamId,
                      (int)(playerTag.transform.localPosition.x*1000), 
                      (int)(playerTag.transform.localPosition.y*1000),
                       (int)(playerTag.transform.localPosition.z*1000),
                        (int)(playerTag.transform.localEulerAngles.x*1000),
                         (int)(playerTag.transform.localEulerAngles.y*1000),
                          (int)(playerTag.transform.localEulerAngles.z*1000)));
            }
            string udpMessage = sb.ToString();
            m_onUdpPushAsUTF8.Invoke(udpMessage);
            m_onUdpPushAsByte.Invoke(Encoding.UTF8.GetBytes(udpMessage));

            m_textSent = udpMessage;
            m_lenghtInBytes = Encoding.UTF8.GetBytes(udpMessage).Length;
            m_percentOfUdpMaxSize = (float)m_lenghtInBytes / 65507;

        }
    }
}

