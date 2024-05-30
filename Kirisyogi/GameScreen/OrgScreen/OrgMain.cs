using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharaId;

using Cysharp.Threading.Tasks;
using System;

public class OrgMain : MonoBehaviour
{
    public static OrgInitObj init;
    public static OrgGameObj game;
    public static CharaObj chara;
    public static OrgFieldObj field;
    //public static Org_HandObj hand;

    void Start()
    {
        SaveManager.Load();
        SaveManager.LoadOrg();
        init = Func.InstantObj("SystemObj/OrgInitObj", 0).GetComponent<OrgInitObj>();
        game = Func.InstantObj("SystemObj/OrgGameObj", 0).GetComponent<OrgGameObj>();
        chara = Func.InstantObj("SystemObj/CharaObj", 0).GetComponent<CharaObj>();
        field = Func.InstantObj("SystemObj/OrgFieldObj", 0).GetComponent<OrgFieldObj>();
        init.transform.parent = GameObject.Find("ObjParent").transform;
        game.transform.parent = GameObject.Find("ObjParent").transform;
        chara.transform.parent = GameObject.Find("ObjParent").transform;
        field.transform.parent = GameObject.Find("ObjParent").transform;

        init.chara = chara; init.field = field;
        field.InitFieldDarkness();
        

        game.chara = chara; game.field = field;
        CharaData.InitMoveableCost();
        game.InitPlacement();
    }



    void Update()
    {
        game.UpdateGame();
    }
}
