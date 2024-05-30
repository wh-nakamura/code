using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrgFieldObj: FieldParent
{

    public override void Awake()
    {
        base.Awake();
    }

    
    public override void InitFieldDarkness()
    {
        // フィールドの暗がりの作成

        //$b 相手フィールドの暗がり
        for (int y = Coord.o; y <= Coord.y; y++)
        {
            for (int x = Coord.o; x <= Coord.x; x++)
            {
                //$b 自陣以外の座標
                if (Coord.mistRow < y)
                {
                    fieldData[y, x].status = FieldColor.noMove;
                }
            }
        }
    }

    public override void DispMovableCoord(CharaScript clickedChara_sc)
    {
        // 移動範囲の表示

        DelMoveColor();
        DispCoordFromField(clickedChara_sc, FieldColor.canMove, 0);
    }

    public override void DispTheCoord(Vector3 theCoord)
    {
        // 指定した座標の色を変える

        DelMoveColor();

        // 指定座標の色変え
        int x = (int)theCoord.x; int y = (int)theCoord.y;
        movableFieldList.Add(fieldData[y, x]);
        fieldData[y, x].status = FieldColor.beforeMove;
    }

    public override void DispCoordFromField(
    CharaScript clickedChara_sc, int markKind, int hand_len)
    {
        // 盤上から動かすときの座標表示

        for (int i = 0; i < clickedChara_sc.posValueList.Count; i++) {

            int pos_x = (int)clickedChara_sc.obj.transform.position.x + 
                clickedChara_sc.posValueList[i].x;
            int pos_y = (int)clickedChara_sc.obj.transform.position.y + 
                clickedChara_sc.posValueList[i].y;

            //$b 盤外の除外
            if (pos_x < Coord.o || Coord.x < pos_x || 
            pos_y < Coord.o || Coord.y < pos_y) {continue;}

            //$b 自キャラの除外
            if (charaData[pos_y, pos_x] != null)
            {
                if (charaData[pos_y, pos_x].p_num == clickedChara_sc.p_num)
                {
                    continue;
                }
            }

            //$b 移動可能座標表示時にその座標に相手の霧がかかっているなら
            if (markKind == FieldColor.canMove & 
            fieldData[pos_y, pos_x].mistStatus == Mist.oppMist) {continue;}
            
            CreateCoordMark(pos_x, pos_y, markKind);
        }
    }

    //$p 移動先を表す色を元に戻す&灰色もある
    public override void DelMoveColor() {

        foreach (FieldScript field in movableFieldList) {
            field.status = (Coord.mistRow < //$t 要調整
                field.transform.position.y) ? FieldColor.noMove : FieldColor.basic;
        }
        movableFieldList = new List<FieldScript>();
        foreach (var item in movableFieldMarkList) {Destroy(item);}
        movableFieldMarkList = new List<GameObject>();
    }
}