using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using static CharaId;

public class CharaData : MonoBehaviour
{
    public struct charaData
    {
        public string charaName;
        public CharaId promoteId;

        public bool isKing;
        public int rank;
        public int[] posValueArray;

        public List<string> skillList;
    }

    // 左か右なら右に動けるほうが、手駒の渋滞を防げるため優秀
    // だが、左だけなら相手の手駒時に有利
    //public static Dictionary<(int x, int y), float> moveableCostDict = new Dictionary<(int x, int y), float>()
    //{
    //            {(-2,  2), 8.0f },             {(0,  2), 12.5f},        {(2,  2), 7.5f },
    //                        {(-1,  1), 3.0f }, {(0,  1), 3.5f }, {(1,  1), 2.5f},
    //    {(-2,  0), 3.0f },  {(-1,  0), 2.0f },                   {(1,  0), 1.5f },  {(2,  0), 2.5f },
    //                        {(-1, -1), 1.0f }, {(0, -1), 1.0f }, {(1, -1), 0.5f },
    //            {(-2, -2), 1.0f },             {(0, -2), 1.0f },        {(2, -2), 0.5f },
    //};

    public static Dictionary<(int x, int y), float> moveableCostDict = new Dictionary<(int x, int y), float>()
    {
                {(-2,  2), 0},             {(0,  2), 0},            {(2,  2), 0},
                            {(-1,  1), 0}, {(0,  1), 0}, {(1,  1), 0},
        {(-2,  0), 0},      {(-1,  0), 0}, {(0,  0), 0}, {(1,  0), 0},     {(2,  0), 0},
                            {(-1, -1), 0}, {(0, -1), 0}, {(1, -1), 0},
                {(-2, -2), 0},             {(0, -2), 0},            {(2, -2), 0},
    };

    // ニマス進み
    // 通常、跳躍可能、２のみ

    public static Dictionary<(int x, int y), float> promoteMoveableCostDict = new Dictionary<(int x, int y), float>()
    {
                {(-2,  2), 5.5f },             {(0,  2), 7.0f },        {(2,  2), 5.0f },
                            {(-1,  1), 2.5f }, {(0,  1), 2.0f }, {(1,  1), 2.0f },
        {(-2,  0), 5.0f },  {(-1,  0), 2.5f },                   {(1,  0), 2.0f },  {(2,  0), 4.0f },
                            {(-1, -1), 2.5f }, {(0, -1), 1.0f }, {(1, -1), 2.0f },
                {(-2, -2), 7.5f },             {(0, -2), 8.5f },        {(2, -2), 7.0f },

        ////{(-1,  1), 2.5f}, {(0,  1), 2.0f}, {(1,  1), 2.0f},
        ////{(-1,  0), 2.5f}, {(0,  0), 0.0f}, {(1,  0), 2.0f},
        ////{(-1, -1), 2.5f}, {(0, -1), 2.0f}, {(1, -1), 2.0f},
    };

    public static void InitMoveableCost()
    {
        // 値の初期化
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                moveableCostDict[(x, y)] = 0;
                promoteMoveableCostDict[(x, y)] = 0;
            }
        }

        for (int i = -1; i <= 1; i++)
        {
            // 1マス
            moveableCostDict[(i, 1)]  += 2.0f; // 前方
            moveableCostDict[(i, 0)]  += 1.0f; // 左右
            moveableCostDict[(i, -1)] += 0.0f; // 後方

            moveableCostDict[(-1, i)] += 1.0f; // 左側
            moveableCostDict[(0, i)]  += 1.0f; // 中央
            moveableCostDict[(1, i)]  += 0.5f; // 右側

            // 2マス
            moveableCostDict[(2*i, 2)]  += 7.0f; // 前方
            moveableCostDict[(2*i, 0)]  += 1.0f; // 左右
            moveableCostDict[(2*i, -2)] += 0.0f; // 後方

            moveableCostDict[(-2, 2*i)] += 1.0f; // 左側
            moveableCostDict[(0, 2*i)]  += 1.0f; // 中央
            moveableCostDict[(2, 2*i)]  += 0.5f; // 右側

            //$p 成
            // 1マス
            promoteMoveableCostDict[(i, 1)]  += 2.0f; // 前方
            promoteMoveableCostDict[(i, 0)]  += 1.0f; // 左右
            promoteMoveableCostDict[(i, -1)] += 0.0f; // 後方

            promoteMoveableCostDict[(-1, i)] += 1.0f; // 左側
            promoteMoveableCostDict[(0, i)]  += 1.0f; // 中央
            promoteMoveableCostDict[(1, i)]  += 0.5f; // 右側

            // 2マス
            promoteMoveableCostDict[(2*i, 2)]  += 7.0f; // 前方
            promoteMoveableCostDict[(2*i, 0)]  += 1.0f; // 左右
            promoteMoveableCostDict[(2*i, -2)] += 0.0f; // 後方

            promoteMoveableCostDict[(-2, 2*i)] += 1.0f; // 左側
            promoteMoveableCostDict[(0, 2*i)]  += 1.0f; // 中央
            promoteMoveableCostDict[(2, 2*i)]  += 0.5f; // 右側
        
            /**
            //$p 成
            // 1マス
            promoteMoveableCostDict[(i, 1)]  += 3.0f; // 前方
            promoteMoveableCostDict[(i, 0)]  += 0.5f; // 左右
            promoteMoveableCostDict[(i, -1)] += 0.5f; // 後方

            promoteMoveableCostDict[(-1, i)] += 0.5f; // 左側
            promoteMoveableCostDict[(0, i)]  += 1.0f; // 中央
            promoteMoveableCostDict[(1, i)]  += 1.0f; // 右側

            // 2マス
            promoteMoveableCostDict[(2*i, 2)]  += 2.5f; // 前方
            promoteMoveableCostDict[(2*i, 0)]  += 1.0f; // 左右
            promoteMoveableCostDict[(2*i, -2)] += 1.0f; // 後方

            promoteMoveableCostDict[(-2, 2*i)] += 0.5f; // 左側
            promoteMoveableCostDict[(0, 2*i)]  += 1.0f; // 中央
            promoteMoveableCostDict[(2, 2*i)]  += 1.0f; // 右側
            */
        }
        // 正面のコストのみ更に追加
        moveableCostDict[(0, 1)] += 0.5f;
        moveableCostDict[(0, 2)] += 4.5f;
        promoteMoveableCostDict[(0, 1)] += 0.5f;
        promoteMoveableCostDict[(0, 2)] += 4.5f;
    }

    public static Dictionary<CharaId, charaData> charaDataDict = new Dictionary<CharaId, charaData>
    {
        {ヌル, new charaData() {charaName="ヌル", promoteId=ヌル, isKing=false, posValueArray=new int[] {}}},
        // ~0
        {歩兵, new charaData() {charaName="歩兵", promoteId=金成, isKing=false, rank=1, posValueArray=new int[] {2}}},
        {十字, new charaData() {charaName="十字", promoteId=ヌル, isKing=false, rank=1, posValueArray=new int[] {0}}},
        {斜字, new charaData() {charaName="斜字", promoteId=ヌル, isKing=false, rank=1, posValueArray=new int[] {1}}},
        {王将, new charaData() {charaName="王将", promoteId=ヌル, isKing=true , rank=1, posValueArray=new int[] {0, 1}}},


        {銀将, new charaData() {charaName="銀将", promoteId=金成, isKing=false, rank=2, posValueArray=new int[] {1, 2}}},
        {銀兵, new charaData() {charaName="銀兵", promoteId=ヌル, isKing=false, rank=2, posValueArray=new int[] {1, 2}}},
        {金将, new charaData() {charaName="金将", promoteId=ヌル, isKing=false, rank=2, posValueArray=new int[] {0, 3, 9}}},
        {大将, new charaData() {charaName="大将", promoteId=ヌル, isKing=false, rank=2, posValueArray=new int[] {0, 1}}},
        {十王, new charaData() {charaName="十王", promoteId=ヌル, isKing=true , rank=2, posValueArray=new int[] {0}}},
        {斜王, new charaData() {charaName="斜王", promoteId=ヌル, isKing=true , rank=2, posValueArray=new int[] {1}}},


        {低兵, new charaData() {charaName="低兵", promoteId=ヌル, isKing=false, rank=3, posValueArray=new int[] {1, 6}}},
        {下兵, new charaData() {charaName="下兵", promoteId=ヌル, isKing=false, rank=3, posValueArray=new int[] {2, 5, 6, 7}}},
        {弱兵, new charaData() {charaName="弱兵", promoteId=ヌル, isKing=false, rank=3, posValueArray=new int[] {0, -6}}},
        {暴君, new charaData() {charaName="暴君", promoteId=聖者, isKing=false, rank=3, posValueArray=new int[] {2, 3, 9}}},
        {右兵, new charaData() {charaName="右兵", promoteId=ヌル, isKing=false, rank=3, posValueArray=new int[] {2, 3, 4, 5}}},
        {左兵, new charaData() {charaName="左兵", promoteId=ヌル, isKing=false, rank=3, posValueArray=new int[] {2, 7, 8, 9}}},

        {右子, new charaData() {charaName="右子", promoteId=両兵, isKing=false, rank=3, posValueArray=new int[] {2, 3}}},
        {左子, new charaData() {charaName="左子", promoteId=両兵, isKing=false, rank=3, posValueArray=new int[] {2, 9}}},
        {星兵, new charaData() {charaName="星兵", promoteId=ヌル, isKing=false, rank=3, posValueArray=new int[] {0, 5, 7, -6}}},
        {星王, new charaData() {charaName="星王", promoteId=ヌル, isKing=true , rank=3, posValueArray=new int[] {0, 5, 7, -6}}},
        {銀王, new charaData() {charaName="銀王", promoteId=ヌル, isKing=true , rank=3, posValueArray=new int[] {1, 2}}},
        {金王, new charaData() {charaName="金王", promoteId=ヌル, isKing=true , rank=3, posValueArray=new int[] {0, 3, 9}}},


        {滑車, new charaData() {charaName="滑車", promoteId=ヌル, isKing=false, rank=4, posValueArray=new int[] {2, 6}}},
        {良兵, new charaData() {charaName="良兵", promoteId=ヌル, isKing=false, rank=4, posValueArray=new int[] {2, 3, 5, 8}}},
        {Ｃ兵, new charaData() {charaName="Ｃ兵", promoteId=ヌル, isKing=false, rank=4, posValueArray=new int[] {0, 1, -4}}},
        {Ｄ兵, new charaData() {charaName="Ｄ兵", promoteId=ヌル, isKing=false, rank=4, posValueArray=new int[] {0, 7, 9}}},
        {Ｈ兵, new charaData() {charaName="Ｈ兵", promoteId=ヌル, isKing=false, rank=4, posValueArray=new int[] {1, 4, 8}}},
        {Ｋ兵, new charaData() {charaName="Ｋ兵", promoteId=ヌル, isKing=false, rank=4, posValueArray=new int[] {1, 8}}},

        {Ｐ兵, new charaData() {charaName="Ｐ兵", promoteId=ヌル, isKing=false, rank=4, posValueArray=new int[] {1, 2, 8, -5}}},
        {Ｕ兵, new charaData() {charaName="Ｕ兵", promoteId=ヌル, isKing=false, rank=4, posValueArray=new int[] {0, 1, -2}}},
        {Ｖ兵, new charaData() {charaName="Ｖ兵", promoteId=ヌル, isKing=false, rank=4, posValueArray=new int[] {0, 3, 9, -2}}},
        {横王, new charaData() {charaName="横王", promoteId=ヌル, isKing=true , rank=4, posValueArray=new int[] {4, 8}}},
        {縦王, new charaData() {charaName="縦王", promoteId=ヌル, isKing=true , rank=4, posValueArray=new int[] {2, 6}}},
        {両王, new charaData() {charaName="両王", promoteId=ヌル, isKing=true , rank=4, posValueArray=new int[] {0, 1, -6}}},


        {Ａ兵, new charaData() {charaName="Ａ兵", promoteId=ヌル, isKing=false, rank=5, posValueArray=new int[] {2, 5, 7}}},
        {Ｉ兵, new charaData() {charaName="Ｉ兵", promoteId=ヌル, isKing=false, rank=5, posValueArray=new int[] {1, 2, 6}}},
        {Ｊ兵, new charaData() {charaName="Ｊ兵", promoteId=ヌル, isKing=false, rank=5, posValueArray=new int[] {0, 1, -2, -9}}},
        {Ｒ兵, new charaData() {charaName="Ｒ兵", promoteId=ヌル, isKing=false, rank=5, posValueArray=new int[] {1, 2, 8}}},
        {Ｔ兵, new charaData() {charaName="Ｔ兵", promoteId=ヌル, isKing=false, rank=5, posValueArray=new int[] {2, 3, 6, 9}}},
        {Ｙ兵, new charaData() {charaName="Ｙ兵", promoteId=ヌル, isKing=false, rank=5, posValueArray=new int[] {3, 6, 9}}},

        {賢者, new charaData() {charaName="賢者", promoteId=ヌル, isKing=false, rank=5, posValueArray=new int[] {10, 11, -12, -13, -19}}},
        {責兵, new charaData() {charaName="責兵", promoteId=ヌル, isKing=false, rank=5, posValueArray=new int[] {12}}},
        {地雷, new charaData() {charaName="地雷", promoteId=ヌル, isKing=false, rank=5, posValueArray=new int[] {}}},
        {留王, new charaData() {charaName="留王", promoteId=ヌル, isKing=true , rank=5, posValueArray=new int[] {}}},
        {賢王, new charaData() {charaName="賢王", promoteId=ヌル, isKing=true , rank=5, posValueArray=new int[] {10, 11, -12, -13, -19}}},
        {責王, new charaData() {charaName="責王", promoteId=ヌル, isKing=true , rank=5, posValueArray=new int[] {12}}},


        {弱飛, new charaData() {charaName="弱飛", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {10}}},
        {弱角, new charaData() {charaName="弱角", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {11}}},
        {飛車, new charaData() {charaName="飛車", promoteId=竜王, isKing=false, rank=6, posValueArray=new int[] {10}}},
        {角行, new charaData() {charaName="角行", promoteId=竜馬, isKing=false, rank=6, posValueArray=new int[] {11}}},
        {攻兵, new charaData() {charaName="攻兵", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {12, 13, 19}}},
        {羅将, new charaData() {charaName="羅将", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {1, 10}}},
        
        {豪将, new charaData() {charaName="豪将", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {0, 11}}},
        {覇将, new charaData() {charaName="覇将", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {10, 11}}},
        
        {孔雀, new charaData() {charaName="孔雀", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {0, 5, 7, 12, -2}}},
        {右強, new charaData() {charaName="右強", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {12, 13, 14, 15}}},
        {左強, new charaData() {charaName="左強", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {12, 17, 18, 19}}},
        {壁兵, new charaData() {charaName="壁兵", promoteId=ヌル, isKing=false, rank=6, posValueArray=new int[] {0, 5, 7, -2}}},
        {飛王, new charaData() {charaName="飛王", promoteId=ヌル, isKing=true , rank=6, posValueArray=new int[] {10}}},
        {角王, new charaData() {charaName="角王", promoteId=ヌル, isKing=true , rank=6, posValueArray=new int[] {11}}},


        {仔魚, new charaData() {charaName="仔魚", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {}, skillList=new List<string> {"消滅"}}},
        {稚魚, new charaData() {charaName="稚魚", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0, 5, 7, -2}, skillList=new List<string> {"臆病", "消滅"}}},
        {幼魚, new charaData() {charaName="幼魚", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {2}, skillList=new List<string> {"臆病", "消滅"}}},
        {若魚, new charaData() {charaName="若魚", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {2, 3, 9}, skillList=new List<string> {"臆病", "消滅"}}},
        {雑魚, new charaData() {charaName="雑魚", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0, 1}, skillList=new List<string> {"臆病", "消滅"}}},
        {消歩, new charaData() {charaName="消歩", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {2}, skillList=new List<string> {"消滅"}}},
        {消十, new charaData() {charaName="消十", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0}, skillList=new List<string> {"消滅"}}},
        {消斜, new charaData() {charaName="消斜", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {1}, skillList=new List<string> {"消滅"}}},
        {消銀, new charaData() {charaName="消銀", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {1, 2}, skillList=new List<string> {"消滅"}}},
        {消金, new charaData() {charaName="消金", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0, 3, 9}, skillList=new List<string> {"消滅"}}},
        
        {消将, new charaData() {charaName="消将", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0, 1}, skillList=new List<string> {"消滅"}}},
        {消飛, new charaData() {charaName="消飛", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {10}, skillList=new List<string> {"消滅"}}},
        {消角, new charaData() {charaName="消角", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {11}, skillList=new List<string> {"消滅"}}},
        {Ｌ兵, new charaData() {charaName="Ｌ兵", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {1, 6, 8, -3}}},
        {羅王, new charaData() {charaName="羅王", promoteId=ヌル, isKing=true , rank=7, posValueArray=new int[] {1, 10}}},
        {豪王, new charaData() {charaName="豪王", promoteId=ヌル, isKing=true , rank=7, posValueArray=new int[] {0, 11}}},
        {覇王, new charaData() {charaName="覇王", promoteId=ヌル, isKing=true , rank=7, posValueArray=new int[] {10, 11}}},
        {攻王, new charaData() {charaName="攻王", promoteId=ヌル, isKing=true , rank=7, posValueArray=new int[] {12, 13, 19}}},

        {払歩, new charaData() {charaName="払歩", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {2}, skillList=new List<string> {"霧払い"}}},
        {払十, new charaData() {charaName="払十", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0}, skillList=new List<string> {"霧払い"}}},
        {払斜, new charaData() {charaName="払斜", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {1}, skillList=new List<string> {"霧払い"}}},
        {払銀, new charaData() {charaName="払銀", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {1, 2}, skillList=new List<string> {"霧払い"}}},
        {払金, new charaData() {charaName="払金", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0, 3, 9}, skillList=new List<string> {"霧払い"}}},
        {払将, new charaData() {charaName="払将", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0, 1}, skillList=new List<string> {"霧払い"}}},

        {払飛, new charaData() {charaName="払飛", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {10}, skillList=new List<string> {"霧払い"}}},
        {払角, new charaData() {charaName="払角", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {11}, skillList=new List<string> {"霧払い"}}},
        {払Ｌ, new charaData() {charaName="払Ｌ", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {1, 6, 8, -3}, skillList=new List<string> {"霧払い"}}},
        {払滅, new charaData() {charaName="払滅", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {0}, skillList=new List<string> {"消滅", "霧払い"}}},
        {臆払, new charaData() {charaName="臆払", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0}, skillList=new List<string> {"臆病", "霧払い"}}},
        {払王, new charaData() {charaName="払王", promoteId=ヌル, isKing=true , rank=7, posValueArray=new int[] {0, 1}, skillList=new List<string> {"霧払い"}}},
        {羅払, new charaData() {charaName="羅払", promoteId=ヌル, isKing=true , rank=7, posValueArray=new int[] {1, 10}, skillList=new List<string> {"霧払い"}}},
        {豪払, new charaData() {charaName="豪払", promoteId=ヌル, isKing=true , rank=7, posValueArray=new int[] {0, 11}, skillList=new List<string> {"霧払い"}}},
        {覇払, new charaData() {charaName="覇払", promoteId=ヌル, isKing=true , rank=7, posValueArray=new int[] {10, 11}, skillList=new List<string> {"霧払い"}}},


        {霧歩, new charaData() {charaName="霧歩", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {2}, skillList=new List<string> {"霧生成(自)"}}},
        {霧十, new charaData() {charaName="霧十", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {0}, skillList=new List<string> {"霧生成(自)"}}},
        {霧斜, new charaData() {charaName="霧斜", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {1}, skillList=new List<string> {"霧生成(自)"}}},
        {霧銀, new charaData() {charaName="霧銀", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {1, 2}, skillList=new List<string> {"霧生成(自)"}}},
        {霧金, new charaData() {charaName="霧金", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {0, 3, 9}, skillList=new List<string> {"霧生成(自)"}}},
        {霧将, new charaData() {charaName="霧将", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {0, 1}, skillList=new List<string> {"霧生成(自)"}}},

        {霧飛, new charaData() {charaName="霧飛", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {10}, skillList=new List<string> {"霧生成(自)"}}},
        {霧角, new charaData() {charaName="霧角", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {11}, skillList=new List<string> {"霧生成(自)"}}},
        {霧Ｌ, new charaData() {charaName="霧Ｌ", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {1, 6, 8, -3}, skillList=new List<string> {"霧生成(自)"}}},
        {霧滅, new charaData() {charaName="霧滅", promoteId=ヌル, isKing=false, rank=8, posValueArray=new int[] {0}, skillList=new List<string> {"消滅", "霧生成(自)"}}},
        {臆霧, new charaData() {charaName="臆霧", promoteId=ヌル, isKing=false, rank=7, posValueArray=new int[] {0}, skillList=new List<string> {"臆病", "霧生成(自)"}}},
        {霧王, new charaData() {charaName="霧王", promoteId=ヌル, isKing=true , rank=8, posValueArray=new int[] {0, 1}, skillList=new List<string> {"霧生成(自)"}}},
        {羅霧, new charaData() {charaName="羅霧", promoteId=ヌル, isKing=true , rank=8, posValueArray=new int[] {1, 10}, skillList=new List<string> {"霧生成(自)"}}},
        {豪霧, new charaData() {charaName="豪霧", promoteId=ヌル, isKing=true , rank=8, posValueArray=new int[] {0, 11}, skillList=new List<string> {"霧生成(自)"}}},
        {覇霧, new charaData() {charaName="覇霧", promoteId=ヌル, isKing=true , rank=8, posValueArray=new int[] {10, 11}, skillList=new List<string> {"霧生成(自)"}}},

        // ~80
    };

    public static Dictionary<CharaId, charaData> promoteCharaDataDict = new Dictionary<CharaId, charaData>
    {
        {ヌル, new charaData() {charaName="ヌル", promoteId=ヌル, isKing=false, posValueArray=new int[] {}}},
        // ~0
        {金成, new charaData() {charaName="金成", posValueArray=new int[] {0, 3, 9}}},
        {両兵, new charaData() {charaName="両兵", posValueArray=new int[] {0, 1, -6}}},
        {聖者, new charaData() {charaName="聖者", posValueArray=new int[] {5, 6, 7}}},
        {竜王, new charaData() {charaName="竜王", posValueArray=new int[] {1, 10}}},
        {竜馬, new charaData() {charaName="竜馬", posValueArray=new int[] {0, 11}}},

        // ~5
    };

    public static string GetCharaName(CharaId charaId)
    {
        try {return charaDataDict[charaId].charaName;}
        catch (KeyNotFoundException) 
            {return promoteCharaDataDict[charaId].charaName;}
    }

    public static void SetCharaInfo(CharaScript charaScript)
    {
        charaData charaData;
        try {charaData = charaDataDict[charaScript.charaId];}
        catch (KeyNotFoundException) 
            {charaData = promoteCharaDataDict[charaScript.charaId];}
        
        charaScript.charaName = charaData.charaName;
        charaScript.promoteId = charaData.promoteId;
        charaScript.isKing = charaData.isKing;
        charaScript.rank = charaData.rank;
        charaScript.posValueList = place_range(charaScript, charaData.posValueArray);
        if (charaData.skillList != null) {charaScript.skillList = charaData.skillList;}
        charaScript.cost = CostCalc(charaScript);
    }


    public static List<(int x, int y)> place_range(CharaScript charaScript, params int[] dir_list)
    {
        List<(int x, int y)> move_list = new List<(int, int)> {};
        int p = (charaScript.mine) ? 1 : -1;
        
        foreach (int direction in dir_list)
        {
            switch (direction)
            {
                case 0: // 十字
                    AddMove((0, 1*p), (1*p, 0), (0, -1*p), (-1*p, 0)); break;
                case 1: // 斜め
                    AddMove((1*p, 1*p), (1*p, -1*p), (-1*p, -1*p), (-1*p, 1*p)); break;
                case 2: AddMove((0, 1*p)); break;   // 北
                case 3: AddMove((1*p, 1*p)); break;   // 北東
                case 4: AddMove((1*p, 0)); break;   // 東
                case 5: AddMove((1*p, -1*p)); break;  // 南東
                case 6: AddMove((0, -1*p)); break;  // 南
                case 7: AddMove((-1*p, -1*p)); break; // 南西
                case 8: AddMove((-1*p, 0)); break;  // 西
                case 9: AddMove((-1*p, 1*p)); break;  // 北西

                case 10: AddMove((0, 1*p), (1*p, 0), (0, -1*p), (-1*p, 0),
                    (0, 2*p), (2*p, 0), (0, -2*p), (-2*p, 0)); break;
                case 11: AddMove((1*p, 1*p), (1*p, -1*p), (-1*p, -1*p), (-1*p, 1*p),
                    (2*p, 2*p), (2*p, -2*p), (-2*p, -2*p), (-2*p, 2*p)); break;
                case 12: AddMove((0, 1*p), (0, 2*p)); break;   // 北
                case 13: AddMove((1*p, 1*p), (2*p, 2*p)); break;   // 北東
                case 14: AddMove((1*p, 0), (2*p, 0)); break;   // 東
                case 15: AddMove((1*p, -1*p), (2*p, -2*p)); break;  // 南東
                case 16: AddMove((0, -1*p), (0, -2*p)); break;  // 南
                case 17: AddMove((-1*p, -1*p), (-2*p, -2*p)); break; // 南西
                case 18: AddMove((-1*p, 0), (-2*p, 0)); break;  // 西
                case 19: AddMove((-1*p, 1*p), (-2*p, 2*p)); break;  // 北西

                case -2: move_list.Remove((0, 1*p)); break;   // 北
                case -3: move_list.Remove((1*p, 1*p)); break;   // 北東
                case -4: move_list.Remove((1*p, 0)); break;   // 東
                case -5: move_list.Remove((1*p, -1*p)); break;  // 南東
                case -6: move_list.Remove((0, -1*p)); break;  // 南
                case -7: move_list.Remove((-1*p, -1*p)); break; // 南西
                case -8: move_list.Remove((-1*p, 0)); break;  // 西
                case -9: move_list.Remove((-1*p, 1*p)); break;  // 北西

                case -12: move_list.Remove((0, 2*p)); break;   // 北
                case -13: move_list.Remove((2*p, 2*p)); break;   // 北東
                case -14: move_list.Remove((2*p, 0)); break;   // 東
                case -15: move_list.Remove((2*p, -2*p)); break;  // 南東
                case -16: move_list.Remove((0, -2*p)); break;  // 南
                case -17: move_list.Remove((-2*p, -2*p)); break; // 南西
                case -18: move_list.Remove((-2*p, 0)); break;  // 西
                case -19: move_list.Remove((-2*p, 2*p)); break;  // 北西
            }
        }
        return move_list;

        void AddMove(params (int, int)[] moveTupleList)
        {
            foreach ((int, int) moveTuple in moveTupleList)
            {
                move_list.Add(moveTuple);
            }
        }
    }

    public static float CostCalc(CharaScript charaScript, List<(int x, int y)> promotePosValueList = null)
    {
        //$b 成った駒かどうか
        //if (charaScript.promoteState == true) {bool isPromote = true;}

        float maxCost = OrgRuleTextScript.maxCost;
        float defaultCost = 10;
        float kingCost = 5;
        //float noPutCost = (maxCost / 2) - ((defaultCost + kingCost) / 2 + defaultCost);
        float noPutCost = (maxCost / 4);
        float cost = defaultCost;

        bool forwardFlag = false;
        bool promoteFlag = (promotePosValueList != null) ? true : false;
        int p = (charaScript.mine) ? 1 : -1;

        List<(int x, int y)> posValueList = (promoteFlag) ? 
            promotePosValueList : charaScript.posValueList;

        foreach ((int x, int y) moveT in posValueList)
        {
            (int x, int y) move = (moveT.x * p, moveT.y * p);
            cost += (promoteFlag) ? 
                promoteMoveableCostDict[move] : moveableCostDict[move];

            if      (move == (0, 1)) // 北
            {
                forwardFlag = true;
            }
            else if (move == (1, 1)) // 北東
            {
                forwardFlag = true;
            }
            else if (move == (1, 0)) // 東
            {}
            else if (move == (1, -1)) // 南東
            {}
            else if (move == (0, -1)) // 南
            {}
            else if (move == (-1, -1)) // 南西
            {}
            else if (move == (-1, 0)) // 西
            {}
            else if (move == (-1, 1)) // 北西
            {
                //$b この移動先以外に前方に移動しない場合
                if (forwardFlag == false && charaScript.isKing == false)
                {
                    cost += noPutCost;
                }

                forwardFlag = true;
            }

            else if (move == (0, 2)) // 北
            {
                ////if (charaScript.isKing) {cost += kingCost*1.5f;}
            }
            else if (move == (2, 2)) // 北東
            {
                ////if (charaScript.isKing) {cost += kingCost;}
            }
            else if (move == (-2, 2)) // 北西
            {
                ////if (charaScript.isKing) {cost += kingCost;}
            }
        }

        //$b 既成駒の場合
        if (promoteFlag)
        {
            //
        }
        //$b 成っていない駒の場合
        else
        {
            //$b 前方系の移動範囲を持たない場合
            if (forwardFlag == false && charaScript.isKing == false)
            {
                //$b これらの能力を持っていない場合
                if (!charaScript.FindSkill("消滅"))
                {
                    cost += noPutCost;
                }
                //$b 消滅能力を持つ場合
                else if (charaScript.FindSkill("消滅"))
                {
                    cost -= kingCost * 3f;
                }
            }
        }

        //$b 能力を持つ場合
        cost = CostCalcSkill(charaScript, cost);

        //$b 王の場合
        if (charaScript.isKing)
        {
            cost -= kingCost;
            cost *= 1.5f;

            if (60 < cost)
            {
                cost *= 0.75f;
                cost += 15f;
            }
            cost = Func.Round(cost);
        }
        
        //$b 成る駒の場合
        if (charaScript.promoteId != 0 && !promoteFlag)
        {
            //$t 成り後に能力を持つ場合の追加の処理
            float promoteCost = CostCalc(charaScript, place_range(charaScript, 
                promoteCharaDataDict[charaScript.promoteId].posValueArray));

            /**
            List<(int x, int y)> promotePosValueList2 = new List<(int x, int y)>();
            List<(int x, int y)> promotePosValueList2T = place_range(charaScript, promoteCharaDataDict[charaScript.promoteId].posValueArray);

            foreach (List<(int x, int y)> pos in charaScript.posValueList)
            {
                if (promotePosValueList2T.Contains(pos))
                {
                    continue;
                }
                else {
                    promotePosValueList2.Add(pos);
                }
            }
            float promoteCost = CostCalc(charaScript, place_range(charaScript, promotePosValueList2));
            **/
            
            // 駒のコスト + ((成り前駒のコスト　ー　成り先駒のコスト) / 2)
            cost += Func.Round((promoteCost - cost) / 2);
        }

        return cost;


        float CostCalcSkill(CharaScript charaScript, float cost)
        {        
            foreach (string skillKind in charaScript.skillList)
            {
                switch (skillKind)
                {
                    case "臆病":
                        // コスト反比例ー固定値
                        cost = Func.Round((cost * 0.25f)) - kingCost/2;
                        if (!charaScript.FindSkill("消滅")) {cost += defaultCost;}
                        else if (charaScript.isKing) {cost += kingCost;}
                        break;
                    case "消滅":
                        // 13.5 + (13.5 - 20)/2 = 10.5
                        // 18 + (18 - 20)/2 = 17
                        // 2.5 + (2.5 - 20)/2 = -6
                        ////costT += cost - defaultCost*2; break;
                        // 元のコストに比例＋固定値
                        cost += Func.Round((cost - defaultCost*2) / 2) + (kingCost); break;
                    case "霧生成(自)":
                        // 移動範囲の量に比例＋固定値
                        cost += (posValueList.Count * kingCost/2) + (kingCost); break;
                    case "霧払い":
                        cost += (posValueList.Count * kingCost/2) + (kingCost/2); break;
                    default:
                        if (skillKind != "") {print(skillKind);}
                        break;
                }
            }
            return cost;
        }
    }
}


