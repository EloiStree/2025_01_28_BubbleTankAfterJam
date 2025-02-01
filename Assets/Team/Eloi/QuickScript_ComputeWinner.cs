using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class QuickScript_ComputeWinner : MonoBehaviour
{
    public TeamInfo[] m_teams = new TeamInfo[]{
        new TeamInfo(){m_name="Team Red",m_teamId=0,m_score=0,m_color=Color.red},
        new TeamInfo(){m_name="Team Green",m_teamId=1,m_score=0,m_color=Color.green},
        new TeamInfo(){m_name="Team Blue",m_teamId=2,m_score=0,m_color=Color.blue},
        new TeamInfo(){m_name="Team Yellow",m_teamId=3,m_score=0,m_color=Color.yellow},
        new TeamInfo(){m_name="Team Cyan",m_teamId=4,m_score=0,m_color=Color.cyan}
    };

    public UnityEvent<string> m_onTextMeshProColorScore;

    [System.Serializable]
    public class TeamInfo
    {
        public string m_name;
        public int m_teamId;
        public int m_score;
        public Color m_color;
    }

    public PlayerInSceneTagMono[] m_players;

    public string m_lastScore;

    [ContextMenu("Compute Winner")]
    public void ComputeWinner()
    {
        Dictionary<int, int> teamIdToScore = new Dictionary<int, int>();
        m_players = PlayerInSceneTagMono.playerActiveInScene.ToArray();

        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_players[i] == null)
                continue;
            PlayerTeamIdRelayMono team = m_players[i].GetComponent<PlayerTeamIdRelayMono>();
            if (team == null)
                continue;

            if (teamIdToScore.ContainsKey(team.m_teamId))
            {
                teamIdToScore[team.m_teamId]++;
            }
            else
            {
                teamIdToScore.Add(team.m_teamId, 1);
            }
        }
        int teamId=0;
        int minScore=0;
        foreach (var item in teamIdToScore)
        {
            if (item.Value > minScore)
            {
                minScore = item.Value;
                teamId = item.Key;
            }
        }


        m_lastScore = "";
        foreach(var item in teamIdToScore)
        {
            m_lastScore += m_teams[item.Key].m_name + " : " + item.Value ;
        }
        int winnerCount = 0;
        for (int i = 0; i < m_teams.Length; i++)
        {
            if (teamIdToScore.ContainsKey(m_teams[i].m_teamId) == false)
                continue;
            if(teamIdToScore[m_teams[i].m_teamId] == minScore)
            {
                winnerCount++;
                TeamInfo teamInfo;
                GetTeamInfo(m_teams[i].m_teamId, out teamInfo);
                teamInfo.m_score++;
            }   
        }
        if (winnerCount == 1)
        {
            m_onTeamIdWon.Invoke(teamId);
            m_onColorWon.Invoke(m_teams[teamId].m_color);
            m_lastScore += "Winner is " + m_teams[teamId].m_name + " with " + m_teams[teamId].m_score + " points";
            m_onWinnderDebug.Invoke(m_lastScore); 
        }
        else
        {
            m_onNoWinner.Invoke();
            m_onColorWon.Invoke(Color.white);    
             m_onWinnderDebug.Invoke(m_lastScore); 
        }

        StringBuilder sb=  new StringBuilder();
        for (int i = 0; i < m_teams.Length; i++)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(m_teams[i].m_color);
            sb.Append($"<#{hexColor}>{m_teams[i].m_score} ");
           }
        m_onTextMeshProColorScore.Invoke(sb.ToString());
    }

    public UnityEvent<string> m_onWinnderDebug;
    public UnityEvent<int> m_onTeamIdWon;
    public UnityEvent<Color> m_onColorWon;
    public UnityEvent m_onNoWinner;

    void GetTeamInfo(int teamId,out TeamInfo teamInfo)
    {
        teamInfo = new TeamInfo();
        for (int i = 0; i < m_teams.Length; i++)
        {
            if (m_teams[i].m_teamId == teamId)
            {
                teamInfo = m_teams[i];
                return;
            }
        }
    }
}
