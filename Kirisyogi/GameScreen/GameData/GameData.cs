using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameData : MonoBehaviour
{
    public static string userName = "1-20";

    //public static int[][] init2List = new int[][] // {charaId, x, y}
    //{
    //    new int[3], // 十
    //    new int[3], // 王
    //    new int[3], // 斜
    //    new int[3], // 歩
    //    new int[3],
    //    new int[3],
    //};

    public static List<List<int>> init2List = new List<List<int>>();

    public static string roomID = null;
}
