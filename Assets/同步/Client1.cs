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
//引入库
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Linq;
public class Client1
{

    Socket serverSocket; //服务器端socket
    IPAddress ip; //主机ip
    IPEndPoint ipEnd;
    string recvStr; //接收的字符串
    string sendStr; //发送的字符串
    byte[] recvData = new byte[1024]; //接收的数据，必须为字节
    byte[] sendData = new byte[1024]; //发送的数据，必须为字节
    int recvLen; //接收的数据长度
    Thread connectThread; //连接线程
                          // Json_Manager Json_Manager = new Json_Manager();
                          //初始化
    public void InitSocket(string ip_Ip, int port)
    {
        //定义服务器的IP和端口，端口与服务器对应
        ip = IPAddress.Parse(ip_Ip); //可以是局域网或互联网ip，此处是本机
        ipEnd = new IPEndPoint(ip, port);


        //开启一个线程连接，必须的，否则主线程卡死
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketConnet()
    {
        if (serverSocket != null)
            serverSocket.Close();
        //定义套接字类型,必须在子线程中定义
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Debug.Log("ready to connect");
        //连接
        serverSocket.Connect(ipEnd);
        SocketSend("sadads");
        //输出初次连接收到的字符串
        recvLen = serverSocket.Receive(recvData);
        recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
    }

    void SocketSend(string sendStr)
    {
        //清空发送缓存
        sendData = new byte[1024];
        //数据类型转换
        sendData = Encoding.ASCII.GetBytes(sendStr);
        //发送
        serverSocket.Send(sendData, sendData.Length, SocketFlags.None);
    }

    int connt;
    void SocketReceive()
    {
        SocketConnet();
        //不断接收服务器发来的数据
        while (true)
        {
            recvData = new byte[2048];
            recvLen = serverSocket.Receive(recvData);
            if (recvLen == 0)
            {
                SocketConnet();
                continue;
            }
            if (recvLen >= 3)
            {
                byte[] Hand_Json = SplitArray(recvData, 0, 3);
                int connt = int.Parse(UTF8Encoding.UTF8.GetString(Hand_Json));
                Debug.Log(connt);
                if ((recvLen - 3) >= connt)
                {
                    Debug.Log(recvLen);
                    string message = Encoding.UTF8.GetString(recvData, 4, recvLen - 3);
                    Debug.Log("message  " + message);
                    ArrayList al_1 = new ArrayList(recvData);
                    al_1.RemoveRange(4, connt - 3);
                    recvData = (byte[])al_1.ToArray(typeof(byte));
                }
            }
            else
            {
                Debug.Log(UTF8Encoding.UTF8.GetString(recvData));
            }
        }
    }
    //数组拆分
    public byte[] SplitArray(byte[] Source, int StartIndex, int EndIndex)
    {
        try
        {
            byte[] result = new byte[EndIndex - StartIndex + 1];
            for (int i = 0; i <= EndIndex - StartIndex; i++) result[i] = Source[i + StartIndex];
            return result;
        }
        catch (IndexOutOfRangeException ex)
        {
            throw new Exception(ex.Message);
        }
    }
    void SocketQuit()
    {
        //关闭线程
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最后关闭服务器
        if (serverSocket != null)
            serverSocket.Close();
        Debug.Log("diconnect");
    }
    //程序退出则关闭连接
    void OnApplicationQuit()
    {
        SocketQuit();
    }
}
