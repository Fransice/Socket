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
    public string ip_ip;
    public int port;
    void Awake()
    {
        ldysocket = new LdySocket();
        ldysocket.InitClient(ip_ip, port, (string msg) =>
        {
            clientContent = msg;
            print(clientContent);
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
        GUI.color = Color.red;
        GUI.Label(new Rect(10, 10, 100, 20), "ping: " + delayTime.ToString() + "ms");

        if (null != ping && ping.isDone)
        {
            delayTime = ping.time;
            ping.DestroyPing();
            ping = null;
            Invoke("SendPing", 1.0F);//每秒Ping一次
        }
    }
    public string IP = "192.168.1.100";
    Ping ping;
    float delayTime;

    void Start()
    {
        SendPing();
    }



    void SendPing()
    {
        ping = new Ping(IP);
    }
}