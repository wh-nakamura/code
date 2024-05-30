using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Cysharp.Threading.Tasks;
using System;

public class InitObj: MonoBehaviour
{
    //$p フォトン関連
    public Photon.Realtime.Player ally_p;
    public Photon.Realtime.Player opp_p;

    public int p_num;
    public int turn = 1;

    public FieldObj field;
    public CharaObj chara;

    public int[] turnPArray = new int[2];
    public float costT;
    public bool costEqFlag;



    //$p 
    public void StartGame()
    {
        //BattleUIScript.Edit_simple_info_text("通信待機中・・・");
    }



    
    public void InitPlayer()
    {
        //プレイヤー別の初期化

        ally_p = PhotonNetwork.LocalPlayer;
        ally_p.InitHashtable();

        //$t 先攻後攻決める場合はここも変更
        if (PhotonNetwork.PlayerList[0] == ally_p) {p_num = 0;}
        else if (PhotonNetwork.PlayerList[1] == ally_p) {p_num = 1;}

        costT = InitPlacement(GameData.init2List, p_num);

        ally_p.send_int_d2("init2List", GameData.init2List);
    }

    public string aa()
    {
        string matchingTime = (string)DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分ss秒");
        FuncD.pri("時間", matchingTime);

        opp_p = (PhotonNetwork.PlayerList[0] == ally_p) ? 
            PhotonNetwork.PlayerList[1] : PhotonNetwork.PlayerList[0];

        opp_p.send_string("matchingTime", matchingTime);

        //Func.UniWait(this, ally_p.recv_string("matchingTime") == matchingTime);
        //var token = this.GetCancellationTokenOnDestroy();
        //await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: token);

        return ally_p.recv_string("matchingTime");
    }


    //$p init_liの受信
    public int RecvInit()
    {
        ////opp_p = PhotonNetwork.PlayerList[(p_num+1)%2];
        opp_p = (PhotonNetwork.PlayerList[0] == ally_p) ? 
            PhotonNetwork.PlayerList[1] : PhotonNetwork.PlayerList[0];

        if (ally_p.recv_int_d2("init2List") != null &&
        opp_p.recv_int_d2("init2List") != null)
        {
            List<List<List<int>>> allInit_li = new List<List<List<int>>> 
            {
                (p_num == 0) ? ally_p.recv_int_d2("init2List"): opp_p.recv_int_d2("init2List"),
                (p_num == 0) ? opp_p.recv_int_d2("init2List"): ally_p.recv_int_d2("init2List")
            };
            float costTT = InitPlacement(opp_p.recv_int_d2("init2List"), (p_num+1)%2);
            print(costT); print(costTT);
            if (costT == costTT)
            {
                turnPArray[0] = 0;
                turnPArray[1] = 1;
                costEqFlag = true;
            }
            else
            {
                if (costT < costTT)
                {
                    turnPArray[0] = p_num;
                    turnPArray[1] = (p_num+1)%2;
                }
                else
                {
                    turnPArray[0] = (p_num+1)%2;
                    turnPArray[1] = p_num;
                }
                costEqFlag = false;
            }
        } else {return 2;}
        ally_p.send_int_d2("init2List", null);
        return 3;
    }

    public float InitPlacement(List<List<int>> init2List, int now_p_num)
    {
        // 初期配置の反映

        (CharaId charaId, int x, int y) initTuple;

        //$b キャラ毎
        for (int i = 0; i < init2List.Count; i++)
        {
            initTuple = ((CharaId)init2List[i][0], init2List[i][1], init2List[i][2]); //$t init2Listをタプル型に
            
            //$b プレイヤー別にフィールドのどちら側に置くかを判定
            (int x, int y) coordTupleT = (p_num == now_p_num) ? 
                (initTuple.x, initTuple.y): 
                ((Coord.x+1)-initTuple.x, (Coord.y+1)-initTuple.y);
            
            CharaScript charaScript = FuncK.InstanceChara(
                p_num, now_p_num, initTuple.charaId, coordTupleT);

            charaScript.DupCheck(chara.onFieldChara2List[now_p_num], charaScript, false);
            field.MoveChara(charaScript);
        }
        return OrgRuleTextScript.CalcCost(chara.onFieldChara2List[now_p_num]);
    }
}
