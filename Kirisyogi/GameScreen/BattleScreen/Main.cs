using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;


public class Main : MonoBehaviourPunCallbacks
{

    public static bool start_pass;
    public static bool server_pass;
    public int init_pass = 2;

    public static InitObj init;
    public static GameObj game;
    public static CharaObj chara;
    public static FieldObj field;
    public static HandObj hand;

    public Photon.Realtime.Player ally_p;

    public static int bb = 2;
    public static bool aaa = false;
    public UniTaskScript uniTaskScript;
    
    
    void Start()
    {
        uniTaskScript = UniTaskScript.This;
        start_pass = false;
        server_pass = false;
        SaveManager.Load();
        SaveManager.LoadOrg();
        init = Func.InstantObj("SystemObj/InitObj", 0).GetComponent<InitObj>();
        game = Func.InstantObj("SystemObj/GameObj", 0).GetComponent<GameObj>();
        chara = Func.InstantObj("SystemObj/CharaObj", 0).GetComponent<CharaObj>();
        field = Func.InstantObj("SystemObj/FieldObj", 0).GetComponent<FieldObj>();
        hand = Func.InstantObj("SystemObj/HandObj", 0).GetComponent<HandObj>();
        init.transform.parent = GameObject.Find("ObjParent").transform;
        game.transform.parent = GameObject.Find("ObjParent").transform;
        chara.transform.parent = GameObject.Find("ObjParent").transform;
        field.transform.parent = GameObject.Find("ObjParent").transform;
        hand.transform.parent = GameObject.Find("ObjParent").transform;
        
        init.chara = chara; init.field = field;
        ally_p = ally_p = PhotonNetwork.LocalPlayer;
        CharaData.InitMoveableCost();

        var token = this.GetCancellationTokenOnDestroy();

        ////await UniTask.WaitUntil(() => start_pass, cancellationToken: token);
        ////print(1);
        ////await UniTask.WaitUntil(() => init_pass == PhotonNetwork.PlayerList.Length, cancellationToken: token);
        ////print(2);
        ////await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: token);
        ////print(3);
        ////for (int i = 0; i < 3; i++)
        ////{
        ////    print(aaa);
        ////    await UniTask.WaitUntil(() => aaa, cancellationToken: token);
        ////    print(1);
        ////    aaa = false;
        ////    ////await uniTaskScript.UniWait(ref aaa);
        ////    print(2);
        ////    aaa = false;
        ////    await UniTask.WaitUntil(() => aaa, cancellationToken: token);
        ////    print(3);
        ////    aaa = false;
        ////}
    
    }

    public static void InitPlayer() {
        init.InitPlayer();
        start_pass = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {aaa = true;}

        if (!start_pass)
        {
            if (ally_p.recv_int_d2("init2List") != null)
            {
                ally_p.send_int_d2("init2List", null);
                ////print(ally_p.recv_int_d2("init2List")[0][0]);
            }
            else {
                if (server_pass)
                {
                    start_pass = true;
                    string roomID = (GameData.roomID != null) ? GameData.roomID : $"{Network_manager.i}";
                    GameData.roomID = null;
                    PhotonNetwork.JoinOrCreateRoom($"Room{roomID}", Network_manager.Return_roomOption(), null);
                }
            }
        }
        if (start_pass)
        {
            //$b プレイヤーが二人になったとき
            if (init_pass == PhotonNetwork.PlayerList.Length)
            {
                init_pass = init.RecvInit(); //** 2 or 3

                if (init_pass == 3)
                {
                    game.chara = chara; game.field = field;
                    game.hand = hand;
                    game.p_num = init.p_num; game.turn = init.turn;
                    game.ally_p = init.ally_p; game.opp_p = init.opp_p;
                    game.turnPArray = init.turnPArray; game.costEqFlag = init.costEqFlag;
                    BattleUIScript.InitCostComparisonMarkText(game.costEqFlag, (game.turnPArray[0] != game.p_num));
                    //print(game.ally_p); print(game.opp_p);
                    BattleUIScript.Able_surrender();
                    BgmScript.This.ChangeBgm(BgmKind.basicBattleBgm);
                }
                
            }
            //$b 初期化完了
            else if (init_pass == 3) {game.UpdateGame();}
        }
    }
}