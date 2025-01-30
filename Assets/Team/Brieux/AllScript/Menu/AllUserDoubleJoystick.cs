using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AllUserDoubleJoystick : MonoBehaviour
{
    public List<UserIdToDoubleJoyStick> allUser;
    public List<Team> allTeam;
    public List<User> users;
   
    public UnityEvent<UserIdToDoubleJoyStick> onValueChanged;
    public UnityEvent<UserIdToDoubleJoyStick> onNewUser;
    public GameObject prefabUser;
    public TextMeshProUGUI textAllUserConnected;
    public GameObject m_menuPanel;
    public List<GameObject> allMap;
    public List<GameObject> allUsersGameObject;

    [SerializeField]
    private Timer theTimer;

    [SerializeField]
    private GameObject map;
    private int userMax = 100;
    private bool isGamePause = false;
    private bool isGameStart = false;

    public TextMeshProUGUI win;


    public void DoAction(int action)
    {
        switch (action)
        {
            case 42:
                LaunchGame();
                break;
            case 69:
                Debug.Log("hello");
                StartCoroutine(reset());
                break;
        }
    }

    IEnumerator reset()
    {
         yield return new WaitForSeconds(1);
        ResetGame();
    }

    public void SetOrAdd(int userId , Vector2 joystickLeft, Vector2 joystickRight)
    {
        if (isGamePause) return;

        for (int i = 0; i < allUser.Count; i++)
        {
            if (allUser[i].id == userId)
            {
                if (i < allUsersGameObject.Count)
                {
                    allUser[i].joystickLeft = joystickLeft;
                    allUser[i].joystickRight = joystickRight;
                    PlayerGamepadRelayMono PGM = allUsersGameObject[i].GetComponentInChildren<PlayerGamepadRelayMono>();

                    if (PGM != null)
                    {
                        PGM.PushInGamepadValue(userId, joystickLeft, joystickRight);
                    }

                    onValueChanged.Invoke(allUser[i]);
                }
                return;

            }
        }

        if (allUser.Count >= userMax)
        {
            return;
        }

        UserIdToDoubleJoyStick newUser = new UserIdToDoubleJoyStick { id = userId, joystickLeft = joystickLeft, joystickRight = joystickRight };
        allUser.Add(newUser);
        onNewUser.Invoke(newUser);
        onValueChanged.Invoke(newUser);
        User tempUser = new User();
        tempUser.id = userId;
        users.Add( tempUser);
        //textAllUserConnected.text += $"user avec id : {userId}\r\n"; 
        textAllUserConnected.text += " |";
    }

    public Color[] teamColors = {
                Color.red,
                Color.blue,
                Color.green,
                Color.yellow,
                Color.cyan
            };
    public void PushIntegerAction(int userId, int action)
    {
        if (isGamePause) return; 

        for (int i = 0; i < allUser.Count; i++)
        {
            if (allUser[i].id == userId)
            {
                if(i < allUsersGameObject.Count)
                {
                    PlayerGamepadRelayMono gamepadUser = allUsersGameObject[i].GetComponent<PlayerGamepadRelayMono>();
                    gamepadUser.PushInIntegerAction(userId, action);
                    allUser[i].lastReceived = action;
                }
                return;
            }
        }

        UserIdToDoubleJoyStick newUser = new UserIdToDoubleJoyStick { id = userId, joystickLeft = Vector2.zero , joystickRight = Vector2.zero };

        allUser.Add(newUser);
        onNewUser.Invoke(newUser);
        onValueChanged.Invoke(newUser);
        User tempUser = new User();
        tempUser.id = userId;
        users.Add(tempUser);
        //textAllUserConnected.text += $"user avec id : {userId}\r\n";
        textAllUserConnected.text += " |";

    }

    public void LaunchGame()
    {
        if (CreateTeam())
        {
            if (allTeam.Count >= 2)
            {
                if (allMap.Count < allTeam.Count - 1)
                {
                    map = Instantiate(allMap[0]);
                }
                else{
                    map = Instantiate(allMap[allTeam.Count - 2]);
                }
            }
            else
            {
                Debug.LogError("Not enough teams to select a map.");
                return;
            }
            theTimer.isGameStarted = true;

            List<Vector3> teamPositions = new List<Vector3>();
            GameObject[] spawner = GameObject.FindGameObjectsWithTag("Spawn");

            

            foreach (var spawn in spawner)
            {
                teamPositions.Add(spawn.transform.position + new Vector3(0,1,0));
            }

            

            m_menuPanel.SetActive (false);
            for (int teamIndex = 0; teamIndex < allTeam.Count; teamIndex++)
            {
                Team team = allTeam[teamIndex];

                for (int i = 0; i < team.User.Count; i++)
                {
                    GameObject gameobjectUser = Instantiate(prefabUser);
                    gameobjectUser.name = $"{team.User[i].id}";


                    PlayerTeamIdRelayMono idOfPlayer = gameobjectUser.gameObject.GetComponentInChildren<PlayerTeamIdRelayMono>();
                    PlayerColorRelayMono colorPlayer = gameobjectUser.GetComponentInChildren<PlayerColorRelayMono>();
                    PlayerGamepadRelayMono gamepadPlayer = gameobjectUser.GetComponentInChildren<PlayerGamepadRelayMono>();

                    

                    idOfPlayer?.SetTeamId(teamIndex);
                    colorPlayer?.SetColor(teamColors[teamIndex]);
                    gamepadPlayer?.PushInGamepadValue(team.User[i].id, Random.insideUnitCircle, Random.insideUnitCircle);

                    //Vector3 offset = new Vector3(i * 1, 0, 0);

                    Vector3 offset;
                    if (i == 0)
                    {
                        offset = Vector3.zero;
                    }
                    else
                    {
                        float angle = (360f / team.User.Count) * (i - 1); 
                        float radius = 2f; 
                        offset = new Vector3(
                            Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 
                            1.5f,
                            Mathf.Sin(angle * Mathf.Deg2Rad) * radius  
                        );
                    }

                    gameobjectUser.transform.position = teamPositions[teamIndex] + offset;

                    Renderer renderer = gameobjectUser.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = teamColors[teamIndex];
                    }

                    allUsersGameObject.Add(gameobjectUser);

                    Debug.Log($"Utilisateur {team.User[i].id} positionné en {gameobjectUser.transform.position}");
                }
            }
        }
        else
        {
            Debug.LogError("Probleme.");
        }
    }

    public bool CreateTeam()
    {
        int sizeUser = users.Count;
        allTeam = new List<Team>();
        int numberOfTeams;

        if (sizeUser < 1)
        {
            Debug.LogError("Il n'y a pas assez de personnes pour créer une équipe.");
            return false;
        }

        if (sizeUser < 9)
        {
            Debug.Log("Création de 2 équipes.");
            numberOfTeams = 2;
        }
        else if (sizeUser < 15)
        {
            Debug.Log("Création de 3 équipes.");
            numberOfTeams = 3;
        }
        else if (sizeUser < 20)
        {
            Debug.Log("Création de 4 équipes.");
            numberOfTeams = 4;
        }
        else if (sizeUser <= userMax)
        {
            Debug.Log("Création de 4 équipes.");
            numberOfTeams = 5;
        }
        else
        {
            Debug.LogError("Vous avez trop de personnes.");
            return false;
        }

        for (int i = 0; i < numberOfTeams; i++)
        {
            allTeam.Add(new Team { User = new List<User>() });
        }

        for (int i = 0; i < sizeUser; i++)
        {
            int teamIndex = i % numberOfTeams;
            allTeam[teamIndex].User.Add(users[i]);
        }
        return true;
    }

    public void PauseGame()
    {
        Debug.Log("je met le jeu en pause.");
        isGamePause= true;
    }

    public void ResetGame()
    {
        GameObject.Destroy(map);
        isGamePause = false;
        theTimer.isGameStarted = false;
        foreach (GameObject user in allUsersGameObject)
        {
            user.SetActive(true);
            GameObject.Destroy(user);
        }
        allUsersGameObject.Clear();

        theTimer.isGood = true;

        allUser.Clear();
        allTeam.Clear();
        users.Clear();
        textAllUserConnected.text = "ALL USER CONNECTED :\r\n";
        m_menuPanel.SetActive(true);
        win.text = "";
    }
}


[System.Serializable]
public class RelayDoubleGamepadEvent : UnityEvent<int, Vector2, Vector2> { }
