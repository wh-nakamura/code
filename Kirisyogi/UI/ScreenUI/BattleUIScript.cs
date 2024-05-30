using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;

[System.Serializable]
public class BattleUIScript : MonoBehaviour
{

    //$p ゲームオブジェクト
    //public GameObject Retry_button_t;
    //public GameObject Result_text_t;
    public GameObject ResultObj;
    public GameObject Result_text;
    public GameObject simple_info_text;
    public GameObject CostComparisonText;
    public string CostComparisonMarkText;
    public Button surrender_button;
    public static GameObject info_text;
    public static GameObject test_text;

    public static BattleUIScript test = null;

    void Awake() {
        if (test == null) {
            test = this;
        }
    }
    

    void Start() {
        //Retry_button = Retry_button_t;
        //Result_text = Result_text_t;
        //print(Retry_button);
        
    }

    public static void Hide() {
        BattleUIScript.test.ResultObj.SetActive(false);
    }

    public static void Disp_result() {
        BattleUIScript.test.ResultObj.SetActive(true);
        SEScript.This.RingSE((Main.game.win_p_num == Main.game.p_num) ? 
            SoundKind.VictorySE : SoundKind.DefeatSE);
    }

    public static void Edit_text(GameObject textObj, string text_t) {
        textObj.GetComponent<TextMeshProUGUI>().text = text_t;
    }

    public static void Edit_simple_info_text(string text_t) {
        test.simple_info_text.GetComponent<TextMeshProUGUI>().text = text_t;
    }

    public static void Able_surrender() {
        BattleUIScript.test.surrender_button.interactable = true;
    }

    public static void InitCostComparisonMarkText(bool costEqFlag, bool isThanCost)
    {
        string textT;
        if (costEqFlag) {textT = "＝";}
        else {textT = (isThanCost) ? "＞" : "＜";}
        test.CostComparisonMarkText = textT;
        test.CostComparisonText.SetActive(true);
    }

    public static void EditCostComparisonText(bool isMyTurn)
    {
        string textO = test.CostComparisonText.GetComponent<TextMeshProUGUI>().text;
        string textT = (isMyTurn) ? 
            $"<color=red>●</color>    {test.CostComparisonMarkText}    <color=white>●</color>": 
            $"<color=white>●</color>    {test.CostComparisonMarkText}    <color=red>●</color>";
        test.CostComparisonText.GetComponent<TextMeshProUGUI>().text = textT;
    }

    //$p 勝敗表示
    public static string Create_result_text(bool win, int resultReason) {
        string result_text = "";
        switch (resultReason) {
            case Because.takeKing:
                result_text = win ? "王を取りました": "王が取られました";
                break;
            case Because.arrivalKing:
                result_text = win ? "あなたの王が\r\n端に着きました": "相手の王が\r\n端に着きました";
                break;
            case Because.surrender:
                result_text = win ? "相手が降参しました": "降参を選択しました";
                break;
            case Because.timeOut:
                result_text = win ? "相手の時間切れです": "時間切れです";
                break;
            case Because.disconnect:
                result_text = win ? "相手の通信が\r\n切断されました": "あなたの通信が\r\n切断されました";
                break;
            case Because.turnMax:
                result_text = win ? "100ターン目です\r\n初期編成コスト差で": "100ターン目です\r\n初期編成コスト差で";
                break;
        }
        result_text += win ? "\r\nあなたの勝利です！": "\r\nあなたの敗北です";
        
        return result_text;
    }
}
