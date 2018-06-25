using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void Start()
    {
        string[] arr = { "abc1", "abc2", "abc3", };
        ArrayList al = new ArrayList(arr);
        al.RemoveRange(1, 2);
        arr = (string[])al.ToArray(typeof(string));
        foreach (string s in arr)
        {
            print(s);
        }
    }
}
