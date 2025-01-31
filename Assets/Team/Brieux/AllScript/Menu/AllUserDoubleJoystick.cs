using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AllUserDoubleJoystick : MonoBehaviour
{
    public GameObject prefabUser;
    public TextMeshProUGUI textAllUserConnected;
    public GameObject m_menuPanel;
    private bool isGamePause = false;
    private bool isGameStart = false;

    public TextMeshProUGUI win;

    public UnityEvent m_onGameStart;
    public UnityEvent m_onBeforeGameStop;
    public UnityEvent m_onAfterGameStop;


public TeamInfo [] m_teamInfo = new TeamInfo[]{
    new TeamInfo{m_teamId=0, m_teamName="Red", m_color=Color.red},
    new TeamInfo{m_teamId=1, m_teamName="Green", m_color=Color.green},
    new TeamInfo{m_teamId=2, m_teamName="Blue", m_color=Color.blue},
    new TeamInfo{m_teamId=3, m_teamName="Yellow", m_color=Color.yellow},
    new TeamInfo{m_teamId=4, m_teamName="Cyan", m_color=Color.cyan}
};

[System.Serializable]
public class TeamInfo{

    public string m_teamName;
    public int m_teamId;
    public  Color m_color;
    public  Transform m_parent;
    public Transform m_spawnPoint;
}
 
 public List<int> m_playerInLobby= new List<int>();

    public List<GameobjectToScriptIndexPlayer> m_playerIngame;
    [System.Serializable]
    public class GameobjectToScriptIndexPlayer
    {
        public int m_playerIndex;
        public GameObject m_created;
        public PlayerInSceneTagMono m_tag;
        public PlayerGamepadRelayMono m_gamepad;
        public PlayerColorRelayMono m_color;
        public PlayerTeamIdRelayMono m_teamId;
    }


    public void SetOrAdd(int userId, Vector2 joystick){

        SetOrAdd(userId, joystick,joystick);
    }
    public void SetOrAdd(int userId , Vector2 joystickLeft, Vector2 joystickRight)
    {
        if (isGamePause) return;

        if (m_playerInLobby.Contains(userId) == false)
        {
            m_playerInLobby.Add(userId);
            textAllUserConnected.text += " |";
        }

        foreach (var user in m_playerIngame)
        {
            if (user.m_playerIndex == userId)
            {
                user.m_gamepad.PushInGamepadValue(userId, joystickLeft, joystickRight);
                return;
            }
        }
    }

    public void PushIntegerAction(int userId, int action)
    {
        if (isGamePause) return; 

        if (m_playerInLobby.Contains(userId) == false)
        {
            m_playerInLobby.Add(userId);
            textAllUserConnected.text += " |";
        }

        foreach (var user in m_playerIngame){
            if (user.m_playerIndex == userId)
            {
                    user.m_gamepad.PushInIntegerAction(userId,action);     
                    return;       
            }
        }
    }

    public bool m_gameStarted=false;

    public float m_randomRadius =3;
    public void LaunchGame()
    {
       

            m_gameStarted = true;
          
            m_menuPanel.SetActive (false);
            int teamIndex = 0;
            foreach (var userIndex in m_playerInLobby)
            {
                    int teamClaim = teamIndex % m_teamInfo.Length;
                    Transform spawn  = m_teamInfo[teamClaim].m_spawnPoint;
                    Transform parent = m_teamInfo[teamClaim].m_parent;
                    GameObject gameobjectUser = Instantiate(prefabUser);
                    gameobjectUser.transform.SetParent(parent);

                    gameobjectUser.name = $"{userIndex}";
                    gameobjectUser.transform.position = spawn.position + new Vector3(Random.Range(-m_randomRadius, m_randomRadius), 0, Random.Range(-m_randomRadius, m_randomRadius));
                    gameobjectUser.transform.rotation = spawn.rotation;

                    var c = new GameobjectToScriptIndexPlayer { 
                        m_playerIndex = userIndex,
                         m_created = gameobjectUser,
                          m_tag = gameobjectUser.GetComponentInChildren<PlayerInSceneTagMono>(),
                           m_gamepad = gameobjectUser.GetComponentInChildren<PlayerGamepadRelayMono>(),
                            m_color = gameobjectUser.GetComponentInChildren<PlayerColorRelayMono>(), 
                            m_teamId = gameobjectUser.GetComponentInChildren<PlayerTeamIdRelayMono>() 
                            };
                    c.m_teamId?.SetTeamId(teamClaim);
                    c.m_color?.SetColor(m_teamInfo[teamClaim].m_color);
                    c.m_gamepad?.PushInGamepadValue(userIndex, Vector2.zero, Vector2.zero);
                    m_playerIngame.Add(c);

                    teamIndex++;
            }
            m_onGameStart.Invoke();
       
    }

   


    public void PauseGame()
    {
        Debug.Log("je met le jeu en pause.");
        isGamePause= true;
    }

    public void ResetGame()
    {
        m_onBeforeGameStop.Invoke();
        m_playerInLobby.Clear();
        isGamePause = false;
        isGameStart = false;
        foreach (var user in m_playerIngame)
        {
            Destroy(user.m_created);
        }
        m_playerIngame.Clear();
        m_playerInLobby.Clear();
        textAllUserConnected.text = "ALL USER CONNECTED :\r\n";
        m_menuPanel.SetActive(true);
        win.text = "";
        m_onAfterGameStop.Invoke();
    }
}


[System.Serializable]
public class RelayDoubleGamepadEvent : UnityEvent<int, Vector2, Vector2> { }
