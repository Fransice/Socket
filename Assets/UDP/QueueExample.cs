using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

public class QueueExample : MonoBehaviour
{
    public static QueueExample instance;
    public Queue queue;
    public Text queueCount;
    public Text queueContent;
    public Text queuePeek;
    List<byte> clientDate;
    byte[] byt;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        queue = new Queue();
        byt = new byte[1024];
    }

    void Update()
    {
        byte[] con = new byte[1024];
        if (queue.Count > 0)
        {
            var obj = queue.Dequeue();
            print(obj);
            // print(Encoding.UTF8.GetString(a);
            // print(queue.ToArray[0]);
        }
        else
        {
            queuePeek.text = "queuePeek : ";
        }

        //增加元素
        if (Input.GetKeyDown(KeyCode.A))
        {
            // InsetTime();
            showQueue();
        }

        //减去元素
        if (Input.GetKey(KeyCode.D))
        {
            if (queue.Count > 0)
            {
                queue.Dequeue();
                // string str = queue.Dequeue();
                print(queue.Dequeue());
            }
        }

        //清除队列所有元素
        if (Input.GetKeyDown(KeyCode.C))
        {
            queue.Clear();
            showQueue();
        }

        queueCount.text = "queueCount : " + queue.Count.ToString();
    }
    //添加
    public void InsetTime(string conne)
    {
        queue.Enqueue(conne);
        // print(conne);
        // clientDate.Add(conne);
        // print(Encoding.UTF8.GetString(clientDate.ToArray(), 0, 3));
    }
    //减少
    void showQueue()
    {
        queueContent.text = "queueContent : ";
        foreach (float value in queue)
        {
            queueContent.text += value.ToString() + " | ";
        }
    }
}