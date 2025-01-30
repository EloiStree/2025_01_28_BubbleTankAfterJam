using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class BroadcastToAllThePackageUdpMono : MonoBehaviour
{
    [TextArea(0,16)]
    public string m_targetsIpv4Port;

    public List<TargetIPV4> m_targets = new List<TargetIPV4>();

[System.Serializable]
    public class TargetIPV4{
        public string ipv4;
        public int port;

    }
    public void PushBytesIn(byte [] toPush){

        
    }

    // void Start(){

    //     m_targets = new List<TargetIPV4>();
    //     string []lines = m_targetsIpv4Port.Split('\n');
    //     foreach(var l in lines ){
    //             string [] to = l.Split(':');
    //             if(to.Length==2){

    //                 int.TryParse(to[1], out int p){
    //                     m_targets.Add(new TargetIPV4(){to[0], p});

    //                 }
    //             }

    //     }


    // }


    // public void BoardcastBytes(byte [] bytes){
    //     UdpClient c = new UdpClient();
    //     foreach ( var t in m_targets){
            

    //     }

    // }
    
}
