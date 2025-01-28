using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_TextMeshProUGUI;
    private float m_Time = 0;

    [SerializeField]
    private AllUserDoubleJoystick factory;

    [SerializeField]
    private TextMeshProUGUI textAllUserDead;

    public bool isGameStarted = false;

    [SerializeField]
    private List<GameObject> activeUsers;

    public AllUserDoubleJoystick userDb;

    public bool isGood = true;
    public List<Team> allTeamActif;

    public bool isWin = false;

    public TextMeshProUGUI winnable;


    void Update()
    {
        if (isGameStarted)
        {
            if (isGood)
            {
                activeUsers = new List<GameObject>(userDb.allUsersGameObject);
                allTeamActif = new List<Team>(userDb.allTeam);
                isGood= false;
            }
            m_Time += Time.deltaTime;

            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(m_Time);
            m_TextMeshProUGUI.text = timeSpan.ToString(@"mm\:ss");

            if (timeSpan.TotalMinutes >= 5) 
            {
                Debug.Log("Temps �coul�, r�initialisation du jeu.");
                foreach (GameObject gameObject in activeUsers)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        Debug.Log($"{gameObject.name} est inactif et sera ignor�.");
                    }
                }
                factory.ResetGame();
            }


            for (int i = 0; i < allTeamActif.Count; i++)
            {
                for (int j = 0; j < allTeamActif[i].team.Count; j++)
                {
                    int idOfUSer = allTeamActif[i].team[j].id;

                    for (int k = 0; k < activeUsers.Count; k++)
                    {
                        PlayerTeamIdRelayMono PTIRM = activeUsers[k].GetComponentInChildren<PlayerTeamIdRelayMono>();
                        int idOfUserGameObject = PTIRM.GetTeamId();

                        if(idOfUSer == idOfUserGameObject)
                        {
                            if (!activeUsers[k].activeInHierarchy)
                            {
                                User fuckit = allTeamActif[i].team[j];
                                allTeamActif[i].team.Remove(fuckit);
                            }
                        }

                    }
                }
            }


            for (int i = 0; i < allTeamActif.Count; i++)
            {
                if (allTeamActif[i].team.Count < 1)
                {
                    allTeamActif.RemoveAt(i);
                    break;
                }
            }

            isWin = allTeamActif.Count <= 1;

            if(isWin)
            {
                int j = 0;
                int idUser =  allTeamActif[0].team[0].id;
                Color[] teamColors = {
                    Color.red,
                    Color.blue,
                    Color.green,
                    Color.HSVToRGB(255, 165 ,0),
                    Color.cyan
                };

                GameObject[] user = GameObject.FindGameObjectsWithTag("Player");

                try
                {

                    for (int i = 0; i < user.Length; i++)
                {
                    if ( int.Parse( user[i].name) == idUser)
                    {
                        PlayerColorRelayMono colorUser = user[i].GetComponent<PlayerColorRelayMono>();
                        Color color = colorUser.m_playerColor;

                        

                        for (j = 0; j < teamColors.Length; j++)
                        {
                            if(color == teamColors[j])
                            {
                                break;
                            }
                        }



                        break;
                    }
                }

                }
                catch
                {
                    Debug.Log("yooooooooooo");
                }

                string[] whatColor =
                {
                    "Rouge",
                    "Bleu",
                    "Vert",
                    "Black",
                    "Cyan"
                };


                winnable.text = $"la team gagnante est {whatColor[j]}";
                StartCoroutine(delWinableText());
            }

            for (int i = activeUsers.Count - 1; i >= 0; i--) 
            {
                
                if (textAllUserDead!=null)
                textAllUserDead.text = $"Liste des mort : \r\n";

                if (!activeUsers[i].activeInHierarchy)
                {
                    //Debug.Log($"Utilisateur {activeUsers[i].name} est d�sactiv� et sera retir�.");
                    //activeUsers.RemoveAt(i);

                    PlayerTeamIdRelayMono PTIRM = activeUsers[i].GetComponentInChildren<PlayerTeamIdRelayMono>();
                    int idOfUser = PTIRM.GetTeamId();



                    //textAllUserDead.text += $"user {idOfUser}\r\n" ;

                }
            }

        }
        else
        {
            m_Time = 0;
        }
    }


    IEnumerator delWinableText()
    {
        yield return new WaitForSeconds(5);

        Debug.Log("TG");
        userDb.ResetGame();
    }
}
