//
//   █████▒█    ██  ▄████▄   ██ ▄█▀       ██████╗ ██╗   ██╗ ██████╗
// ▓██   ▒ ██  ▓██▒▒██▀ ▀█   ██▄█▒        ██╔══██╗██║   ██║██╔════╝
// ▒████ ░▓██  ▒██░▒▓█    ▄ ▓███▄░        ██████╔╝██║   ██║██║  ███╗
// ░▓█▒  ░▓▓█  ░██░▒▓▓▄ ▄██▒▓██ █▄        ██╔══██╗██║   ██║██║   ██║
// ░▒█░   ▒▒█████▓ ▒ ▓███▀ ░▒██▒ █▄       ██████╔╝╚██████╔╝╚██████╔╝
//  ▒ ░   ░▒▓▒ ▒ ▒ ░ ░▒ ▒  ░▒ ▒▒ ▓▒       ╚═════╝  ╚═════╝  ╚═════╝
//  ░     ░░▒░ ░ ░   ░  ▒   ░ ░▒ ▒░
//  ░ ░    ░░░ ░ ░ ░        ░ ░░ ░
//           ░     ░ ░      ░  ░
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tcp_Client : MonoBehaviour
{
    public Text Text3;
    public Text Text1;
    public Text Text2;
    AsyncTCPClient Tcp_1 = new AsyncTCPClient();
    AsyncTCPClient Tcp_2 = new AsyncTCPClient();
    AsyncTCPClient Tcp_3 = new AsyncTCPClient();
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Tcp_1.AsynConnect("127.0.0.1", 8989);
        // Tcp_1.AsynConnect("127.0.0.1", 8988);
        // Tcp_1.AsynConnect("127.0.0.1", 8987);
    }
    // private void Update()
    // {
    //     Text3.text = Tcp_1.Sendmsg();
    //     Text1.text = Tcp_2.Sendmsg();
    //     Text2.text = Tcp_3.Sendmsg();
    // }
}
