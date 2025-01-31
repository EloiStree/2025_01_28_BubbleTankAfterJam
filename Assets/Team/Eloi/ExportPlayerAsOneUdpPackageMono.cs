
using System.Collections;
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
    
    public string m_lineFormat_id_team_position_rotation = "{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}";


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

    public int m_playerInScene;


    public void NotifyGameStart()
    {
            m_previousPlayer.Clear();
        m_currentPlayerActive.Clear();
        m_newPlayerActive.Clear();
        m_destroyPlayer.Clear();
    }
    public void NotifyGameStop()
    {
        m_previousPlayer.Clear();
        m_currentPlayerActive.Clear();
        m_newPlayerActive.Clear();
        m_destroyPlayer.Clear();
    }
    private IEnumerator PushPlayerAsOneUdpPackage()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenCoroutinePush);
            yield return new WaitForEndOfFrame();
            m_previousPlayer = m_currentPlayerActive.ToList();
            m_currentPlayerActive= PlayerInSceneTagMono.playerActiveInScene.Where(k=>k!=null).ToList();
            m_newPlayerActive = m_currentPlayerActive.Except(m_previousPlayer).ToList();
            m_destroyPlayer = m_previousPlayer.Except(m_currentPlayerActive).ToList();


            if (m_newPlayerActive.Count > 0 || m_destroyPlayer.Count > 0)
            {
                if (m_newPlayerActive.Count > 0)
                {
                    //Debug.Log("New player active: " + m_newPlayerActive.Count);
                    foreach (var player in m_newPlayerActive)
                    {
//                        Debug.Log("New player active: " + player.name);
                    }
                }

                if (m_destroyPlayer.Count > 0)
                {
  //                  Debug.Log("Destroy player active: " + m_destroyPlayer.Count);
                    foreach (var player in m_destroyPlayer)
                    {
                        if(player==null)
                            continue;
    //                    Debug.Log("Destroy player active: " + player.name);
                    }
                }
            }

            StringBuilder sb=  new StringBuilder();
            sb.AppendLine("ID:TEAM:PX:PY:PZ:RX:RY:RZ:RADIUS:XTOZANGLE");

            foreach(var playerTag in m_currentPlayerActive)
            {
                if (playerTag==null)
                    continue;
                // DRITY CODE
                PlayerGamepadRelayMono playerGamepadRelayMono = playerTag.GetComponent<PlayerGamepadRelayMono>();
                PlayerTeamIdRelayMono playerTeamIdRelayMono = playerTag.GetComponent<PlayerTeamIdRelayMono>();
                PlayerRadiusSizeFetchMono size = playerTag.GetComponent<PlayerRadiusSizeFetchMono>();
    
                if(playerGamepadRelayMono==null || playerTeamIdRelayMono==null || size==null)
                    continue;
                
                int playerId = playerGamepadRelayMono.m_userIntegerId;
                int teamId = playerTeamIdRelayMono.m_teamId;

                Vector3 dir = playerTag.transform.forward;
                dir.y = 0;

                float floatAngleClassicMath = -Vector3.SignedAngle(Vector3.right, dir, Vector3.up);



                sb.AppendLine(string.Format(
                    m_lineFormat_id_team_position_rotation,
                    playerId,
                     teamId,
                      (int)(playerTag.transform.position.x*1000), 
                      (int)(playerTag.transform.position.y*1000),
                       (int)(playerTag.transform.position.z*1000),
                        (int)(playerTag.transform.eulerAngles.x*1000),
                         (int)(playerTag.transform.eulerAngles.y*1000),
                          (int)(playerTag.transform.eulerAngles.z*1000),
                           (int)(size.m_currentSize*1000),
                           (int)(floatAngleClassicMath*1000)
                           ));
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

