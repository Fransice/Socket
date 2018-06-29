// using UnityEngine;
// using System.Collections;
// using UnityEngine.UI;

// public class QueueExample : MonoBehaviour
// {
//     public Queue queue;
//     public Text queueCount;
//     public Text queueContent;
//     public Text queuePeek;

//     void Start()
//     {
//         queue = new Queue();
//     }

//     void Update()
//     {
//         if (queue.Count > 0)
//         {
//             queuePeek.text = "queuePeek : " + queue.Peek().ToString();
//         }
//         else
//         {
//             queuePeek.text = "queuePeek : ";
//         }

//         //增加元素
//         if (Input.GetKeyDown(KeyCode.A))
//         {
//             InsetTime();
//             showQueue();
//         }

//         //减去元素
//         if (Input.GetKeyDown(KeyCode.D))
//         {
//             if (queue.Count > 0)
//             {
//                 Debug.Log(queue.Dequeue());
//                 showQueue();
//             }
//         }

//         //清除队列所有元素
//         if (Input.GetKeyDown(KeyCode.C))
//         {
//             queue.Clear();
//             showQueue();
//         }

//         queueCount.text = "queueCount : " + queue.Count.ToString();
//     }

//     void InsetTime()
//     {
//         queue.Enqueue(Time.time);
//     }

//     void showQueue()
//     {
//         queueContent.text = "queueContent : ";
//         foreach (float value in queue)
//         {
//             queueContent.text += value.ToString() + " | ";
//         }
//     }
// }
