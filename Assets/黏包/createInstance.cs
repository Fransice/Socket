﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class createInstance : MonoBehaviour
{
    //SocketTCPClient.cs
    private static string ip = "127.0.0.1";
    private static int port = 8999;
    private static Socket socketClient;
    public static List<string> listMessage = new List<string>();
    ///<summary>
    ///创建一个SocketClient实例
    ///</summary>
    ///ip地址 端口 类型默认为TCP
    public static void CreateInstance(string ipStr, int iPort)
    {
        ip = ipStr;
        port = iPort;
        socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ConnectServer();
    }
    private void Start()
    {
        // init(ip, port);
        CreateInstance(ip, port);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string[] str = { "测试字符串1", "test1", "test11" };
            socketClient.Send(BuildDataPackage(1, 2, 3, 4, 5, str));
            string[] str2 = { "我是与1同时发送的测试字符串2，请注意我是否与其他信息粘包", "test2", "test22" };
            socketClient.Send(BuildDataPackage(1, 6, 7, 8, 9, str2));
        }
    }
    void OnApplicationQuit()
    {
        socketClient.Close();
    }
    /// <summary>s
    ///连接服务器
    /// </summary>
    private static void ConnectServer()
    {
        try
        {
            socketClient.Connect(IPAddress.Parse(ip), port);
            Thread threadConnect = new Thread(new ThreadStart(ReceiveMessage));
            threadConnect.Start();
            print("链接成功...");
        }
        catch (ArgumentNullException e)
        {
            Debug.Log(e.ToString());
        }
        catch (SocketException e)
        {
            Debug.Log(e.ToString());
        }
    }
    ///<summary>
    ///接收消息
    ///</summary>
    private static void ReceiveMessage()
    {
        while (true)
        {
            //接受消息头（消息校验码4字节 + 消息长度4字节 + 身份ID8字节 + 主命令4字节 + 子命令4字节 + 加密方式4字节 = 28字节）
            int HeadLength = 28;
            //存储消息头的所有字节数
            byte[] recvBytesHead = new byte[HeadLength];
            //如果当前需要接收的字节数大于0，则循环接收
            while (HeadLength > 0)
            {
                print("接收到消息");
                byte[] recvBytes1 = new byte[28];
                //将本次传输已经接收到的字节数置0
                int iBytesHead = 0;
                //如果当前需要接收的字节数大于缓存区大小，则按缓存区大小进行接收，相反则按剩余需要接收的字节数进行接收
                if (HeadLength >= recvBytes1.Length)
                {
                    iBytesHead = socketClient.Receive(recvBytes1, recvBytes1.Length, 0);
                }
                else
                {
                    iBytesHead = socketClient.Receive(recvBytes1, HeadLength, 0);
                }
                //将接收到的字节数保存
                print("seveing...");
                recvBytes1.CopyTo(recvBytesHead, recvBytesHead.Length - HeadLength);
                //减去已经接收到的字节数
                HeadLength -= iBytesHead;
            }
            //接收消息体（消息体的长度存储在消息头的4至8索引位置的字节里）
            byte[] bytes = new byte[4];
            Array.Copy(recvBytesHead, 4, bytes, 0, 4);
            int BodyLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0));
            //存储消息体的所有字节数
            byte[] recvBytesBody = new byte[BodyLength];
            //如果当前需要接收的字节数大于0，则循环接收
            while (BodyLength > 0)
            {
                byte[] recvBytes2 = new byte[BodyLength < 1024 ? BodyLength : 1024];
                //将本次传输已经接收到的字节数置0
                int iBytesBody = 0;
                //如果当前需要接收的字节数大于缓存区大小，则按缓存区大小进行接收，相反则按剩余需要接收的字节数进行接收
                if (BodyLength >= recvBytes2.Length)
                {
                    iBytesBody = socketClient.Receive(recvBytes2, recvBytes2.Length, 0);
                }
                else
                {
                    iBytesBody = socketClient.Receive(recvBytes2, BodyLength, 0);
                }
                //将接收到的字节数保存
                recvBytes2.CopyTo(recvBytesBody, recvBytesBody.Length - BodyLength);
                //减去已经接收到的字节数
                BodyLength -= iBytesBody;
            }
            //一个消息包接收完毕，解析消息包
            UnpackData(recvBytesHead, recvBytesBody);
            print("接受完成..正在解析....");
        }
    }
    /// <summary>
    /// 解析消息包
    /// </summary>
    /// <param name="Head">消息头</param>
    /// <param name="Body">消息体</param>
    public static void UnpackData(byte[] Head, byte[] Body)
    {
        byte[] bytes = new byte[4];
        Array.Copy(Head, 0, bytes, 0, 4);
        Debug.Log("接收到数据包中的校验码为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

        bytes = new byte[8];
        Array.Copy(Head, 8, bytes, 0, 8);
        Debug.Log("接收到数据包中的身份ID为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt64(bytes, 0)));

        bytes = new byte[4];
        Array.Copy(Head, 16, bytes, 0, 4);
        Debug.Log("接收到数据包中的数据主命令为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

        bytes = new byte[4];
        Array.Copy(Head, 20, bytes, 0, 4);
        Debug.Log("接收到数据包中的数据子命令为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

        bytes = new byte[4];
        Array.Copy(Head, 24, bytes, 0, 4);
        Debug.Log("接收到数据包中的数据加密方式为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

        bytes = new byte[Body.Length];
        for (int i = 0; i < Body.Length;)
        {
            byte[] _byte = new byte[4];
            Array.Copy(Body, i, _byte, 0, 4);
            i += 4;
            int num = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(_byte, 0));

            _byte = new byte[num];
            Array.Copy(Body, i, _byte, 0, num);
            i += num;
            Debug.Log("接收到数据包中的数据有：" + Encoding.UTF8.GetString(_byte, 0, _byte.Length));
        }
    }
    /// <summary>
    /// 构建消息数据包
    /// </summary>
    /// <param name="Crccode">消息校验码，判断消息开始</param>
    /// <param name="sessionid">用户登录成功之后获得的身份ID</param>
    /// <param name="command">主命令</param>
    /// <param name="subcommand">子命令</param>
    /// <param name="encrypt">加密方式</param>
    /// <param name="MessageBody">消息内容（string数组）</param>
    /// <returns>返回构建完整的数据包</returns>
    public static byte[] BuildDataPackage(int Crccode, long sessionid, int command, int subcommand, int encrypt, string[] MessageBody)
    {
        //消息校验码默认值为0x99FF
        Crccode = 65433;
        //消息头各个分类数据转换为字节数组（非字符型数据需先转换为网络序  HostToNetworkOrder:主机序转网络序）
        byte[] CrccodeByte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Crccode));
        byte[] sessionidByte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(sessionid));
        byte[] commandByte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(command));
        byte[] subcommandByte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(subcommand));
        byte[] encryptByte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(encrypt));
        //计算消息体的长度
        int MessageBodyLength = 0;
        for (int i = 0; i < MessageBody.Length; i++)
        {
            if (MessageBody[i] == "")
                break;
            MessageBodyLength += Encoding.UTF8.GetBytes(MessageBody[i]).Length;
        }
        //定义消息体的字节数组（消息体长度MessageBodyLength + 每个消息前面有一个int变量记录该消息字节长度）
        byte[] MessageBodyByte = new byte[MessageBodyLength + MessageBody.Length * 4];
        //记录已经存入消息体数组的字节数，用于下一个消息存入时检索位置
        int CopyIndex = 0;
        for (int i = 0; i < MessageBody.Length; i++)
        {
            //单个消息
            byte[] bytes = Encoding.UTF8.GetBytes(MessageBody[i]);
            //先存入单个消息的长度
            BitConverter.GetBytes(IPAddress.HostToNetworkOrder(bytes.Length)).CopyTo(MessageBodyByte, CopyIndex);
            CopyIndex += 4;
            bytes.CopyTo(MessageBodyByte, CopyIndex);
            CopyIndex += bytes.Length;
        }
        //定义总数据包（消息校验码4字节 + 消息长度4字节 + 身份ID8字节 + 主命令4字节 + 子命令4字节 + 加密方式4字节 + 消息体）
        byte[] totalByte = new byte[28 + MessageBodyByte.Length];
        //组合数据包头部（消息校验码4字节 + 消息长度4字节 + 身份ID8字节 + 主命令4字节 + 子命令4字节 + 加密方式4字节）
        CrccodeByte.CopyTo(totalByte, 0);
        BitConverter.GetBytes(IPAddress.HostToNetworkOrder(MessageBodyByte.Length)).CopyTo(totalByte, 4);
        sessionidByte.CopyTo(totalByte, 8);
        commandByte.CopyTo(totalByte, 16);
        subcommandByte.CopyTo(totalByte, 20);
        encryptByte.CopyTo(totalByte, 24);
        //组合数据包体
        MessageBodyByte.CopyTo(totalByte, 28);
        Debug.Log("发送数据包的总长度为：" + totalByte.Length);
        return totalByte;
    }
    ///<summary>
    ///发送信息
    ///</summary>
    public static void SendMessage(byte[] sendBytes)
    {
        //确定是否连接
        if (socketClient.Connected)
        {
            //获取远程终结点的IP和端口信息
            IPEndPoint ipe = (IPEndPoint)socketClient.RemoteEndPoint;
            socketClient.Send(sendBytes, sendBytes.Length, 0);
        }
    }
    //SocketTCPServer.cs
    private static string ip1 = "127.0.0.1";
    private static int port1 = 5690;
    private static Socket socketServer;
    public static List<Socket> listPlayer = new List<Socket>();
    private static Socket sTemp;
    ///<summary>
    ///绑定地址并监听
    ///</summary>
    ///ip地址 端口 类型默认为TCP
    public static void init(string ipStr, int iPort)
    {
        try
        {
            ip1 = ipStr;
            port1 = iPort;
            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketServer.Bind(new IPEndPoint(IPAddress.Parse(ip1), port1));
            Thread threadListenAccept = new Thread(new ThreadStart(ListenAccept));
            threadListenAccept.Start();
        }
        catch (ArgumentNullException e)
        {
            Debug.Log(e.ToString());
        }
        catch (SocketException e)
        {
            Debug.Log(e.ToString());
        }
    }
    ///<summary>
    ///监听用户连接
    ///</summary>
    private static void ListenAccept()
    {
        socketServer.Listen(0);                       //对于socketServer绑定的IP和端口开启监听
        sTemp = socketServer.Accept();                //如果在socketServer上有新的socket连接，则将其存入sTemp，并添加到链表
        listPlayer.Add(sTemp);
        Thread threadReceiveMessage = new Thread(new ThreadStart(ReceiveMessage));
        threadReceiveMessage.Start();
        while (true)
        {
            sTemp = socketServer.Accept();
            listPlayer.Add(sTemp);
        }
    }
}
