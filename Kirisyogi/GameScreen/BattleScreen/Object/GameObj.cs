using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Threading;

using UnityEngine.UI;
using Photon.Pun;

using TMPro; //TextMeshProを扱う際に必要



public class GameObj: GameParent
{
    // フォトン関連
    public Photon.Realtime.Player ally_p;
    public Photon.Realtime.Player opp_p;

    public TimeBarManager time_bar_sc;

    public int turn;
    public int win_p_num = 2;
    public int gamePhase = 1;
    public int p_num;
    public int resultReason;
    public int turnCount = 0;

    public bool kingBgmFlag = false;
    public int[] turnPArray;
    public bool costEqFlag;


    public override void Awake()
    {
        base.Awake();
    }

    //$c ゲーム全体の処理
    public void UpdateGame()
    {
        switch (gamePhase)
        {
            //$b 対戦中
            case Phase.whileGame:
            {
                //$b 相手の通信の切断時
                if (PhotonNetwork.PlayerList.Length == 1) {DisconnectOppProcess();} //$t 観戦にも対応できるようにする
                //$b このターンの最初なら
                if (isStartTurn) {StartTurnProcess();}

                //$b 勝敗がついた場合
                if (win_p_num != 2) {ResultProcess();}
                //$b 勝敗がついていない場合
                else
                {
                    GetMouseInfo();
                    
                    HoverDispInfo(hand.hand_li[p_num].Count);
                    
                    MainGame();

                    field.DispPlayerCoord(hoverObj);
                }
                break;
            }

            //$b 対戦終了
            case Phase.resultGame:
            {
                try {
                    if (resultReason == Because.disconnect || opp_p.recv_bool("check_result"))
                    {
                        BgmScript.This.StopBgm();
                        Invoke("InvokeDiscon", 0.5f);
                        //Network_manager.Discon();
                        
                        gamePhase = Phase.afterResult;
                    }
                } catch (System.NullReferenceException) {;}
                break;
            }
            
            //$b 対戦終了後
            case Phase.afterResult:
            {
                GetMouseInfo();
                HoverDispInfo(hand.hand_li[p_num].Count);
                field.DispPlayerCoord(hoverObj);
                break;
            }
        }
    }

    public void EndDeleteMist()
    {
        // 霧の削除
        for (int y = 1; y <= Coord.y; y++)
        {
            for (int x = 1; x <= Coord.x; x++)
            {
                if (field.fieldData[y, x].mistStatus == Mist.oppMist)
                {
                    field.fieldData[y, x].mistStatus = Mist.allyMist;
                }
            }
        }
        field.UpdateCharaActive();
    }

    private void StartTurnProcess()
    {
        turnCount += 1;
        bool isMyTurn = (p_num == turnPArray[(turnCount+1)%2]);
        //$b 自分のターンなら
        if (isMyTurn) {time_bar_sc.countPass = true;}
        
        string turnText = ((isMyTurn) ? "自分のターン" : "相手のターン") + $"({turnCount})";
        if (costEqFlag) {turnText += "*";}
        BattleUIScript.Edit_simple_info_text(turnText);
        time_bar_sc.slider.value = time_bar_sc.max_value;
        BattleUIScript.EditCostComparisonText(isMyTurn);

        isStartTurn = false;
    }

    
    private void GetMouseInfo()
    {
        // マウスからオブジェクト情報取得
        
        AssignClickObj();
        AssignHoverObj();
    }

    private void MainGame()
    {
        // ゲーム処理

        //$b 自分ターン
        if (p_num == turnPArray[(turnCount+1)%2])
        {
            //$b オブジェクトを選択していないなら
            if (clickObj) {ChoiceObject();}
            else {NotClickObjProcess();}
        }
        //$b 相手ターン
        else
        {
            //$b 相手の情報を受け取ったら
            if (opp_p.recv_int_d1("moveChara_li") != null)
            {
                ally_p.send_bool("ack", true);

                Ref_recv(opp_p.recv_int_d1("moveChara_li"));
                opp_p.send_int_d1("moveChara_li", null);
            }
        }
        CheckResult();

        void CheckResult()
        {
            //$b 相手から正規以外の勝敗が送られた場合
            switch (opp_p.recv_int("resultReason"))
            {
                case Because.none:
                    return;
                case Because.surrender:
                    resultReason = Because.surrender; break;
                case Because.timeOut:
                    resultReason = Because.timeOut; break;
                default:
                    print(opp_p.recv_int("resultReason")); break;
            }
            win_p_num = p_num;
            opp_p.send_int("resultReason", 0);
        }
        
    }
    
    public override void NotClickObjProcess()
    {
        // 使用オブジェクト以外を選択した場合の処理

        clickedChara = null;
        //field.DelMoveColor();
    }

    private void ChoiceObject()
    {
        // オブジェクト選択時の処理

        //$b 移動先を選択したなら
        if (clickObj.CompareTag("MarkTag")) {
            ChoiceMarkProcess();
        }
        //$b キャラを選択したなら
        else if (clickObj.CompareTag("CharaTag")) {
            ChoiceCharaProcess();
        }
        //$b 上記以外のオブジェクトを選択したなら
        else {
            NotClickObjProcess();
        }
    }

    public void ChoiceMarkProcess()
    {
        // 移動先選択時の処理

        // 選択したマークの座標
        (int x, int y) destTuple = Func.returnPos(clickObj); // destination 行き先

        int[] moveChara_li = new int[] { //** {1, 3, 4, 1} 座標は移動後の相手目線での座標
            (int)clickedChara.charaId, ((Coord.x+1)-destTuple.x), ((Coord.y+1)-destTuple.y), clickedChara.dupId};

        //$b 手札から移動する場合
        if (!field.IsInField(Func.returnPos(clickedChara.obj))) {
            clickedChara.DupCheck(chara.onFieldChara2List[clickedChara.p_num], clickedChara, false);
            hand.hand_li[clickedChara.p_num].Remove(clickedChara);
        }

        opp_p.send_bool("ack", false);
        ally_p.send_int_d1("moveChara_li", moveChara_li);

        //Invoke(nameof(AckCheck), 0.5f);

        MovePiece(destTuple);

        EndProcess();
    }

    //public void AckCheck()
    //{
    //    print("ackCheck");
    //    if (opp_p.recv_bool("ack")) {
    //        opp_p.send_bool("ack", false);
    //    } else {
    //        //List<int> moveChara_li = ally_p.recv_int_d1("moveChara_li");
    //        //ally_p.send_int_d1("moveChara_li", moveChara_li);
    //        print("送信");
    //        //Func.print_r(moveChara_li);
    //    }
    //    CancelInvoke();
    //}

    
    public override void ChoiceCharaProcess()
    {
        // キャラ選択時の処理

        clickedChara = clickObj.GetComponent<CharaScript>();

        //$b 自分のキャラなら
        if (clickedChara.mine)
        {
            // 移動先表示
            field.DispMovableCoord(chara.onFieldChara2List, clickedChara, 
                FieldColor.attackMove, hand.hand_li[p_num].Count);
        }
        //$b 相手のキャラなら
        else
        {
            clickedChara = null;
        }
    }

    
    public void MovePiece((int x, int y) destTuple)
    {
        // 駒移動時の処理

        (int x, int y) beforeTuple = (clickedChara.coordTuple.x, clickedChara.coordTuple.y);

        //$b 移動先に駒があった場合
        if (field.charaData[destTuple.y, destTuple.x])
        {
            CharaScript deleteChara_sc = field.charaData[destTuple.y, destTuple.x];
            
            //$p 取られた駒の処理
            chara.onFieldChara2List[deleteChara_sc.p_num].Remove(deleteChara_sc);
            Destroy(deleteChara_sc.obj);

            //$b 消滅する駒の場合
            if (deleteChara_sc.FindSkill("消滅"))
            {
                SEScript.This.RingSE(SoundKind.ExtinctionSE);
            }
            //$b 消滅しない駒の場合
            else
            {
                CharaId handCharaId = (deleteChara_sc.promoteState) ? 
                    deleteChara_sc.originId : deleteChara_sc.charaId;

                //$p 手駒の生成
                CharaScript charaScript = FuncK.InstanceChara(
                    p_num, clickedChara.p_num, handCharaId, (0, 0));

                //$p 取られた駒の処理
                charaScript.DupCheck(hand.hand_li[charaScript.p_num], charaScript, true);
                chara.onFieldChara2List[deleteChara_sc.p_num].Remove(deleteChara_sc);
                Destroy(deleteChara_sc.obj);

                //$b 王がとられた場合
                if (deleteChara_sc.isKing)
                {
                    win_p_num = clickedChara.p_num;
                    resultReason = Because.takeKing;
                }
            }
        }

        //$p 霧の処理
        
        int mistStatusO = field.fieldData[destTuple.y, destTuple.x].mistStatus;
        bool mistOpenFlag = false;

        //$b 移動先に霧があった場合
        if (mistStatusO != Mist.noMist)
        {
            //$b 移動した駒から見て相手の霧なら
            if ((clickedChara.mine == true) && (mistStatusO == Mist.oppMist) ||
            (clickedChara.mine == false) && (mistStatusO == Mist.allyMist))
            {
                field.fieldData[destTuple.y, destTuple.x].mistStatus = Mist.noMist;
                mistOpenFlag = true;
            }
        }

        //$b 移動後の効果を持つ場合
        if (clickedChara.FindSkill("霧生成(自)"))
        {
            int mistStatus = (clickedChara.mine) ? Mist.allyMist : Mist.oppMist;

            //$b 移動先の位置に自分の霧が無い場合
            if (field.fieldData[destTuple.y, destTuple.x].mistStatus != mistStatus)
            {
                field.fieldData[destTuple.y, destTuple.x].mistStatus = mistStatus;
                SEScript.This.RingSE(SoundKind.CreateMistSE);
            }
        }
        else if (clickedChara.FindSkill("霧払い"))
        {
            int oppMist = (!clickedChara.mine) ? Mist.allyMist : Mist.oppMist;
            int destTupleY = destTuple.y + ((clickedChara.mine) ? 1 : -1);

            //$b 前方の座標が範囲内なら
            if ((Coord.o <= destTupleY) && (destTupleY <= Coord.y))
            {
                print(field.fieldData[destTupleY, destTuple.x].mistStatus);
                //$b 移動先の前方に相手の霧がある場合
                if (field.fieldData[destTupleY, destTuple.x].mistStatus == oppMist)
                {
                    field.fieldData[destTupleY, destTuple.x].mistStatus = Mist.noMist;
                    SEScript.This.RingSE(SoundKind.CreateMistSE);
                }
            }
        }

        //$p 王が霧から出た場合のBGM変更の処理

        if (kingBgmFlag == false)
        {
            if (clickedChara.isKing && ((mistStatusO == Mist.noMist) || mistOpenFlag) && !clickedChara.FindSkill("霧生成(自)"))
            {
                BgmScript.This.ChangeBgm(BgmKind.finalBattleBgm);
                kingBgmFlag = true;
            }
        }

        //$p 成る処理

        //$b プレイヤーごとの成り座標に移動した場合
        if (((clickedChara.mine == true) && (Coord.y - Coord.mistRow < destTuple.y)) ||
        ((clickedChara.mine == false) && (destTuple.y < Coord.o + Coord.mistRow)))
        {
            //$b その駒が成っていない場合
            if (!clickedChara.promoteState)
            {
                //$b 成る駒の場合かつ手駒から出ていない場合
                if (clickedChara.promoteId != 0 && (beforeTuple.y % 6) != 0)
                {
                    clickedChara = ConsistsPiece();
                }
            }
        }

        //$p 端の処理

        //$b プレイヤー毎の相手の端に移動した場合
        if (((clickedChara.mine == true) && (destTuple.y == Coord.y)) ||
        ((clickedChara.mine == false) && (destTuple.y == Coord.o)))
        {
            //$b 王が移動した場合
            if (clickedChara.isKing && 20 < turnCount)
            {
                win_p_num = clickedChara.p_num;
                resultReason = Because.arrivalKing;
            }
        }

        //$p 移動に関する盤面色の処理
        
        field.BackMovedFieldColor();
        
        //$b 盤面から移動している場合
        if (field.IsInField(beforeTuple))
        {
            field.fieldData[beforeTuple.y, beforeTuple.x].status = FieldColor.beforeMove;
            field.movedFieldList.Add(field.fieldData[beforeTuple.y, beforeTuple.x]);
        }
        field.fieldData[destTuple.y, destTuple.x].status = FieldColor.beforeMove;
        field.movedFieldList.Add(field.fieldData[destTuple.y, destTuple.x]);
        
        //$b 相手のキャラの場合
        if (!clickedChara.mine)
        {
            //$b 盤面から移動している場合
            if (field.IsInField(beforeTuple))
            {
                //$b 移動前の座標、又は移動後の座標に相手の霧がかかっている場合
                if ((field.fieldData[beforeTuple.y, beforeTuple.x].mistStatus == Mist.mist) ||
                field.fieldData[destTuple.y, destTuple.x].mistStatus == Mist.mist)
                {
                    for (int y = 1; y <= Coord.y; y++)
                    {
                        for (int x = 1; x <= Coord.x; x++)
                        {
                            if (field.fieldData[y, x].mistStatus == Mist.mist)
                            {
                                field.fieldData[y, x].status = FieldColor.beforeMove;
                                field.movedFieldList.Add(field.fieldData[y, x]);
                            }
                        }
                    }
                }
            }
            //$b 手札から移動している場合
            else 
            {
                //$b 移動後の座標に相手の霧がかかっている場合
                if (field.fieldData[destTuple.y, destTuple.x].mistStatus == Mist.mist)
                {
                    for (int y = 1; y <= Coord.y; y++)
                    {
                        for (int x = 1; x <= Coord.x; x++)
                        {
                            if (field.fieldData[y, x].mistStatus == Mist.mist)
                            {
                                field.fieldData[y, x].status = FieldColor.beforeMove;
                                field.movedFieldList.Add(field.fieldData[y, x]);
                            }
                        }
                    }
                }
            }
        }

        //$p 移動
        clickedChara.coordTuple = destTuple;
        field.MoveChara(clickedChara);
        //SEScript.This.RingSE(SoundKind.PieceMoveSE);
    }

    //$c 駒が成る処理
    private CharaScript ConsistsPiece()
    {
        // キャラ情報の設定
        CharaScript consistsChara_sc = FuncK.InstanceChara(
            p_num, clickedChara.p_num, clickedChara.promoteId, clickedChara.coordTuple);

        Vector3 beforePos = 
            consistsChara_sc.obj.transform.position = clickedChara.obj.transform.position;
        consistsChara_sc.originId = clickedChara.charaId;
        consistsChara_sc.cost = clickedChara.cost;
        consistsChara_sc.promoteState = true;
        
        chara.onFieldChara2List[clickedChara.p_num].Remove(clickedChara);
        chara.onFieldChara2List[clickedChara.p_num].Add(consistsChara_sc);
        field.charaData[(int)beforePos.y, (int)beforePos.x] = null;
        Destroy(clickedChara.obj);

        return consistsChara_sc;
    }

    public void Ref_recv(List<int> moveChara_li)
    {
        // 相手から受け取った情報の反映

        //$p 受信したキャラの検索

        //$b charaIdとdupIdから盤上検索
        clickedChara = chara.onFieldChara2List[(p_num+1)%2].Find(
            charaScript => charaScript.charaId == (CharaId)moveChara_li[0] & 
            charaScript.dupId == moveChara_li[3]);
        //$b 盤上にいない場合charaIdから手札検索
        if (!clickedChara) {
            clickedChara = hand.hand_li[(p_num+1)%2].Find(
                charaScript => charaScript.charaId == (CharaId)moveChara_li[0]);
        }

        //$b 手札から移動する場合
        if (!field.IsInField(clickedChara.coordTuple))
        {
            clickedChara.DupCheck(
                chara.onFieldChara2List[clickedChara.p_num], clickedChara, false);
            hand.hand_li[clickedChara.p_num].Remove(clickedChara);
        }
        // 移動
        MovePiece((moveChara_li[1], moveChara_li[2]));

        EndProcess();
    }

    
    private void EndProcess()
    {
        // ターン終了時の処理

        hand.hand_update(hand.hand_li);
        field.DelMoveColor();

        clickedChara = null;
        time_bar_sc.countPass = false;
        isStartTurn = true;
        turn = (turn+1)%2;

        // 受信データの削除
        opp_p.send_int_d1("moveChara_li", null);

        SEScript.This.RingSE(SoundKind.TurnChangeSE);

        if (100 <= turnCount)
        {
            turnCount--;
            win_p_num = (costEqFlag) ? turnPArray[1] : turnPArray[0];
            resultReason = Because.turnMax;
            ally_p.send_int("resultReason", (int)resultReason);
        }
    }

    private void ResultProcess()
    {
        // 勝敗時の処理

        time_bar_sc.countPass = false;

        BattleUIScript.Disp_result();
        
        Func.EditText(BattleUIScript.test.Result_text, 
            BattleUIScript.Create_result_text(win_p_num == p_num, resultReason));

        gamePhase = Phase.resultGame;
        
        // 自分の画面でゲームの勝敗が決したことを送信
        opp_p.send_bool("check_result", true);
    }

    public void SurrenderProcess()
    {
        // サレンダー処理

        win_p_num = (p_num+1)%2;
        resultReason = Because.surrender;
        ally_p.send_int("resultReason", (int)resultReason);
        
    }

    public void TimeOutProcess()
    {
        // 時間切れ処理

        win_p_num = (p_num+1)%2;
        resultReason = Because.timeOut;
        ally_p.send_int("resultReason", (int)resultReason);
    }

    public void DisconnectOppProcess()
    {
        // 相手の通信切断時の処理

        win_p_num = p_num;
        resultReason = Because.disconnect;
    }
    
    private void InvokeDiscon()
    {
        // ゲーム終了時の通信切断の遅延

        Network_manager.Discon();
    }
}
