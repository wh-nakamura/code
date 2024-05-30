using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrgRuleTextScript : MonoBehaviour
{
    public const float maxCost = Rule.cost;

    public GameObject orgRuleText;
    public GameObject battleTransitionButton;

    public static OrgRuleTextScript This = null;

    void Awake() {
        if (This == null) {
            This = this;
        }
    }

    public static float CalcCost(List<CharaScript> charaList)
    {
        float cost = 0;
        bool kingFlag = false;

        foreach (CharaScript charaScript in charaList)
        {
            //$b 王の場合
            if (charaScript.isKing)
            {
                cost += charaScript.cost;
                if (kingFlag) {cost -= 30;}
                kingFlag = true;

                //////$b この王のコストが他の王より高い場合
                ////if (kingCostT < charaScript.cost)
                ////{
                ////    cost -= kingCostT;
                ////    kingCostT = charaScript.cost;
                ////    cost += kingCostT;
                ////}
            }
            //$b 王でないなら
            else
            {
                cost += charaScript.cost;
            }
        }
        return cost;
    }

    public void ConfirmOrgRule(CharaObj Chara) {

        float cost = 0;
        bool kingFlag = false;
        string costText = $"コスト：";

        ////float kingCostT = 0;

        // 編成状況の確認
        cost = OrgRuleTextScript.CalcCost(Chara.onFieldChara2List[0]);
        
        if (Chara.onFieldChara2List[0].Find(charaScript => charaScript.isKing)) {kingFlag = true;}
        bool costFlag = (cost <= maxCost) ? true : false;

        OrgUIScript.This.BattleTransitionButton.interactable = (
            costFlag && kingFlag) ? true : false;

        // 文字の作成
        string textColor = (costFlag) ? "blue" : "red";
        costText += $"<color={textColor}>{cost}</color>/{maxCost} 王：";
        costText += (kingFlag) ? "<color=blue>有</color>" : "<color=red>無</color>";

        orgRuleText.GetComponent<TextMeshProUGUI>().text = costText;
    }
}