using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client_Start : MonoBehaviour
{
    Client Client = new Client();
    Client1 Client1 = new Client1();
    Client2 Client2 = new Client2();
    void Update()
    {
        Client.InitSocket("192.168.199.145", 8989);
        Client1.InitSocket("192.168.199.145", 8988);
        Client2.InitSocket("192.168.199.145", 8987);
    }
}
