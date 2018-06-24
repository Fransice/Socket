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

    void Awake()
    {
        ldysocket = new LdySocket();
        ldysocket.InitClient("127.0.0.1", 8989, (string msg) =>
        {
            clientContent = msg;
        });
    }

    void OnGUI()
    {
        needSendText = GUILayout.TextField(needSendText);
        if (GUILayout.Button("点击发送消息"))
        {
            if (needSendText != "")
            {
                ldysocket.ClientSendMessage(needSendText);
            }
        }
    }
}