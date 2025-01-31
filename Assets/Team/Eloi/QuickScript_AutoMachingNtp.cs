using Eloi.IID;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuickScript_AutoMachingNtp : MonoBehaviour
{
    public long m_timestampMilliseconds = 0;
    public long m_ntpOffsetLocalToServerInMilliseconds;
    public long m_timestampMillisecondsNTP = 0;

    public long m_timeSinceGameStarted;


    public void SetOffset(long ntpOffsetLocalToServerInMilliseconds)
    {
        m_ntpOffsetLocalToServerInMilliseconds = ntpOffsetLocalToServerInMilliseconds;
    }
    public void SetOffset(int ntpOffsetLocalToServerInMilliseconds)
    {
        m_ntpOffsetLocalToServerInMilliseconds = ntpOffsetLocalToServerInMilliseconds;
    }
    void Update()
    { 
        NtpOffsetFetcher.GetCurrentTimeAsMillisecondsLocalUtc(out m_timestampMilliseconds);
        NtpOffsetFetcher.GetCurrentTimeAsMillisecondsUtcNtp(
            m_timestampMilliseconds,
            m_ntpOffsetLocalToServerInMilliseconds,
            out m_timestampMillisecondsNTP);

            m_totaleMilliSeconds = m_matchInMilliseconds + m_lobbyInMilliSeconds + m_warmingNextMatch;
            m_timeSinceGameStarted = m_timestampMillisecondsNTP % m_totaleMilliSeconds;
            bool previousInLobby = m_isInLobby;
            bool previousInMatch = m_isInMatch;
            bool previousInWarmingNextMatch = m_isWarmingNextMatch;
            
             if (m_timeSinceGameStarted <m_lobbyInMilliSeconds)
            {
                m_isInMatch = false;
                m_isInLobby = true;
                m_isWarmingNextMatch = false;
                m_timeLeftInMilliseconds = (m_lobbyInMilliSeconds)- (int)m_timeSinceGameStarted;
            }
            else if (m_timeSinceGameStarted >=m_lobbyInMilliSeconds && m_timeSinceGameStarted <= m_totaleMilliSeconds-m_warmingNextMatch)
            {
                m_isInMatch = true;
                m_isInLobby = false;
                m_isWarmingNextMatch = false;
                m_timeLeftInMilliseconds = m_totaleMilliSeconds - m_warmingNextMatch - (int)m_timeSinceGameStarted;
            }
           
            else if (m_timeSinceGameStarted>m_matchInMilliseconds+m_lobbyInMilliSeconds)
            {
                m_isInMatch = false;
                m_isInLobby = false;
                m_isWarmingNextMatch = true;
                m_timeLeftInMilliseconds = m_totaleMilliSeconds - (int)m_timeSinceGameStarted;
            }

            if (previousInLobby != m_isInLobby)
                m_onInLobbyChanged.Invoke(m_isInLobby);
            if (previousInMatch != m_isInMatch)
                m_onInMatchChanged.Invoke(m_isInMatch);
            if (previousInWarmingNextMatch != m_isWarmingNextMatch)
                m_onInWarmingNextMatchChanged.Invoke(m_isWarmingNextMatch);

            m_timeLeftBeforeNext.Invoke(m_timeLeftInMilliseconds);

            
    }

    public bool m_isInLobby;
    public bool m_isInMatch;
    public bool m_isWarmingNextMatch;

    public int m_matchInMilliseconds = 55000;
    public int m_lobbyInMilliSeconds =10000;
    public int m_warmingNextMatch = 5000;

    public int m_totaleMilliSeconds;  
    public int m_timeLeftInMilliseconds;

    public  UnityEvent<bool> m_onInLobbyChanged;
    public  UnityEvent<bool> m_onInMatchChanged;
    public  UnityEvent<bool> m_onInWarmingNextMatchChanged;

    public UnityEvent<int> m_timeLeftBeforeNext;
    
}
