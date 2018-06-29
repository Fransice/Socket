
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
using UnityEngine;
using System.Collections;

public class ClientSocketDemo : MonoBehaviour
{

    private LdySocket ldysocket;
    private string clientContent;
    private string needSendText = "";
    LdySocket ldysocket1 = new LdySocket();
    LdySocket ldysocket2 = new LdySocket();
    LdySocket ldysocket3 = new LdySocket();

    void Awake()
    {
        ldysocket1.InitClient("127.0.0.1", 8080);
        // ldysocket2.InitClient("127.0.0.1", 8988);
        // ldysocket3.InitClient("127.0.0.1", 8987);
    }

    void OnGUI()
    {
        needSendText = GUILayout.TextField(needSendText);
        if (GUILayout.Button("点击发送消息"))
        {
            if (needSendText != "")
            {
                ldysocket1.ClientSendMessage(needSendText);
                // ldysocket2.ClientSendMessage(needSendText);
                // ldysocket3.ClientSendMessage(needSendText);
            }
        }
    }
}