using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using System.Linq;
using static CharaId;

public class AddPieceListScript : MonoBehaviour
{
    public GameObject addPieceListObject;
    public GameObject scrollbar;
    public GameObject contents;

    public List<CharaScript> sortList;
    public FindPanelScript FindPanel;
    public int totalCharaNumber2 = 0;

    public static AddPieceListScript This = null;

    void Awake() {
        if (This == null) {
            This = this;
        }
    }

    void Start()
    {
        int totalCharaNumber = (int)TotalCharaNum - 1;

        addPieceListObject.SetActive(true);
        
        Vector2 contentsSize = contents.GetComponent<RectTransform>().sizeDelta;
        contentsSize.y = 10; // 隙間確保
        contentsSize.y += (totalCharaNumber / 3) * 80; // 基本枠
        contentsSize.y += ((totalCharaNumber % 3) != 0) ? 80 : 0; // 余り枠
        contents.GetComponent<RectTransform>().sizeDelta = contentsSize;

        float x = -60; float y = (contentsSize.y / 2) + 45; // -60 260 : 60 -80

        for (int i = 1; i <= totalCharaNumber; i++) {
        
            x = -60 + (60 * ((i - 1) % 3));
            y -= (((i - 1) % 3) == 0) ? 80 : 0;

            // キャラを作成
            GameObject charaContent = Instantiate((GameObject)Resources.Load(
                "Syogi/Chara/PieceTemplates/UIAddPieceTemplate"), new Vector3(x, y, Layer.UI), 
                Quaternion.identity) as GameObject;

            // Contentに移動
            charaContent.transform.SetParent(GameObject.Find(
                $"UICanvas/AddPieceList/Viewport/Contents").transform, false);

            // 情報セット
            OrgSetCharaInfo(charaContent, 0, 0, (CharaId)i, (0, 0));

            sortList.Add(charaContent.GetComponent<CharaScript>());
        }
        addPieceListObject.SetActive(false);
    }

    public void UpdateSqueezeMoveList()
    {
        //if (squeezeMoveList.find(""))
        //{
        //    squeezeMoveList.Add("ObjectName");
        //    obj1.active(true) // チェックマークを付ける
        //} else {
        //    remove()
        //    obj1.active(false) // チェックマークを消す
        //}
        
    }

    public bool FindPieceList(CharaScript charaScript, bool exclusionFlag)
    {
        ////bool falseFlag = (exclusionFlag) ? true : false;
        ////bool trueFlag = (exclusionFlag) ? false : true;

        bool trueFlag = (exclusionFlag) ? true : false;
        bool falseFlag = (exclusionFlag) ? false : true;

        int exclusionDestNum = 0;

        if (FindPanel.rankNum !< charaScript.rank)
        {
            return true;
        }

        //$b 移動先の絞り込みリスト
        foreach(GameObject destObject in FindPanel.destList)
        {
            //$b 指定した移動先を持っていない場合
            if ((0, 0) == charaScript.posValueList.Find(destT => $"({destT.x}, {destT.y})" == destObject.name))
            {
                if (exclusionFlag) {exclusionDestNum++;}
                else {return falseFlag;}
            }
        }
        if (exclusionFlag && exclusionDestNum == FindPanel.destList.Count) {return falseFlag;}

        //$b 属性の絞り込みリスト
        foreach(GameObject typeObject in FindPanel.typeList)
        {
            //$b 指定した属性を持っていない場合
            switch (typeObject.name)
            {
                case "King": if (!charaScript.isKing) {return falseFlag;} break;
                case "Promote": if (charaScript.promoteId == 0) {return falseFlag;} break;
                case "Skill": if (charaScript.skillList.Count() == 0) {return falseFlag;} break;
                case "消滅": if (!charaScript.skillList.Contains("消滅")) {return falseFlag;} break;
                case "霧払い": if (!charaScript.skillList.Contains("霧払い")) {return falseFlag;} break;
                case "霧生成(自)": if (!charaScript.skillList.Contains("霧生成(自)")) {return falseFlag;} break;
                
                
                default: return falseFlag;
            }
        }

        return trueFlag;
    }

    public void SortPieceList()
    {
        OrgMain.game.clickObj = gameObject;
        OrgMain.game.uiClickFlag = true;
        // 絞り込み
        // 移動先、王、コスト、（ランク）、解放未開放
        // [1, 2, 4], true, [0, 50], [1, 2, 3], true

        //List<int> squeezeMoveList = new List<int>() {1, 2};
        
        //foreach (int squeeze in squeezeList)
        //{
        //    switch (squeeze)
        //    {
        //        case 1:
        //            for (int i = length - 1; i >= 0 ; i--)
        //            {
        //                sortList[i].
        //            }
        //        default:
        //    }
        //    
        //}

        ////addPieceListObject.SetActive(true);
        
        foreach (Transform pieceObject in contents.transform)
        {
            Destroy(pieceObject.gameObject);
        }

        // ソート
        // 移動先の数、コスト
        List<CharaScript> sortedList;
        if (!FindPanel.orderFlag)
        {
            sortedList = sortList.OrderBy(CharaScript => CharaScript.cost).ToList();
        }
        else
        {
            sortedList = sortList.OrderByDescending(CharaScript => CharaScript.cost).ToList();
        }
        List<CharaScript> findList = new List<CharaScript>();

        foreach (CharaScript charaScript in sortedList)
        {
            // キャラを作成
            GameObject charaContent = Instantiate((GameObject)Resources.Load(
                "Syogi/Chara/PieceTemplates/UIAddPieceTemplate"), new Vector3(0, 0, Layer.UI), 
                Quaternion.identity) as GameObject;

            // 情報セット
            OrgSetCharaInfo(charaContent, 0, 0, charaScript.charaId, (0, 0));
            Destroy(charaContent); 
            if (FindPieceList(charaContent.GetComponent<CharaScript>(), FindPanel.exclusionFlag)) {continue;}
            findList.Add(charaScript);
        }

        totalCharaNumber2 = findList.Count();

        Vector2 contentsSize = contents.GetComponent<RectTransform>().sizeDelta;
        contentsSize.y = 10; // 隙間確保
        contentsSize.y += (totalCharaNumber2 / 3) * 80; // 基本枠
        contentsSize.y += ((totalCharaNumber2 % 3) != 0) ? 80 : 0; // 余り枠
        contents.GetComponent<RectTransform>().sizeDelta = contentsSize;

        float x = -60; float y = (contentsSize.y / 2) + 45; // -60 260 : 60 -80

        for (int i = 1; i <= totalCharaNumber2; i++) {
        
            x = -60 + (60 * ((i - 1) % 3));
            y -= (((i - 1) % 3) == 0) ? 80 : 0;

            // キャラを作成
            GameObject charaContent = Instantiate((GameObject)Resources.Load(
                "Syogi/Chara/PieceTemplates/UIAddPieceTemplate"), new Vector3(x, y, Layer.UI), 
                Quaternion.identity) as GameObject;

            // 情報セット
            OrgSetCharaInfo(charaContent, 0, 0, findList[i-1].charaId, (0, 0));

            if (FindPieceList(charaContent.GetComponent<CharaScript>(), FindPanel.exclusionFlag)) {Destroy(charaContent); continue;}

            // Contentに移動
            charaContent.transform.SetParent(GameObject.Find(
                $"UICanvas/AddPieceList/Viewport/Contents").transform, false);
        }

        ////addPieceListObject.SetActive(false);
    }



    public void OrgSetCharaInfo(GameObject charaObj, int p_num, int chara_p_num, CharaId charaId, (int x, int y) coordTupleT) {
        
        CharaScript charaScript = charaObj.GetComponent<CharaScript>();
        charaScript.obj = charaObj;

        charaScript.p_num = chara_p_num;
        charaScript.mine = (p_num == chara_p_num);

        charaScript.charaId = charaId;
        charaScript.coordTuple = coordTupleT;
        charaScript.SetChara_info(charaScript);

        charaScript.obj.transform.Find("PieceRestNumber"
            ).GetComponent<TextMeshProUGUI>().text = 
            "   " + charaScript.charaName;

        FuncK.SetCharaName(charaScript);
        FuncK.SetMovableMark(charaScript);
    }
}
