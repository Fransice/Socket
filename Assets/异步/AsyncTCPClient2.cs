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
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Threading;
public class AsyncTCPClient2
{
    private string New_ip;
    private int New_port;
    //创建套接字
    Socket client;
    Thread socket_Measage;
    string message;
    int num = 0;
    /// <summary>
    /// 连接到服务器
    /// </summary>
    public void AsynConnect(string Ip, int port)
    {
        New_ip = Ip;
        New_port = port;
        //端口及IP
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Ip), port);
        //创建套接字
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //开始连接到服务器
        client.BeginConnect(ip, asyncResult =>
        {
            client.EndConnect(asyncResult);
            socket_Measage = new Thread(new ThreadStart(AsynRecive));
            socket_Measage.Start();
            //接受消息
        }, null);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="message"></param>
    public void AsynSend(Socket socket, string message)
    {
        if (socket == null || message == string.Empty) return;
        //编码
        byte[] data = Encoding.UTF8.GetBytes(message);
        try
        {
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
            {
                //完成发送消息
                int length = socket.EndSend(asyncResult);
                Debug.Log(string.Format("客户端发送消息:{0}", message));
            }, null);
        }
        catch (Exception ex)
        {
            Debug.Log("异常信息：{0}" + ex);
        }
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="socket"></param>
    public void AsynRecive()
    {
        while (true)
        {

            byte[] data = new byte[1024];
            try
            {
                //开始接收数据
                //************************************************************************************************************ */
                client.BeginReceive(data, 0, data.Length, SocketFlags.None,
                asyncResult =>
                {
                    int length = client.EndReceive(asyncResult);
                    //消息处理
                    if (length == 0)
                    {
                        AsynConnect(New_ip, New_port);
                        return;
                    }
                    if (length >= 3)
                    {
                        byte[] Hand_Json = SplitArray(data, 0, 3);
                        int connt = int.Parse(UTF8Encoding.UTF8.GetString(Hand_Json));
                        Debug.Log(connt);
                        if ((length - 3) >= connt)
                        {
                            message = Encoding.UTF8.GetString(data, 4, length - 3);
                            Debug.Log("message  " + message);
                            num++;
                            ArrayList al_1 = new ArrayList(data);
                            al_1.RemoveRange(4, connt - 3);
                            data = (byte[])al_1.ToArray(typeof(byte));
                        }
                    }
                    else
                    {
                        return;
                    }
                    //************************************************************************************************************ */
                    // Debug.Log(string.Format("收到服务器消息:{0}", Encoding.UTF8.GetString(data)));
                }, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：", ex.Message);
            }

        }
    }
    public string Sendmsg()
    {
        return num.ToString();
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
    private void OnApplicationQuit()
    {
        if (client != null)
        {
            client.Close();
        }
    }
}
