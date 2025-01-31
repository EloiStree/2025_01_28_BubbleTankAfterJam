using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class BroadcastToAllThePackageUdpMono : MonoBehaviour
{
    [TextArea(0,16)]
    public string m_targetsIpv4Port="127.0.0.1:8008";

    public List<TargetIPV4> m_targets = new List<TargetIPV4>();

    [System.Serializable]
    public class TargetIPV4{
        public string ipv4;
        public int port;

    }



public bool m_usePlayerPrefs = true;
    public string m_playerPrefId="IPV4";

    void OnEnable()
    {
        if(m_usePlayerPrefs)
        m_targetsIpv4Port = PlayerPrefs.GetString(m_playerPrefId, m_targetsIpv4Port);
        RefreshList();
    }

    void OnDisable()
    {        if(m_usePlayerPrefs)

        PlayerPrefs.SetString(m_playerPrefId, m_targetsIpv4Port);
    }
    void OnDestroy()
    {        if(m_usePlayerPrefs)

        PlayerPrefs.SetString(m_playerPrefId, m_targetsIpv4Port);
    }
   

    public void SetTargetWithText(string text){

        m_targetsIpv4Port = text;
        RefreshList();        if(m_usePlayerPrefs)

        PlayerPrefs.SetString(m_playerPrefId, m_targetsIpv4Port);

        
    }

    void RefreshList()
    {
        m_targets.Clear();
        string[] lines = m_targetsIpv4Port.Split('\n');
        foreach (var line in lines)
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2 && int.TryParse(parts[1], out int port))
            {
                m_targets.Add(new TargetIPV4 { ipv4 = parts[0], port = port });
            }
        }
    }

    public string m_last_PushAsUTF8;

    public void PushBytesIn(byte[] bytes)
    {
        using (UdpClient client = new UdpClient())
        {
            foreach (var target in m_targets)
            {
                try
                {
                    client.Send(bytes, bytes.Length, target.ipv4, target.port);
                }
                catch (SocketException ex)
                {
                    Debug.LogError($"Failed to send to {target.ipv4}:{target.port} - {ex.Message}");
                }
            }
            client.Close();
        }
        m_last_PushAsUTF8 = DateTime.Now.ToString("HH:mm:ss") + " " + System.Text.Encoding.UTF8.GetString(bytes);
    }
    
}
