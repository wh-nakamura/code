using UnityEngine;


//$p 定数
// レイヤー
public class Layer {
    public const float background = -0.1f;
    public const float field = -0.2f;
    public const float piece = -0.3f;
    public const float mist = -0.4f;
    public const float mark = -0.5f;
    public const float UI = -0.6f;
};

public class ColorConst
{
    public Color gray = new Color(125, 125, 125, 255);
    public Color white = new Color(255, 255, 255, 255);
}

public class SoundKind
{
    public const int hoverCharaSE = 1;
    public const int hoverFieldSE = 2;
    public const int otherSE = 3;
    public const int autoSE = 4;
    public const int positiveSE = 5;
    public const int negativeSE = 6;
    public const int PieceMoveSE = 7;
    public const int TurnChangeSE = 8;
    public const int VictorySE = 9;
    public const int DefeatSE = 10;
    public const int ExtinctionSE = 11;
    public const int CreateMistSE = 12;
}

public class BgmKind
{
    public const int beforeBattleBgm = 1;
    public const int basicBattleBgm = 2;
    public const int finalBattleBgm = 3;

}

public class FieldColor
{
    public const int basic = 1;
    public const int canMove = 2;
    public const int attackMove = 3;
    public const int noMove = 4;
    public const int beforeMove = 5;
}
public class Coord {
    public static int o = 1; // origin
    public static int x = 3;
    public static int y = 5;

    // 盤面の長さが奇数でも偶数でも霧生成の処理を変えないための定数
    public static int mistRow = (((Coord.y + (Coord.y % 2)) / 2) - 1); //** 3:1 4:1 5:2 6:2 7:3 8:3 9:4
}

public class Rule {
    public const int cost = 100;
    public const int handPieceMax = 2;
}

public class SkillKind
{
    public const string 消滅 = "消滅";
    public const string 臆病 = "臆病";

}

namespace a
{
    public class b {
        public const int c = 1;
    }
}

public enum CharaId
{
    ヌル,
    歩兵, 十字, 斜字, 王将, 
    銀将, 銀兵, 金将, 大将, 十王, 斜王, 
    低兵, 下兵, 弱兵, 暴君, 右兵, 左兵, 右子, 左子, 星兵, 星王, 銀王, 金王, 
    滑車, 良兵, Ｃ兵, Ｄ兵, Ｈ兵, Ｋ兵, Ｐ兵, Ｕ兵, Ｖ兵, 横王, 縦王, 両王, 
    Ａ兵, Ｉ兵, Ｊ兵, Ｒ兵, Ｔ兵, Ｙ兵, 賢者, 責兵, 地雷, 留王, 賢王, 責王, 
    弱飛, 弱角, 飛車, 角行, 攻兵, 羅将, 豪将, 覇将, 孔雀, 右強, 左強, 壁兵, 飛王, 角王, 
    仔魚, 稚魚, 幼魚, 若魚, 雑魚, 消歩, 消十, 消斜, 消銀, 消金, 消将, 消飛, 消角, Ｌ兵, 羅王, 豪王, 覇王, 攻王, 
    払歩, 払十, 払斜, 払銀, 払金, 払将, 払飛, 払角, 払Ｌ, 払滅, 臆払, 払王, 羅払, 豪払, 覇払, 
    霧歩, 霧十, 霧斜, 霧銀, 霧金, 霧将, 霧飛, 霧角, 霧Ｌ, 霧滅, 臆霧, 霧王, 羅霧, 豪霧, 覇霧, 
    

    ////歩兵, 十字, 斜字, 王将, 銀将,
    ////金将, 右兵, 左兵, 大将, 弱兵,
    ////暴君, 縦王, Ａ兵, Ｃ兵, Ｄ兵, 
    ////Ｈ兵, Ｉ兵, Ｊ兵, Ｋ兵, Ｌ兵, 
    ////Ｐ兵, Ｒ兵, Ｔ兵, Ｕ兵, Ｖ兵, 
    ////Ｙ兵, 滑車, 良兵, 地雷, 壁兵, 
    ////留王, 星兵, 星王, 十王, 斜王, 
    ////銀王, 金王, 責王, 飛車, 角行,
    ////羅王, 右子, 左子, 低兵, 下兵,
    ////弱飛, 弱角, 責王, 攻兵, 攻王,
    ////賢者, 賢王, 孔雀, 消固, 消歩,
    ////消十, 消斜, 消銀, 消金, 消将,
    ////消飛, 消角, 霧歩, 霧十, 霧斜, 
    ////霧銀, 霧金, 霧将, 霧飛, 霧角, 
    ////霧王, 銀兵, 横王, 右強, 左強, 
    ////両王, 霧滅, 羅将, 羅霧, 霧Ｌ,
    ////飛王, 角王, 

    TotalCharaNum, 
    金成, 両兵, 聖者, 竜王, 竜馬, 
};

public class Mist {
    public const int noMist = 0;
    public const int allyMist = 1;
    public const int oppMist = 2;

    public const int none = 0;
    public const int haze = 1;
    public const int mist = 2;
    public const int fog = 3;
}

public class Because {
    public const int none = 0;
    public const int takeKing = 1;
    public const int arrivalKing = 2;
    public const int surrender = 3;
    public const int timeOut = 4;
    public const int disconnect = 5;
    public const int turnMax = 6;
}

public class Phase {
    public const int whileGame = 1;
    public const int resultGame = 2;
    public const int afterResult = 3;
}


//$p その他
////public const int P_NUMS = 2;

