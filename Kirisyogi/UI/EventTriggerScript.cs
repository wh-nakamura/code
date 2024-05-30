using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerScript : MonoBehaviour
{
    public void HoverChara() {
        OrgMain.game.hoverObj = gameObject;
        OrgMain.game.hoverChara = gameObject.GetComponent<CharaScript>();
    }

    public void ReturnObject() {
        OrgMain.game.clickObj = gameObject;
        OrgMain.game.uiClickFlag = true;
    }

    public void ScrollObject() {
        Func.ScrollObject(AddPieceListScript.This.scrollbar);
    }

    public void HoverSE() {
        SEScript.This.RingSE(SoundKind.otherSE);
    }

    public void HoverCharaSE() {
        SEScript.This.RingSE(SoundKind.hoverCharaSE);
    }

    public void aa()
    {
        print(gameObject);
        print(this);
        print(this.gameObject);
    }
}
