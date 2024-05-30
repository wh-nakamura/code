using ExitGames.Client.Photon;
using Photon.Realtime;

using System.Collections.Generic;
using System.Linq;

public static class Plpe // PlayerPropertiesExtensions
{
    public static Hashtable send_li = new Hashtable();

    public static void InitHashtable(this Player player)
    {
        send_li = new Hashtable();
        send_li["moveChara_li"] = null;
        send_li["init2List"] = null;
        send_li["resultReason"] = 0;
        send_li["check_result"] = false;
        send_li["ack"] = false;
        send_li["test"] = 0;
        player.SetCustomProperties(send_li);
        send_li.Clear();
        FuncD.pri("init2List", send_li["init2List"]);
    }


    //$p 送信
    public static void send_bool(this Player player, string key, bool num)
    {
        send_li[key] = num;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }
    
    public static void send_string(this Player player, string key, string num)
    {
        send_li[key] = num;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }
    
    public static void send_int(this Player player, string key, int? num)
    {
        send_li[key] = num;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }

    public static void send_int_d1(this Player player, string key, int[] numList) {

        int[] numArray;
        if (numList != null) {
            numArray = numList.ToArray();
        } else {numArray = null;}

        send_li[key] = numArray;
        //send_li[key] = numList;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }

    public static void send_int_d2(this Player player, string key, List<List<int>> num2List) {

        int[][] num2Array;
        if (num2List != null) {
            num2Array = new int[num2List.Count][];
            for (int i = 0; i < num2List.Count; i++) {
                num2Array[i] = num2List[i].ToArray();
            }
        } else {num2Array = null;}

        send_li[key] = num2Array;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }

    //$p 受信
    public static bool recv_bool(this Player player, string key) {
        if (player.CustomProperties[key] == null) {return false;}
        return (bool)player.CustomProperties[key];
    }

    public static string recv_string(this Player player, string key) {
        if (player.CustomProperties[key] == null) {return "";}
        return (string)player.CustomProperties[key];
    }

    public static int recv_int(this Player player, string key)
    {
        if (player.CustomProperties[key] == null) {return 0;}
        return (int)player.CustomProperties[key];
    }

    public static List<int> recv_int_d1(this Player player, string key)
    {
        int[] numArray = ((int[])player.CustomProperties[key]);

        if (numArray == null) {return null;}

        return numArray.ToList();
    }

    public static List<List<int>> recv_int_d2(this Player player, string key)
    {
        int[][] num2Array = (int[][])player.CustomProperties[key];
        List<List<int>> num2List = new List<List<int>>();

        if (num2Array == null) {return null;}

        for (int i = 0; i < num2Array.Length; i++) {
            num2List.Add(num2Array[i].ToList());
        }

        return num2List;
    }
}