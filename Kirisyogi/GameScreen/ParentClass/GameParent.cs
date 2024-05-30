using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Threading;

using UnityEngine.UI;
using Photon.Pun;

using TMPro; //TextMeshProを扱う際に必要


public abstract class GameParent: MonoBehaviour
{

    //$p ホバー
    public GameObject hoverObj = null;
    public CharaScript hoverChara = null;
    public FieldScript hoverField = null;
    
    //$p クリック
    public GameObject clickObj = null;
    public CharaScript clickedChara = null;

    // クリック時の比較用
    public GameObject clickedObj;
    public Vector3 clickedMousePos;
    
    public bool isStartTurn = true;

    //$p SystemObj
    public FieldParent field;
    public HandObj hand;
    public CharaObj chara;
    
    public DetailTabScript DetailTab;


    public virtual void Awake() 
    {
        DetailTab = DetailTabScript.This;
    }

    protected void AssignClickObj()
    {
        // クリック

        //$b ボタンダウン時
        if (Input.GetMouseButtonDown(0))
        {
            clickedObj = Mouse.GetDownObj();
            clickedMousePos = Input.mousePosition;
        }
        //$b ボタンアップ時
        else if (Input.GetMouseButtonUp(0))
        {
            ////clickObj = (clickedMousePos == Input.mousePosition) ? 
            ////    Mouse.GetUpObj() : null;
                
            clickObj = Mouse.GetUpObj();

            if (clickObj && clickObj.CompareTag("Null")) {clickObj = null;}
        }
    }

    protected void AssignHoverObj()
    {
        // ホバー

        hoverObj = Mouse.GetHoverObj();
        hoverChara = null;
        hoverField = null;

        if (hoverObj)
        {
            hoverChara = (hoverObj.CompareTag("CharaTag")) ? 
                hoverObj.GetComponent<CharaScript>() : null;
            hoverField = (hoverObj.CompareTag("FieldTag")) ? 
                hoverObj.GetComponent<FieldScript>() : null;
        }
    }

    public void HoverDispInfo(int handCount = 0)
    {
        // ホバー時の移動先情報表示

        //$b 動かすキャラを選択している場合
        if (clickedChara) {return;}

        //$b キャラをホバーしていないなら
        if (!hoverChara)
        {
            field.DelMoveColor();
            return;
        }
        // 移動先表示
        field.DispMovableCoord(chara.onFieldChara2List, hoverChara, 
            FieldColor.canMove, handCount);

        // キャラの詳細情報表示
        DetailTab.DispDetailTab(hoverChara);
    }

    // キャラ選択時の処理
    public virtual void ChoiceCharaProcess() {}

    // 使用オブジェクト以外を選択した場合の処理
    public virtual void NotClickObjProcess() {}
}
