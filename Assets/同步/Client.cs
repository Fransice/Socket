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
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System;
using System.Text;

public class Client : MonoBehaviour
{
    string ip = "127.0.0.1";
    byte[] send;
    int port = 8002;
    //定义线程  处理接受的数据
    Thread threadClient = null;
    Thread New_Send = null;
    //创建客户端套接字，负责连接服务器
    Socket socketClient = null;
    bool IS_Client;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Connent();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (IS_Client)
        {
            StartCoroutine(Send());
        }
    }
    public void Connent()
    {
        //创建网络节点
        IPAddress address = IPAddress.Parse(ip);
        //网络节点对象
        IPEndPoint endpore = new IPEndPoint(address, port);
        //链接服务器
        socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            //开启链接
            socketClient.Connect(endpore);
            IS_Client = true;
            print("开启心跳。。。");
        }
        catch (Exception ex)
        {
            print("客户端连接服务器发生异常：" + ex);
            print("正在尝试重连。。。");
            Connent();
        }
    }
    IEnumerator Send()
    {
        yield return new WaitForSeconds(0.2f);
        try
        {
            send = new byte[1024];
            string i = "1";
            send = Encoding.ASCII.GetBytes(i);
            //发送  
            socketClient.Send(send, send.Length, SocketFlags.None);
            print("连接正常");
        }
        catch (Exception es)
        {
            print("连接出现问题：" + es);
            print("正在重连...");
            Connent();
        }

    }

}
