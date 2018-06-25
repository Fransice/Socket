using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Threading;


//客户端
public class ClientSocketTest : MonoBehaviour
{

    private static byte[] resultByte = new byte[512];
    private static IPAddress ip;
    private static Socket clientSocket;
    private void Start()
    {
        Main();
    }
    static void Main()
    {
        //设定服务器IP地址
        ip = IPAddress.Parse("127.0.0.1");
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IPEndPoint ipEndpoint = new IPEndPoint(ip, 10000);
            //建立一个一部的建立连接，当连接建立成功调用 CallBack 方法
            IAsyncResult result = clientSocket.BeginConnect(ipEndpoint, new AsyncCallback(CallBack), clientSocket);
            //clientSocket.Connect(new IPEndPoint(ip, 10000));
            //超时检测，当建立超过 5 秒 还没有连接成功表示超时
            bool successConnect = result.AsyncWaitHandle.WaitOne(5000, true);
            if (!successConnect) //判断是否连接成功
            {
                //连接失败，关闭
                Closed(clientSocket);
            }
            else
            {
                //开启一个接收消息的线程
                Thread receiveMessageThread = new Thread(ReceiveMessage);
                //启动线程， clientSocket 为 ReceiveMessage 的参数
                receiveMessageThread.Start(clientSocket);
            }

            Console.WriteLine("连接服务器成功");
        }
        catch
        {
            Console.WriteLine("连接服务器失败，按任意键退出");
            return;
        }

        //通过clientSocket发送消息,循环3次向服务端发送信息
        for (int i = 0; i < 3; i++)
        {
            try
            {
                //等待 1 秒
                Thread.Sleep(1000);
                string sendMessage = "I'm Client ";
                //将发送的字符串转换为数据包                 
                SendMessage(clientSocket, sendMessage);
                //Console.WriteLine("向服务器发送消息 ： {0}" + sendMessage);
            }
            catch
            {
                Closed(clientSocket);
                break;
            }
        }

        //下面是手动输入数字发送给服务端，当输入为 0 时退出循环
        int number = 1;
        while (number != 0)
        {
            string str = Console.ReadLine();
            number = int.Parse(str);
            string sendMessage = "I'm client send : " + number.ToString();
            SendMessage(clientSocket, sendMessage);
        }

        Console.WriteLine("发送完毕");
        Console.ReadLine();
    }

    private static void CallBack(IAsyncResult asyncConnect)
    {
        Console.WriteLine("连接成功");
    }

    //关闭
    private static void Closed(Socket cliSocket)
    {
        // 如果客户端Socket对象不为空，并且为连接状态
        if (cliSocket != null && cliSocket.Connected)
        {
            //关闭Socket
            cliSocket.Shutdown(SocketShutdown.Both);
            cliSocket.Close();
        }
        cliSocket = null;//将客户端对象赋值为空
    }

    //发送消息调用该方法
    private static void SendMessage(Socket cliSocket, string str)
    {
        Socket ClientSocket = cliSocket;
        //将发送的字符串转换为数据包
        byte[] messageByte = Encoding.UTF8.GetBytes(str);
        //判断客户端是否连接
        if (!ClientSocket.Connected)
        {
            Console.WriteLine("客户端为连接，向服务端发送消息失败");
            Closed(ClientSocket);
            return;
        }

        try
        {
            //开启一个异步发送
            IAsyncResult asyncSend = clientSocket.BeginSend(messageByte, 0, messageByte.Length, SocketFlags.None, new AsyncCallback(SendMessageCallBack), ClientSocket);
            //超时检测，超过五秒表示发送失败
            bool sendResult = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
            if (!sendResult)
            {
                Closed(ClientSocket);
            }
        }
        catch
        {
            Console.WriteLine("send error");
        }
    }

    //发送消息的回调方法
    private static void SendMessageCallBack(IAsyncResult asyncConnect)
    {
        Console.WriteLine("客户端已向服务端发送消息");
    }

    //接收消息
    private static void ReceiveMessage(object cliSocket)
    {
        //创建一个Socket对象接收要cliSocket
        Socket ClieSocket = (Socket)cliSocket;
        //在这个线程中接收服务器发送的数据
        while (true)
        {
            if (!ClieSocket.Connected)
            {
                ClieSocket.Close();
                break;
            }
            try
            {
                //通过clientSocket接收数据                 
                //接收数据保存到receiveByte中
                byte[] receiveByte = new byte[216];
                //Receive方法会一直等待服务端回发消息,如果没有接收到会一直在这里等待
                int length = ClieSocket.Receive(receiveByte, receiveByte.Length, 0);
                if (length <= 0)
                {
                    ClieSocket.Close();
                    break;
                }

                if (length > 1)
                {
                    //将接收到的字节转换为字符串
                    string receiveString = Encoding.ASCII.GetString(receiveByte, 0, receiveByte.Length);
                    Console.WriteLine("recive : {0}", receiveString);
                }
            }
            catch { }
        }
    }
}

