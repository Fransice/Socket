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

//关于网络
using System.Net;

//关于套接字
using System.Net.Sockets;

//关于文本
using System.Text;
using System;

//声明一个委托
public delegate void ldyReceiveCallBack(string content);

public class LdySocket
{
    #region

    //声明客户端的套接字
    Socket clientSocket;
    //声明客户端的委托对象
    ldyReceiveCallBack clientReceiveCallBack;
    //声明客户端的缓存1KB
    byte[] clientBuffer = new byte[1024];
    //1.ip地址 2.端口3.委托对象
    public void InitClient(string ip, int port, ldyReceiveCallBack rcb)
    {
        //接受委托对象
        clientReceiveCallBack = rcb;
        //实例客户端的Socket 参数（IPV4 ，双向读写流，TCP协议）
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //实例化一个客户端的网络端点        IPAddress.Parse (ip)：将IP地址字符串转换为Ip地址实例
        IPEndPoint clientEP = new IPEndPoint(IPAddress.Parse(ip), port);
        //连接服务器
        clientSocket.Connect(clientEP);
        //第一个是缓存  第二个 是从第几个开始接受 第三个 接受多少个字节  第四个 需不需要特殊的服务 第五个回调函数 第六个当前对象
        clientSocket.BeginReceive(clientBuffer, 0, this.clientBuffer.Length, SocketFlags.None,
            new System.AsyncCallback(clientReceive), this.clientSocket);
    }

    void clientReceive(System.IAsyncResult ar)
    {
        //获取一个客户端正在接受数据的对象
        Socket workingSocket = ar.AsyncState as Socket;
        int byteCount = 0;
        string content = "";
        try
        {
            //结束接受数据 完成储存
            byteCount = workingSocket.EndReceive(ar);

        }
        catch (SocketException ex)
        {
            //如果接受消息失败
            clientReceiveCallBack(ex.ToString());
        }
        if (byteCount >= 3)
        {
            byte[] Hand_Json = SplitArray(clientBuffer, 0, 3);
            int connt = int.Parse(UTF8Encoding.UTF8.GetString(Hand_Json));
            if ((byteCount - 3) <= connt)
            {
                Debug.Log(clientBuffer);
                string message = Encoding.UTF8.GetString(clientBuffer, 4, byteCount - 3);
                Debug.Log(message);
                ArrayList al_1 = new ArrayList(clientBuffer);
                al_1.RemoveRange(4, connt - 3);
                clientBuffer = (byte[])al_1.ToArray(typeof(byte));
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


    public void ClientSendMessage(string msg)
    {
        if (msg != "")
        {
            //将要发送的字符串消息转换成BYTE数组
            clientBuffer = UTF8Encoding.UTF8.GetBytes(msg);
        }
        try
        {
            clientSocket.BeginSend(clientBuffer, 0, this.clientBuffer.Length, SocketFlags.None,
            new System.AsyncCallback(SendMsg),
            this.clientSocket);
        }
        catch (System.Exception es)
        {
            Debug.Log("断开连接...." + es);
            Debug.Log("正在重连");
        }

    }

    void SendMsg(System.IAsyncResult ar)
    {
        Socket workingSocket = ar.AsyncState as Socket;
        workingSocket.EndSend(ar);
    }

    #endregion
}
