using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FuncD : MonoBehaviour
{
    public GameObject DebugButton;
    public GameObject DebugText;

    public static FuncD This = null;

    void Awake() {
        if (This == null) {
            This = this;
        }
    }

    public static void pri(string text1, object text2)
    {
        if (text2 == null) {text2 = "null";}
        string text3 = text1 + ":" + text2;
        print(text3);
        This.DebugText.GetComponent<TextMeshProUGUI>().text += text3 + " | ";
    }
}
