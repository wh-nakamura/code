using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;               // ファイル読み込みに必要です
using Newtonsoft.Json;         // Json.net用に必要です

[Serializable]
public class SaveData
{
    public int org_len = 4;
    public List<int> org_li_1 = new List<int>() {2, 1, 1};
    public List<int> org_li_2 = new List<int>() {4, 2, 1};
    public List<int> org_li_3 = new List<int>() {3, 3, 1};
    public List<int> org_li_4 = new List<int>() {1, 2, 2};
    public List<int> org_li_5 = new List<int>() {1, 1, 1};
    public List<int> org_li_6 = new List<int>() {1, 1, 1};
    public List<int> org_li_7 = new List<int>() {1, 1, 1};
    public List<int> org_li_8 = new List<int>() {1, 1, 1};

    public int score = 1500;
    public int stone = 0;

    public string id = "";
    public string pwd = "";
}

public class SaveManager : MonoBehaviour
{

    static string filePath;
    static SaveData saveData = new SaveData();
    public static List<List<int>> org_li_li = new List<List<int>>() {saveData.org_li_1, saveData.org_li_2, saveData.org_li_3, saveData.org_li_4, saveData.org_li_5, saveData.org_li_6, saveData.org_li_7, saveData.org_li_8};
    
    public static bool IsSaveData()
    {
        filePath = Application.dataPath;

        return (File.Exists(filePath + "/Savedata.json") ||
        File.Exists(filePath + "/Script/Kirisyogi/Savedata.json")) ?
        true : false;
    }

    public static bool Save()
    {
        saveData.org_len = GameData.init2List.Count;
        for (int i = 0; i < saveData.org_len; i++) {
            switch (i) {
                case 0: saveData.org_li_1 = GameData.init2List[i]; break;
                case 1: saveData.org_li_2 = GameData.init2List[i]; break;
                case 2: saveData.org_li_3 = GameData.init2List[i]; break;
                case 3: saveData.org_li_4 = GameData.init2List[i]; break;
                case 4: saveData.org_li_5 = GameData.init2List[i]; break;
                case 5: saveData.org_li_6 = GameData.init2List[i]; break;
                case 6: saveData.org_li_7 = GameData.init2List[i]; break;
                case 7: saveData.org_li_8 = GameData.init2List[i]; break;
                default: print($"エラー{i}"); break;
            }
            ////for (int j = 0; j < GameData.init2List[i].Length; j++) {
            ////    org_li_li[i][j] = GameData.init2List[i][j];
            ////}
            
        }

        string json = JsonUtility.ToJson(saveData);
        StreamWriter streamWriter;
        
        try { // ワークスペースにjsonファイルを置くための処理
            filePath = Application.dataPath + "/Script/Kirisyogi/Savedata.json"; // /Assets
            streamWriter = new StreamWriter(filePath);
        } catch (System.IO.DirectoryNotFoundException) {
            filePath = Application.dataPath + "/Savedata.json";
            streamWriter = new StreamWriter(filePath);
        }
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();

        //print("save");

        return true;
    }

    public static void SaveLogin(string id, string pwd)
    {
        saveData.id = id;
        saveData.pwd = pwd;

        string json = JsonUtility.ToJson(saveData);
        StreamWriter streamWriter;
        
        try { // ワークスペースにjsonファイルを置くための処理
            filePath = Application.dataPath + "/Script/Kirisyogi/Savedata.json"; // /Assets
            streamWriter = new StreamWriter(filePath);
        } catch (System.IO.DirectoryNotFoundException) {
            filePath = Application.dataPath + "/Savedata.json";
            streamWriter = new StreamWriter(filePath);
        }
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();

        print("save");
    }

    public static (string id, string pwd) LoadLogin()
    {
        filePath = Application.dataPath;

        if (File.Exists(filePath + "/Savedata.json") ||
        File.Exists(filePath + "/Script/Kirisyogi/Savedata.json"))
        {
            StreamReader streamReader;

            try { // ワークスペースにjsonファイルを置くための処理
                filePath = Application.dataPath + "/Script/Kirisyogi/Savedata.json"; // /Assets
                streamReader = new StreamReader(filePath);
                print("UnityEditのみ");
            } catch (System.IO.DirectoryNotFoundException) {
                filePath = Application.dataPath + "/Savedata.json";
                streamReader = new StreamReader(filePath);
            }
            print(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            SaveData loadData = JsonUtility.FromJson<SaveData>(data);
            if (loadData == null) {return ("", "");}

            return (loadData.id, loadData.pwd);

        } else {
            FuncD.pri("存在しないパス", Application.dataPath);
            return ("", "");
        }
    }
    
    public static void Load()
    {
        filePath = Application.dataPath;

        if (File.Exists(filePath + "/Savedata.json") ||
        File.Exists(filePath + "/Script/Kirisyogi/Savedata.json"))
        {
            StreamReader streamReader;

            try { // ワークスペースにjsonファイルを置くための処理
                filePath = Application.dataPath + "/Script/Kirisyogi/Savedata.json"; // /Assets
                streamReader = new StreamReader(filePath);
                print("UnityEditのみ");
            } catch (System.IO.DirectoryNotFoundException) {
                filePath = Application.dataPath + "/Savedata.json";
                streamReader = new StreamReader(filePath);
            }
            print(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            SaveData loadData = JsonUtility.FromJson<SaveData>(data);
            if (loadData == null) {return;}
            LoadData(loadData);

            print("load");
        } else {
            FuncD.pri("存在しないパス", Application.dataPath);
        }
    }

    // jsonファイルからのロード
    public static void LoadData(SaveData loadData) {
        saveData.org_len = loadData.org_len;
        saveData.org_li_1 = loadData.org_li_1;
        saveData.org_li_2 = loadData.org_li_2;
        saveData.org_li_3 = loadData.org_li_3;
        saveData.org_li_4 = loadData.org_li_4;
        saveData.org_li_5 = loadData.org_li_5;
        saveData.org_li_6 = loadData.org_li_6;
        saveData.org_li_7 = loadData.org_li_7;
        saveData.org_li_8 = loadData.org_li_8;
        saveData.score = loadData.score;
        saveData.stone = loadData.stone;
    }

    // 編成データのロード
    public static void LoadOrg() {
        List<List<int>> org_li_li = new List<List<int>>() {saveData.org_li_1, saveData.org_li_2, saveData.org_li_3, saveData.org_li_4, saveData.org_li_5, saveData.org_li_6, saveData.org_li_7, saveData.org_li_8};
        GameData.init2List = new List<List<int>>() {};
        for (int i = 0; i < saveData.org_len; i++) {
            GameData.init2List.Add(org_li_li[i]);
            //for (int j = 0; j < org_li_li[i].Length; j++) {
            //    GameData.init2List[i][j] = org_li_li[i][j];
            //}
        }
        ////print(GameData.init2List.Count);
        ////print(GameData.init2List[0][0]);
        ////print(GameData.init2List[1][0]);
        ////print(GameData.init2List[2][0]);
        ////print(GameData.init2List[3][0]);
    }
}
