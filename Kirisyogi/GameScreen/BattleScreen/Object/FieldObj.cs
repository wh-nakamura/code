using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Linq;

public class FieldObj: FieldParent
{
    public override void Awake() {
        base.Awake();
    }
    
    //$p 移動範囲の表示
    public override void DispMovableCoord(List<List<CharaScript>> onFieldChara2List, 
    CharaScript clickedChara_sc, int markKind, int hand_len) {

        //$b キャラを選択しているか
        if (clickedChara_sc) {
            Vector3 nowObjPos = clickedChara_sc.obj.transform.position;

            DelMoveColor();

            //$b 手札を選択したか
            if (nowObjPos.y < Coord.o || Coord.y < nowObjPos.y)
            {
                ////DispCoordFromHand(onFieldChara2List, clickedChara_sc, markKind);
                bool isKingOut = false;
                //$b 自分の王が自陣外にいるか
                for (int y = Coord.o; y <= Coord.y; y++)
                {
                    for (int x = Coord.o; x <= Coord.x; x++)
                    {
                        if (charaData[y, x] == null) {continue;}
                        if (charaData[y, x].isKing && charaData[y, x].mine && Coord.mistRow < charaData[y, x].coordTuple.y)
                        {
                            isKingOut = true;
                            y = Coord.y; break;
                        }
                    }
                }
                if (isKingOut)
                {
                    DispCoordFromHand(clickedChara_sc, markKind);
                }
                else
                {
                    DispCoordFromField(clickedChara_sc, markKind, hand_len);
                }
            }
            else {
                DispCoordFromField(clickedChara_sc, markKind, hand_len);
            }
        }
    }

    //$c 手札から動かすときの座標表示
    public override void DispCoordFromHand(CharaScript clickedChara_sc, int markKind)
    {
        for (int pos_y = Coord.o; pos_y <= Coord.y; pos_y++)
        {
            for (int pos_x = Coord.o; pos_x <= Coord.x; pos_x++)
            {
                //$b 盤外の除外
                if (pos_x < Coord.o || Coord.x < pos_x || pos_y < Coord.o || Coord.y < pos_y) {continue;}

                //$b キャラのいる座標の除外
                try {if (charaData[pos_y, pos_x]/*p_num <= 1*/) {continue;}} //$d 無意味処理
                catch (System.NullReferenceException) {/*座標除外1*/;}

                //$b 相手の霧がかかっている座標の除外
                if (fieldData[pos_y, pos_x].mistStatus == Mist.oppMist) {continue;}

                //$b 歩兵と同じ座標の除外
                //if (x == ExcPos(onFieldChara2List, clickedChara_sc)) {/*座標除外2*/; continue;}
                
                CreateCoordMark(pos_x, pos_y, markKind);
            }
        }
    }

    //$c 盤上から動かすときの座標表示
    public override void DispCoordFromField(CharaScript clickedChara_sc, int markKind, int hand_len)
    {
        for (int i = 0; i < clickedChara_sc.posValueList.Count; i++)
        {
            int pos_x = (int)clickedChara_sc.obj.transform.position.x + 
                clickedChara_sc.posValueList[i].x;
            int pos_y = (int)clickedChara_sc.obj.transform.position.y + 
                clickedChara_sc.posValueList[i].y;

            //$b 盤外の除外
            if (pos_x < Coord.o || Coord.x < pos_x || pos_y < Coord.o || Coord.y < pos_y) {continue;}

            //$b 自キャラの除外
            try {if (charaData[pos_y, pos_x].p_num == clickedChara_sc.p_num) {continue;}}
            catch (System.NullReferenceException) {;}

            //$b 手札に2体取っている場合
            if (hand_len == 2)
            {
                //$b 相手のキャラの除外
                try {if (charaData[pos_y, pos_x].p_num != clickedChara_sc.p_num) {continue;}}
                catch (System.NullReferenceException) {;}
            }

            //$b 移動可能座標表示時にその座標に相手の霧がかかっているなら
            if (markKind == FieldColor.canMove & 
            fieldData[pos_y, pos_x].mistStatus == Mist.oppMist) {continue;}

            //$b 2マス以上進む場合
            if (1 < Math.Abs(clickedChara_sc.posValueList[i].x) || 
            1 < Math.Abs(clickedChara_sc.posValueList[i].y))
            {
                int pos_xT = (int)clickedChara_sc.obj.transform.position.x + 
                    clickedChara_sc.posValueList[i].x/2;
                int pos_yT = (int)clickedChara_sc.obj.transform.position.y + 
                    clickedChara_sc.posValueList[i].y/2;
                
                //$b キャラの除外
                try {if (0 <= charaData[pos_yT, pos_xT].p_num) {continue;}}
                catch (System.NullReferenceException) {;}
            }

            //$b 能力がある場合
            if (SkillExec(pos_x, pos_y)) {continue;}
            
            CreateCoordMark(pos_x, pos_y, markKind);
        }

        //$b スキルによる移動先の除外
        bool SkillExec(int pos_x, int pos_y)
        {
            if (clickedChara_sc.FindSkill("臆病"))
            {
                //$b 相手のキャラの除外
                try {if (charaData[pos_y, pos_x].p_num != clickedChara_sc.p_num) {return true;}}
                catch (System.NullReferenceException) {;}

                //$b その座標に相手の霧がかかっているなら
                if ((fieldData[pos_y, pos_x].mistStatus == Mist.oppMist) || 
                !clickedChara_sc.mine && fieldData[pos_y, pos_x].mistStatus == Mist.allyMist) {return true;}
            }
            return false;
        }
    }

    //$p 移動先を表す色を元に戻す
    public override void DelMoveColor() {
        
        ChangeDefaultFieldColor(movableFieldList);
        foreach (GameObject item in movableFieldMarkList) {Destroy(item);}

        movableFieldList = new List<FieldScript>();
        movableFieldMarkList = new List<GameObject>();
    }

    public override void ChangeDefaultFieldColor(List<FieldScript> fieldList)
    {
        // 指定した盤面の色を元に戻す

        foreach (FieldScript field in fieldList)
        {
            //$b 移動前座標の色は変更しない
            if (movedFieldList.Find(fieldT => fieldT == field))
            {
                field.status = FieldColor.beforeMove;
            }
            else 
            {
                field.status = FieldColor.basic;
            }
        }
    }

    public override void BackMovedFieldColor()
    {
        // 移動前座標の盤面の色を元に戻す

        foreach (FieldScript field in movedFieldList)
        {
            field.status = FieldColor.basic;
        }
        movedFieldList = new List<FieldScript>();
    }

    
}