using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

public class OrgGameObj : GameParent
{
    public bool addPieceFlag = false;
    public CharaScript addChara = null;

    // UIクリック時に通常のクリック関数でnullに上書きすることを防止
    public bool uiClickFlag = false;

    public AddPieceListScript AddPieceList;
    public OrgRuleTextScript OrgRuleText;


    public override void Awake() {
        base.Awake();
        AddPieceList = AddPieceListScript.This;
        OrgRuleText = OrgRuleTextScript.This;
    }

    public void InitPlacement()
    {
        // キャラの初期配置を行う

        //$b キャラ毎
        for (int i = 0; i < GameData.init2List.Count; i++)
        {
            List<int> initListO = GameData.init2List[i]; // O = 省略のomit
            if (initListO[0] == 0) {return;}
            
            CharaScript charaScript = FuncK.InstanceChara(
                0, 0, (CharaId)initListO[0], (initListO[1], initListO[2]));
            chara.onFieldChara2List[0].Add(charaScript);
            field.MoveChara(charaScript);
        }
    }

    public void UpdateGame()
    {
        //$a ゲーム全体の処理

        //$b このターンの最初なら
        if (isStartTurn) {StartTurnProcess();}
        
        GetMouseInfo();

        HoverDispInfo();
        
        //$b クリック時
        if (Input.GetMouseButtonUp(0)) {MainProcess();}

        //$b 駒追加リストを開いてるか
        if (addPieceFlag)
        {
            field.DispPlayerCoord(null);
            OrgUIScript.This.buttons.SetActive(false);
        }
        else
        {
            field.DispPlayerCoord(hoverObj);
            OrgUIScript.This.buttons.SetActive(true);
        }
    }

    public void StartTurnProcess()
    {
        OrgRuleText.ConfirmOrgRule(chara);
        isStartTurn = false;
    }

    
    private void GetMouseInfo()
    {
        // マウスからオブジェクト情報取得

        // クリック
        //$b 追加ボタンを押していない場合
        if (!uiClickFlag) {AssignClickObj();}
        uiClickFlag = false;

        // ホバー
        //$b キャラ追加画面を開いていない場合
        if (!addPieceFlag) {
            AssignHoverObj();
        }
    }

    private void MainProcess()
    {
        //$a メインとなる処理

        if (clickObj) {ClickObjProcess();}
        else
        {
            NotClickObjProcess();

            // 無をクリックして追加画面を閉じる処理
            if (addPieceFlag) {
                CloseAddPieceList();
            }
        }
    }

    private void ClickObjProcess()
    {
        // オブジェクト選択時の処理

        //$b 移動する、又は移動先のキャラを選択したなら
        if (clickObj.CompareTag("CharaTag"))
        {
            ChoiceCharaProcess();

            //$b キャラリストから選択したなら
            if (addPieceFlag)
            {
                addChara = clickObj.GetComponent<CharaScript>();
                CloseAddPieceList();
            }
        }
        //$b 盤面を選択したなら
        else if (clickObj.CompareTag("FieldTag")) {ChoiceFieldProcess();}

        //$b 上記以外のオブジェクトを選択したなら
        else {NotClickObjProcess();}
    }

    
    public override void NotClickObjProcess()
    {
        // 意味のあるオブジェクトを選択していない場合の処理

        clickedChara = null;
        addChara = null;
        field.DelMoveColor();
    }

    
    public override void ChoiceCharaProcess()
    {
        // キャラ選択時の処理

        //$b 既に追加キャラを選択していた場合
        if (addChara)
        {
            // 盤上の駒を削除し、選択しているキャラを追加する
            CharaScript deletedChara = clickObj.GetComponent<CharaScript>();
            DeleteChara(deletedChara);
            AddCharaProcess(addChara, deletedChara.coordTuple);
        }
        //$b 既に盤面のキャラを選択していた場合
        else if (clickedChara)
        {
            // キャラ同士の位置を入れ替える
            CharaScript afterChara = clickObj.GetComponent<CharaScript>();
            ReplaceChara(clickedChara, afterChara);
        }
        //$b まだキャラを選択していなかった場合
        else
        {
            // クリックしたキャラを選択したキャラとして設定する
            clickedChara = clickObj.GetComponent<CharaScript>();

            //$b 追加キャラを選択していない場合
            if (!addPieceFlag)
            {
                // 選択したキャラの現在地表示
                field.DispTheCoord(clickedChara.transform.position);
            }
            return;
        }
        EndMove();
    }

    
    private void ChoiceFieldProcess()
    {
        // 盤面選択時の処理

        (int x, int y) coordTupleT = (
            (int)clickObj.transform.position.x, (int)clickObj.transform.position.y);

        //$b 自陣を選択したなら
        if (coordTupleT.y <= Coord.mistRow)
        {
            //$b 選択画面のキャラを選択している状態なら
            if (addChara)
            {
                // 選択したマスにキャラを追加する

                //$b そのマスにキャラが既にいる場合
                if (field.charaData[coordTupleT.y, coordTupleT.x])
                {
                    // 既にいるキャラを削除する
                    CharaScript deletedChara = field.charaData[
                        coordTupleT.y, coordTupleT.x];
                    DeleteChara(deletedChara);
                }
                AddCharaProcess(addChara, coordTupleT);
            }
            //$b 盤面のキャラを選択している状態なら
            else if (clickedChara)
            {
                // 選択したマスにキャラを移動する

                //$b そのマスにキャラが既にいる場合
                if (field.charaData[coordTupleT.y, coordTupleT.x])
                {
                    // キャラ同士の位置を入れ替える
                    CharaScript beforeChara = field.charaData[
                        coordTupleT.y, coordTupleT.x];
                    ReplaceChara(beforeChara, clickedChara);
                }
                //$b いない場合
                else
                {
                    // 選択したマスにキャラを移動する
                    clickedChara.coordTuple = coordTupleT;
                    field.MoveChara(clickedChara);
                }
            }
            //$b まだキャラを選択していない状態なら
            else {return;}

            EndMove();
        }
    }

    
    public void EndMove()
    {
        // ターン終了時の処理

        clickedChara = null;
        addChara = null;
        isStartTurn = true;

        field.DelMoveColor();
        Save();
    }

    public void OpenAddPieceList()
    {
        AddPieceList.addPieceListObject.SetActive(true);
        addPieceFlag = true;
    }

    public void CloseAddPieceList()
    {
        AddPieceList.addPieceListObject.SetActive(false);
        addPieceFlag = false;
        GameObject.Find("CollisionFilter").GetComponent<BoxCollider2D>().enabled = false;
    }

    public void AddCharaProcess(CharaScript addChara, (int x, int y) coordTupleT)
    {
        // キャラを追加する処理

        CharaScript addCharaT = FuncK.InstanceChara(0, 0, addChara.charaId, coordTupleT);
        field.charaData[coordTupleT.y, coordTupleT.x] = addCharaT;
        chara.onFieldChara2List[0].Add(addCharaT);
        field.MoveChara(addCharaT);
    }

    public void DeleteChara(CharaScript deletedChara = null)
    {
        // キャラを削除する処理

        //$b 削除するキャラを選択していない場合
        if (!clickedChara) {return;}
        //$b 削除ボタンによる削除か
        if (!deletedChara) {deletedChara = clickedChara;}

        Destroy(deletedChara.obj);
        chara.onFieldChara2List[0].Remove(deletedChara);
        field.charaData[deletedChara.coordTuple.y, 
            deletedChara.coordTuple.x] = null;
        isStartTurn = true;
    }

    public void DeleteAllOrgChara()
    {
        // 編成しているキャラを全て削除する処理

        int len = chara.onFieldChara2List[0].Count;
        for (int i = chara.onFieldChara2List[0].Count - 1; i >= 0 ; i--)
        {
            CharaScript deletedChara = chara.onFieldChara2List[0][i];
            Destroy(deletedChara.obj);
            chara.onFieldChara2List[0].Remove(deletedChara);
            field.charaData[deletedChara.coordTuple.y, 
                deletedChara.coordTuple.x] = null;
        }
        isStartTurn = true;
    }

    public void ResetOrgChara()
    {
        // 編成をデフォルト編成に戻す処理

        DeleteAllOrgChara();
        GameData.init2List = new List<List<int>>()
        {
            new List<int>() {2, 1, 1},
            new List<int>() {4, 2, 1},
            new List<int>() {3, 3, 1},
            new List<int>() {1, 2, 2},
        };
        InitPlacement();
    }

    public void ReplaceChara(CharaScript beforeChara, CharaScript afterChara)
    {
        // キャラの位置を入れ替える処理

        (int x, int y) coordTupleT;
        coordTupleT = beforeChara.coordTuple;
        beforeChara.coordTuple = afterChara.coordTuple;
        afterChara.coordTuple = coordTupleT;

        field.MoveChara(beforeChara);
        field.MoveChara(afterChara);
    }

    async public void Save()
    {
        // 編成データのセーブ

        List<List<int>> init2List = new List<List<int>> {};

        //$b キャラ毎
        foreach (CharaScript charaT in chara.onFieldChara2List[0]) {
            init2List.Add(new List<int> {(int)charaT.charaId, 
                charaT.coordTuple.x, charaT.coordTuple.y});
        }
        GameData.init2List = init2List;

        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.WaitUntil(() => SaveManager.Save(), cancellationToken: token);
    }
}
