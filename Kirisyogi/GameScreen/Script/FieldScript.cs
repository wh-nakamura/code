using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    public GameObject obj;

    public int status = FieldColor.basic;
    public int statusT = FieldColor.basic;
    public int mistStatus = 0;
    public int mistStatusT = 0;
    public string mistKind = "Fog";


    public Color nowFieldColor;

    
    public GameObject Mist;


    // 霧種類、誰の霧か、霧の関連
    // 1, 2, 3  [1, 2]   []

    void Start() {
        // フィールドのデフォルトカラー
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 215, 160, 255);
    }

    void OnMouseEnter()
    {
        SEScript.This.RingSE(SoundKind.hoverFieldSE);
    }

    void Update()
    {
        //$p 盤面の色の更新
        UpdateFieldColor();
        
        //$p 霧の更新
        UpdateFieldMist();
    }

    public void UpdateFieldColor()
    {
        if (status != statusT)
        {
            switch (status)
            {
                case FieldColor.basic:
                    nowFieldColor = new Color32(255, 215, 160, 255); break;
                case FieldColor.canMove:
                    nowFieldColor = new Color32(170, 255, 255, 255); break;
                case FieldColor.attackMove:
                    nowFieldColor = new Color32(255, 100, 75, 255); break;
                case FieldColor.noMove:
                    nowFieldColor = new Color32(100, 100, 100, 100); break;
                case FieldColor.beforeMove:
                    nowFieldColor = new Color32(100, 255, 100, 255); break;
            }
            this.GetComponent<SpriteRenderer>().color = nowFieldColor;

            statusT = status;
        }
    }

    public void UpdateFieldMist()
    {
        if (mistStatus != mistStatusT)
        {
            if (mistStatus == 0) {Mist.SetActive(false);}
            else 
            {
                switch (mistStatus)
                {
                    case 1: mistKind = "Haze"; break;
                    case 2: mistKind = "Mist"; break;
                    case 3: mistKind = "Fog"; break;
                    default: print("error"); break;
                }

                Mist.SetActive(true);
                Mist.GetComponent<SpriteRenderer>().sprite = 
                    Resources.Load<Sprite>($"Syogi/Picture/Mist/{mistKind}");
            }

            mistStatusT = mistStatus;
        }
    }
}
