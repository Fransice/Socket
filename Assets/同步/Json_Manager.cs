// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.IO;
// using LitJson;
// using System.Runtime.Serialization.Formatters.Binary;

// public class Json_Manager
// {
//     //六轴机械臂通信
//     Mechanicalarm_Json mechanicalarm_Json = new Mechanicalarm_Json();
//     //RFID 通信
//     RFID rfid = new RFID();
//     //U3D交互 =====>>>>>> 接口保护板
//     U3DToInterfaceprotection U3DToInterfaceprotection = new U3DToInterfaceprotection();
//     //接口保护板 =====>>>>>> U3D交互
//     InterfaceprotectionToU3D InterfaceprotectionToU3D = new InterfaceprotectionToU3D();
//     //六轴机器人
//     public void Robot_json(string data)
//     {
//         try
//         {
//             JsonData jsonData = JsonMapper.ToObject(data);
//             mechanicalarm_Json.coordAxis = jsonData["coordAxis"].ToString();
//             for (int i = 0; i < jsonData["coordAxis"].Count; i++)
//             {
//                 Debug.Log(jsonData["coordAxis"][i]);
//             }
//         }
//         catch (System.Exception es)
//         {
//             Debug.Log("数据不匹配..." + es);
//         }

//     }
//     //接口保护板
//     public void Interfaceboard_Json(string data)
//     {
//         JsonData jsonDataA = JsonMapper.ToObject(data);

//     }
// }