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
    IPEndPoint clientEP;
    //声明客户端的委托对象
    //声明客户端的缓存1KB
    byte[] clientBuffer = new byte[1024];
    //1.ip地址 2.端口3.委托对象
    public void InitClient(string ip, int port)
    {
        //接受委托对象
        //实例客户端的Socket 参数（IPV4 ，双向读写流，TCP协议）
        clientSocket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        //实例化一个客户端的网络端点        IPAddress.Parse (ip)：将IP地址字符串转换为Ip地址实例
        clientEP = new IPEndPoint(IPAddress.Parse(ip), port);
        //连接服务器
        clientSocket.Connect(clientEP);
        Debug.Log("端口号是:" + clientEP.Port);
        Debug.Log("IP是:" + clientEP.Address);
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
        }
        if (byteCount > 0)
        {
            Debug.Log("端口号是:" + clientEP.Port);
            Debug.Log("IP是:" + clientEP.Address);
            // if (byteCount <= 4) return;
            // int count = BitConverter.ToInt32(clientBuffer, 0);
            // if (byteCount - 4 >= count)//判断一下数据是否足够
            // {
            //     string str = Encoding.UTF8.GetString(clientBuffer, 4, count);
            //     Console.WriteLine(str);
            //     // 更新startIndex
            //     Array.Copy(clientBuffer, count + 4, clientBuffer, 0, byteCount - 4 - count);
            //     byteCount -= (count + 4);
            // }
            // else
            // {
            //     // break;
            // }
            try
            {
                if (byteCount >= 4)
                {
                    byte[] Hand_Json = SplitArray(clientBuffer, 0, 4);
                    int connt = int.Parse(UTF8Encoding.UTF8.GetString(Hand_Json));
                    Debug.Log(connt);
                    if ((byteCount - 4) >= connt)
                    {
                        string message = Encoding.UTF8.GetString(clientBuffer, 4, byteCount - 4);
                        QueueExample.instance.InsetTime(message);
                        Debug.Log("message  " + message);
                        ArrayList al_1 = new ArrayList(clientBuffer);
                        al_1.RemoveRange(4, connt - 4);
                        clientBuffer = (byte[])al_1.ToArray(typeof(byte));
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
            }

            //转换已经接受到得Byte数据为字符串
            // content = UTF8Encoding.UTF8.GetString(clientBuffer);
            // Debug.Log(content);
        }
        //发送数据
        //接受下一波数据
        clientSocket.BeginReceive(clientBuffer, 0, this.clientBuffer.Length, SocketFlags.None,
            new System.AsyncCallback(clientReceive), this.clientSocket);
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
    //发送数据
    public void ClientSendMessage(string msg)
    {
        if (msg != "")
        {
            //将要发送的字符串消息转换成BYTE数组
            clientBuffer = UTF8Encoding.UTF8.GetBytes(msg);
        }
        clientSocket.BeginSend(clientBuffer, 0, this.clientBuffer.Length, SocketFlags.None,
            new System.AsyncCallback(SendMsg),
            this.clientSocket);
    }
    void SendMsg(System.IAsyncResult ar)
    {
        Socket workingSocket = ar.AsyncState as Socket;
        workingSocket.EndSend(ar);
    }

    #endregion
}
