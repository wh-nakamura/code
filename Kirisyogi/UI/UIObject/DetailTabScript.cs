using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DetailTabScript : MonoBehaviour
{
    public GameObject pieceText;
    public GameObject pieceImage;
    public GameObject piecePromoteImage;

    public static DetailTabScript This = null;

    void Awake() {
        if (This == null) {
            This = this;
        }
    }

    public void DispDetailTab(CharaScript hoverChara)
    {
        //$p キャラの詳細情報表示
        
        // 詳細情報のアクティブ化
        pieceText.SetActive(true);
        ////pieceImage.SetActive(true);
        ////piecePromoteImage.SetActive(false);

        int childCount = GameObject.Find(
            $"UICanvas/InfoWindow/DetailTab/PieceDetail").transform.childCount;
        if (3 <= childCount)
        {
            Destroy(GameObject.Find($"UICanvas/InfoWindow/DetailTab/PieceDetail").transform.GetChild(2).gameObject);
        }
        if (2 <= childCount)
        {
            Destroy(GameObject.Find($"UICanvas/InfoWindow/DetailTab/PieceDetail").transform.GetChild(1).gameObject);
        }

        // 詳細情報の代入
        pieceText.GetComponent<TextMeshProUGUI>().text = 
            hoverChara.SetChara_info_text();
        
        // キャラを作成
        GameObject charaContent = Instantiate((GameObject)Resources.Load(
            "Syogi/Chara/PieceTemplates/UIPieceDetailTemplate"), new Vector3(50, 65, Layer.UI), 
            Quaternion.identity) as GameObject;

        // Contentに移動
        charaContent.transform.SetParent(GameObject.Find(
            $"UICanvas/InfoWindow/DetailTab/PieceDetail").transform, false);

        // 情報セット
        OrgSetCharaInfo(charaContent, 0, 0, hoverChara.charaId, (0, 0));

        ////pieceImage.GetComponent<Image>().sprite = 
        ////    Resources.Load<Sprite>(CharaData.Collation_prefab(hoverChara.charaId));

        //$b 成れる駒なら
        if (hoverChara.promoteId != 0) {
            AddPromoteInfo(hoverChara.promoteId);
        }
        //$b 成っている駒なら
        else if (hoverChara.promoteState) {
            AddPromoteInfo(hoverChara.originId);
        }
    }

    //$repeat キャラの成り関連の情報の追加
    public void AddPromoteInfo(CharaId charaId)
    {
        ////piecePromoteImage.SetActive (true);
        pieceText.GetComponent<TextMeshProUGUI>().text += 
            $"<br><br><br>　駒名：　{CharaData.GetCharaName(charaId)}";
        
        // キャラを作成
        GameObject charaContent = Instantiate((GameObject)Resources.Load(
            "Syogi/Chara/PieceTemplates/UIPieceDetailTemplate"), new Vector3(50, -40, Layer.UI), 
            Quaternion.identity) as GameObject;

        // Contentに移動
        charaContent.transform.SetParent(GameObject.Find(
            $"UICanvas/InfoWindow/DetailTab/PieceDetail").transform, false);

        // 情報セット
        OrgSetCharaInfo(charaContent, 0, 0, charaId, (0, 0));

        ////piecePromoteImage.GetComponent<Image>().sprite = 
        ////    Resources.Load<Sprite>(CharaData.Collation_prefab(charaId));
    }

    public void OrgSetCharaInfo(GameObject charaObj, int p_num, int chara_p_num, CharaId charaId, (int x, int y) coordTupleT) {
        
        CharaScript charaScript = charaObj.GetComponent<CharaScript>();
        charaScript.obj = charaObj;

        charaScript.p_num = chara_p_num;
        charaScript.mine = (p_num == chara_p_num);

        charaScript.charaId = charaId;
        charaScript.coordTuple = coordTupleT;
        charaScript.SetChara_info(charaScript);

        FuncK.SetCharaName(charaScript);
        FuncK.SetMovableMark(charaScript);
    }
}
