using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class FieldParent: MonoBehaviour {

    [SerializeField]
    //$p ゲームオブジェクト定数
    public const string CollisionMark = "Syogi/Mark/CollisionMark";
    public const string NowCharaMark = "Syogi/Mark/NowCharaMark";
    public const string NowFieldMark = "Syogi/Mark/NowFieldMark";

    public List<FieldScript> movableFieldList = new List<FieldScript>();
    public List<GameObject> movableFieldMarkList = new List<GameObject>();

    public List<FieldScript> movedFieldList = new List<FieldScript>();
    
    public FieldScript[, ] fieldData = new FieldScript[Coord.y + 1, Coord.x + 1]; // 盤面
    public CharaScript[, ] charaData = new CharaScript[Coord.y + 1, Coord.x + 1]; // 盤面にいるキャラ
    public CharaScript[, ] dispCharaData = new CharaScript[Coord.y + 1, Coord.x + 1]; // 画面に反映されている盤面のキャラ

    public GameObject nowCoordMark;
    public GameObject pastCoordMark;

    
    public virtual void Awake()
    {
        CreateFieldObj();
        InitFieldMist();
    }

    public void CreateFieldObj()
    {
        // 盤面の作成

        GameObject fieldObj;
        for (int y = Coord.o; y <= Coord.y; y++)
        {
            for (int x = Coord.o; x <= Coord.x; x++)
            {
                fieldObj = FuncK.InstanceField(new int[] {x, y});
                fieldObj.name += $"({x}, {y})";
                fieldData[y, x] = fieldObj.transform.GetChild(0).GetComponent<FieldScript>();
                fieldData[y, x].obj = fieldObj;
            }
        }
    }

    public void InitFieldMist()
    {
        // 最初の霧の情報の代入

        for (int y = 1; y <= Coord.mistRow; y++)
        {
            for (int x = 1; x <= Coord.x; x++)
            {
                fieldData[y, x].mistStatus = Mist.allyMist;
                fieldData[Coord.y - (y-1), x].mistStatus = Mist.oppMist;
            }
        }
    }

    public void CreateCoordMark(int x, int y, int markKind)
    {
        // 座標の表示
        
        // 移動可能座標の色変え
        movableFieldList.Add(fieldData[y, x]);
        fieldData[y, x].status = markKind;

        // 移動可能座標の当たり判定
        movableFieldMarkList.Add(Instantiate(
            (GameObject)Resources.Load(CollisionMark), new Vector3(
            (float)x, (float)y, Layer.mark), Quaternion.identity) as GameObject);
    }

    public void DispPlayerCoord(GameObject hoverObj)
    {
        // プレイヤーの現在地の更新

        //$b 前の表示があるなら削除
        if (nowCoordMark) {Destroy(nowCoordMark); nowCoordMark = null;}

        //$b オブジェクトをホバーしているなら
        if (hoverObj)
        {
            nowCoordMark = Instantiate((GameObject)Resources.Load(
                (hoverObj.CompareTag("CharaTag")) ?  NowCharaMark : NowFieldMark), 
                new Vector3(hoverObj.transform.position.x, 
                hoverObj.transform.position.y, 
                Layer.mark), Quaternion.identity) as GameObject;
        }
    }

    
    public void MoveChara(CharaScript clickedChara)
    {
        // キャラの移動

        //__ unityオブジェクトの座標変数はpos
        //__ キャラオブジェクトが持つ座標配列の変数はcoord
        
        Vector3 beforePos = clickedChara.obj.transform.position; // 現在地
        //print("現在地"); print(beforePos.x); print(beforePos.y);
        //print("移動先"); print(clickedChara.coordTuple[0]); print(clickedChara.coordTuple[1]);
        if (IsInField((beforePos.x, beforePos.y))) {
            if (charaData[(int)beforePos.y, (int)beforePos.x] == clickedChara)
            {
                charaData[(int)beforePos.y, (int)beforePos.x] = null;
            }
        }
        charaData[(int)clickedChara.coordTuple.y, (int)clickedChara.coordTuple.x] = clickedChara;
        beforePos.x = clickedChara.coordTuple.x;
        beforePos.y = clickedChara.coordTuple.y;
        clickedChara.obj.transform.position = beforePos;
        UpdateCharaActive();
    }

    public void UpdateCharaActive()
    {
        // 

        bool isMist;

        for (int y = 1; y <= Coord.y; y++)
        {
            for (int x = 1; x <= Coord.x; x++)
            {
                isMist = false;

                //$b 霧以上なら
                if (Mist.mist <= fieldData[y, x].mistStatus) {isMist = true;}
                if (charaData[y, x])
                {
                    if (isMist) {charaData[y, x].obj.SetActive(false);}
                    else {charaData[y, x].obj.SetActive(true);}
                }
            }
        }
    }

    public bool IsInField((float x, float y) coordTupleT)
    {
        // その座標が盤面内か

        //** (1 <= x <= 3) & (1 <= y <= 5)
        return (((Coord.o <= coordTupleT.x) && (coordTupleT.x <= Coord.x)) && 
            ((Coord.o <= coordTupleT.y) && (coordTupleT.y <= Coord.y))) ? true : false;
    }


    //$p 共通
    // 
    public abstract void DispCoordFromField(CharaScript clickedChara_sc, int markKind, int hand_len);

    public abstract void DelMoveColor();

    // 
    public virtual void DispMovableCoord(CharaScript clickedChara_sc) {}

    public virtual void DispMovableCoord(List<List<CharaScript>> onFieldChara2List, 
    CharaScript clickedChara_sc, int markKind, int hand_len) {}


    //$p 非共通
    public virtual void InitFieldDarkness() {}

    public virtual void DispTheCoord(Vector3 theCoord) {}

    public virtual void ChangeDefaultFieldColor(List<FieldScript> fieldList) {}
    public virtual void BackMovedFieldColor() {}

    public virtual void DispCoordFromHand(CharaScript clickedChara_sc, int markKind) {}




    /*
    //$c 駒特有の行けない位置を除外
    public int ExcPos(List<List<CharaScript>> onFieldChara2List, CharaScript clickedChara_sc) {
        int rmPos = 0;

        //$b 歩兵なら
        if (clickedChara_sc.charaId == 1) {
            try {
                rmPos = onFieldChara2List[clickedChara_sc.p_num].Find(
                    charaScript => charaScript.p_num == clickedChara_sc.p_num & 
                    charaScript.charaId == clickedChara_sc.charaId).coordTuple[1];
            }
            catch (System.NullReferenceException) {;}
        }
        return rmPos;
    }
    */
}