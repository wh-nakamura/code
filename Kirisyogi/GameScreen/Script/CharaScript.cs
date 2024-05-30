using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharaScript : MonoBehaviour
{
    // プロパティの値の変更は、プレハブには反映されない
    public CharaId charaId;
    public int p_num;
    public (int x, int y) coordTuple;
    public GameObject obj;
    public bool mine;

    public string charaName;
    public List<(int x, int y)> posValueList;
    public int rank;
    public CharaId promoteId;
    public bool promoteState = false;
    public CharaId originId;
    public bool isKing = false;
    public List<string> skillList;
    public float cost;

    public int dupId = 1; // 同じIdのキャラを識別するためのId


    void OnMouseEnter()
    {
        SEScript.This.RingSE(SoundKind.hoverCharaSE);
    }

    //$p キャラの初期情報の設定
    public void SetChara_info(CharaScript charaScript) {
        CharaData.SetCharaInfo(charaScript);
        //GameObject Text = transform.Find("Canvas/Text").gameObject;
        //Text.GetComponent<UnityEngine.BattleUIScript.Text>().text = $"{charaName}";
        obj.name = charaName;
    }

    public string SetChara_info_text() {
        string text = "";
        text += $"　駒名：　{charaName}";
        text += "<br>　種類：　";
        text += (isKing == true) ? $"<color=red>王</color>:{rank}" : $"通常:{rank}";
        text += $"<br>コスト：　{cost}";
        text += "<br>　成り：　";
        if (promoteState) {text += "既成り";}
        else if (promoteId == 0) {text += "非成り";}
        else {
            text += "未成り";
            text += $"<br>成り先：　{CharaData.GetCharaName(promoteId)}";
        }
        if (skillList.Count != 0)
        {
            foreach (string skillKind in skillList)
            {
                text += $"<br><color=red>{skillKind}</color>：{GetSkillInfoText(skillKind)}。";
            }
            
        }
        return text;
    }

    public string GetSkillInfoText(string skillKind)
    {
        string infoText = "";

        switch (skillKind)
        {
            case "消滅":
                infoText += "この駒は取られると消滅する"; break;
            case "臆病":
                infoText += "この駒は相手の駒及び相手の霧がある位置に進めない"; break;
            case "霧生成(自)":
                infoText += "移動後の位置に自分の霧を生成する"; break;
            case "霧払い":
                infoText += "移動後の前方の位置の相手の霧を払う"; break;
            default:
                break;
        }
        return infoText;
    }

    //$p dupIdの更新
    public void DupCheck(List<CharaScript> handOrCharaList, CharaScript clickedChara, bool is_add_hand)
    {
        //$b 手札に加える場合
        if (is_add_hand) {clickedChara.dupId = 0;}

        //$b 盤面に出す場合
        else
        {
            //$b 手札から盤上に出すときに既に同じIdのキャラがいるとき
            List<CharaScript> dupChara_sc_li = handOrCharaList.FindAll(
                charaScript => charaScript.charaId == clickedChara.charaId);
            int max_dupId = 1;
            foreach (CharaScript chara_sc_t in dupChara_sc_li) {
                if (max_dupId <= chara_sc_t.dupId) {max_dupId = chara_sc_t.dupId + 1;}
            }
            clickedChara.dupId = max_dupId;
        }
        handOrCharaList.Add(clickedChara);
    }

    public bool FindSkill(params string[] skillKindArray)
    {
        foreach (string skillKindT in skillKindArray)
        {
            if (this.skillList.Find(skillKind => skillKind == skillKindT) == null)
            {
                //
            }
            else 
            {
                return true;
            }
        }
        return false;
    }
}
