using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using System;

using static CharaId;

//$p 霧将棋の便利関数置き場
public class FuncK : MonoBehaviour {

    //$p キャラのインスタンス作成
    public static CharaScript InstanceChara(int p_num, int chara_p_num, CharaId charaId, (int x, int y) coordTupleT) {

        ////GameObject charaObj = Instantiate((GameObject)Resources.Load(
        ////    "Syogi/Def_piece"), new Vector3(0, 0, Layer.piece), 
        ////    Quaternion.identity) as GameObject;
        GameObject charaObj = Instantiate((GameObject)Resources.Load(
            "Syogi/Chara/PieceTemplates/PieceTemplate"), new Vector3(0, 0, Layer.piece), 
            Quaternion.identity) as GameObject;

        // 生成場所指定
        charaObj.transform.parent = GameObject.Find(
            "PieceParent/" + ((p_num == chara_p_num) ? "AllyParent": "OppParent")).transform;
        
        return SetChara_info(charaObj, p_num, chara_p_num, charaId, coordTupleT);
    }

    //$p フィールドのインスタンス作成
    public static GameObject InstanceField(int[] coord) {

        GameObject fieldObj = Instantiate((GameObject)Resources.Load(
            "Syogi/Field/Fields"), new Vector3(coord[0], coord[1], Layer.field), 
            Quaternion.identity) as GameObject;
        Instance_after(fieldObj, "FieldParent");

        return fieldObj;
    }

    //$p Instantiate後の後処理
    public static void Instance_after(GameObject gameObject, string parentName) {
        gameObject.transform.parent = GameObject.Find(parentName).transform;
        gameObject.name = gameObject.name.Replace( "(Clone)", "" );
    }


    //$p キャラ情報の設定
    public static CharaScript SetChara_info(GameObject charaObj, int p_num, int chara_p_num, CharaId charaId, (int x, int y) coordTupleT) {
        
        CharaScript charaScript = charaObj.GetComponent<CharaScript>();
        charaScript.obj = charaObj;

        ////charaObj.GetComponent<SpriteRenderer>().sprite = 
        ////    Resources.Load<Sprite>(CharaData.Collation_prefab(charaId));

        charaScript.p_num = chara_p_num;
        charaScript.mine = (p_num == chara_p_num);

        charaScript.charaId = charaId;
        charaScript.coordTuple = coordTupleT;
        charaScript.SetChara_info(charaScript);

        SetCharaName(charaScript);
        SetMovableMark(charaScript);

        if (!charaScript.mine)
        {
            Vector3 movableMarksAngles = charaObj.transform.Find("MovableMarks").transform.eulerAngles;
            movableMarksAngles.z = 180;
            charaObj.transform.Find("MovableMarks").transform.eulerAngles = movableMarksAngles;

            Vector3 angles = charaObj.transform.eulerAngles;
            angles.z = 180;
            charaObj.transform.eulerAngles = angles;

            //charaObj.GetComponent<SpriteRenderer>().flipX = true;
            //charaObj.GetComponent<SpriteRenderer>().flipY = true;
        }
        
        return charaScript;
    }

    public static void SetCharaName(CharaScript charaScript)
    {
        GameObject nameObj = charaScript.obj.transform.Find("CharaName/CharaNameText").gameObject;
        nameObj.GetComponent<TextMeshProUGUI>().text = charaScript.charaName;

        Color nameTextColor;

        if ((int)TotalCharaNum < (int)charaScript.charaId) {nameTextColor = Color.gray;}
        else if (charaScript.isKing) {nameTextColor = Color.red;}
        else {nameTextColor = Color.black;}

        //////$b 既に成っている場合
        ////if (charaScript.promoteState) {nameTextColor = Color.red;}
        //////$b 成らない場合
        ////else if (charaScript.promoteId == 0) {nameTextColor = Color.black;}
        //////$b 成る場合
        ////else {nameTextColor = Color.black;}

        nameObj.GetComponent<TextMeshProUGUI>().color = nameTextColor;
    }

    public static void SetMovableMark(CharaScript charaScript)
    {
        GameObject movableMark;

        //$b 移動先毎
        for (int i = 0; i < charaScript.posValueList.Count; i++)
        {
            //$b 2マス以上の場合
            if (1 < Math.Abs(charaScript.posValueList[i].x) || 1 < Math.Abs(charaScript.posValueList[i].y))
            {
                movableMark = charaScript.obj.transform.Find("MovableMarks/MovableMark" + 
                $"({charaScript.posValueList[i].x/2}, {charaScript.posValueList[i].y/2})").gameObject;
                movableMark.SetActive(true);
                try
                {
                    Image image = movableMark.GetComponent<Image>();
                    image.sprite = Resources.Load<Sprite>("Syogi/Picture/PieceTemplate/MovableMark2_3");

                }
                catch (System.NullReferenceException)
                {
                    SpriteRenderer spriteRenderer = movableMark.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = Resources.Load<Sprite>("Syogi/Picture/PieceTemplate/MovableMark2_3");
                }
                continue;
            }

            movableMark = charaScript.obj.transform.Find("MovableMarks/MovableMark" + 
            $"({charaScript.posValueList[i].x}, {charaScript.posValueList[i].y})").gameObject;
            movableMark.SetActive(true);
        }
    }
}
