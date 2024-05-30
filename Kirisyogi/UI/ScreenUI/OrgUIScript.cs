using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;

[System.Serializable]
public class OrgUIScript : MonoBehaviour
{

    //$p ゲームオブジェクト
    public GameObject buttons;
    public Button BattleTransitionButton;
    public GameObject CollisionFilter;

    public static OrgUIScript This = null;

    void Awake() {
        if (This == null) {
            This = this;
        }
    }

    public static void Edit_text(GameObject textObj, string text_t) {
        textObj.GetComponent<TextMeshProUGUI>().text = text_t;
    }
}