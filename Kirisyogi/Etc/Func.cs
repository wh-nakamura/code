using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using TMPro;
using UnityEngine.UI;

using System.Security.Cryptography;
using System.Text;

using System.IO;

using Cysharp.Threading.Tasks;
using System;


public class Func : MonoBehaviour
{
    public static GameObject test_s;


    public static bool UniWait(OrgMain classObject, bool waitTask)
    {
        UniWaitTask(classObject.GetCancellationTokenOnDestroy(), waitTask);
        return true;
    }
    
    async public static void UniWaitTask(CancellationToken token, bool waitTask)
    {
        await UniTask.WaitUntil(() => (
            waitTask), cancellationToken: token);
    }

    public static double Floor2(double num)
    {
        num = Math.Floor(num*10);
        return num * 0.1;
    }

    public static float Round(float num)
    {
        if (num%0.5 != 0)
        {
            if (num%0.5 <= 0.5)
            {
                num -= num%0.5f;
            }
            else
            {
                num -= num%0.5f;
                num += 1;
            }
        }
        return num;
    }

    public static void sleep(int num)
    {
        print($"{num}ミリ秒");
        Thread.Sleep(num);
    }

    public static void print_t(object x) {
        print(x);
    }

    public static void Destroy_t(GameObject obj) {
        Destroy(obj);
    }

    public static GameObject InstantObj(string objName, float layer) {
        return Instantiate((GameObject)Resources.Load(objName), 
            new Vector3(0, 0, layer), Quaternion.identity) as GameObject;
    }

    public static (int x, int y) returnPos(GameObject obj) {
        return ((int)obj.transform.position.x, (int)obj.transform.position.y);
    }

    public static void ScrollObject(GameObject scrollbar) {

        float wheel = Input.GetAxis("Mouse ScrollWheel");
        float a = 1;

        //$b スクロールしたなら
        if (wheel != 0) {
            ////if (AddPieceListScript.This.totalCharaNumber2 != 0) {a = AddPieceListScript.This.totalCharaNumber2;}
            scrollbar.GetComponent<Scrollbar>().value += wheel*0.45f*a;
        }
    }

    public static GameObject Cre_text(float x, float y, string t)
    {
        GameObject text_canvas = Instantiate(
            (GameObject)Resources.Load("Canvas"), 
            new Vector3(x, y, Layer.UI), 
            Quaternion.identity) as GameObject;
        GameObject text_t = text_canvas.transform.GetChild(0).gameObject;
        text_t.GetComponent<UnityEngine.UI.Text>().text = t;
        return text_canvas;
    }

    public static void EditText(GameObject textObj, string text_t) {
        textObj.GetComponent<TextMeshProUGUI>().text = text_t;
    }

    public static void print_r(object disp_li) {
        Type type = disp_li.GetType();

        if (type == typeof(int[])) {
            foreach (int item in (int[])disp_li) {
                print(item);
            }
        }
        else if (type == typeof(List<int>)) {
            foreach (int item in (List<int>)disp_li) {
                print(item);
            }
        }
        else if (type == typeof(Photon.Realtime.Player[])) {
            foreach (Photon.Realtime.Player item in (Photon.Realtime.Player[])disp_li) {
                print(item);
            }
        }
        else if (type == typeof(List<CharaScript>)) {
            foreach (CharaScript item in (List<CharaScript>)disp_li) {
                print(item);
            }
        }
    }

    public static void hash()
    {
        int[] str = new int[] {1, 2};

        byte[] beforeByteArray = Encoding.UTF8.GetBytes($"{str}");

        // SHA1 ハッシュ値を計算する
        SHA1 sha1 = SHA1.Create();
        byte[] afterByteArray1 = sha1.ComputeHash(beforeByteArray);
        sha1.Clear();

        // バイト配列を16進数文字列に変換
        StringBuilder sb1 = new StringBuilder();
        foreach (byte b in afterByteArray1)
        {
            sb1.Append(b.ToString("x2"));
        }
        print(sb1);
    }

    public static void angou()
    {
        //RSACryptoServiceProviderオブジェクトの作成
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);

        //公開鍵をXML形式で取得
        String publicKey = rsa.ToXmlString(false);
        //秘密鍵をXML形式で取得
        String privateKey = rsa.ToXmlString(true);

        byte[] bytesPublicKey = Encoding.UTF8.GetBytes(publicKey);
        byte[] bytesPrivateKey = Encoding.UTF8.GetBytes(privateKey);

        //公開鍵を保存
        FileStream outfs = new FileStream(Application.dataPath + "/Script/Kirisyogi/b.txt", FileMode.Create, FileAccess.Write);
        outfs.Write(bytesPublicKey, 0, bytesPublicKey.Length);
        outfs.Close();

        //秘密鍵を保存
        FileStream outfs1 = new FileStream(Application.dataPath + "/Script/Kirisyogi/b.txt", FileMode.Create, FileAccess.Write);
        outfs1.Write(bytesPrivateKey, 0, bytesPrivateKey.Length);
        outfs1.Close();

        rsa.Clear();
    }


    ////public static void print_r(int[][] disp_li)
    ////{
    ////    string disp_str = "[";
    ////    bool pas = true;
    ////    int len = disp_li.Length;
    ////    int i = 1;
////
    ////    foreach (var item in disp_li)
    ////    {
    ////        //// if (item.GetType().IsArray) // 型宣言によって次元ごとに分けられないため
    ////        disp_str += "[";
    ////        foreach (var item2 in item)
    ////        {
    ////            disp_str += (pas) ? $"{item2}" : $", {item2}";
    ////            pas = false;
    ////        }
    ////        disp_str += (i == len) ? "]" : "], ";
    ////        pas = true; i += 1;
    ////    }
    ////    disp_str += "]";
    ////    print(disp_str);
    ////}
}
