using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using System.Linq;

public class FindPanelButtonScript : MonoBehaviour
{
    public FindPanelScript FindPanel;

    public void OrderButton()
    {
        OrgMain.game.clickObj = gameObject;
        OrgMain.game.uiClickFlag = true;

        if (FindPanel.orderFlag)
        {
            FindPanel.orderFlag = false;
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            FindPanel.orderFlag = true;
            gameObject.GetComponent<Image>().color = new Color32(125, 125, 125, 255);
        }
    }

    public void ExclusionButton()
    {
        OrgMain.game.clickObj = gameObject;
        OrgMain.game.uiClickFlag = true;

        if (FindPanel.exclusionFlag)
        {
            FindPanel.exclusionFlag = false;
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            FindPanel.exclusionFlag = true;
            gameObject.GetComponent<Image>().color = new Color32(125, 125, 125, 255);
        }
    }

    public void ClickTypeButton()
    {
        OrgMain.game.clickObj = gameObject;
        OrgMain.game.uiClickFlag = true;

        if (!FindPanel.typeList.Find(type => type == gameObject))
        {
            FindPanel.typeList.Add(gameObject);
            gameObject.GetComponent<Image>().color = new Color32(125, 125, 125, 255);
        }
        else
        {
            FindPanel.typeList.Remove(gameObject);
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void ClickDestButton()
    {
        OrgMain.game.clickObj = gameObject;
        OrgMain.game.uiClickFlag = true;

        if (!FindPanel.destList.Find(dest => dest == gameObject))
        {
            FindPanel.destList.Add(gameObject);
            gameObject.GetComponent<Image>().color = new Color32(125, 125, 125, 255);
        }
        else
        {
            FindPanel.destList.Remove(gameObject);
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void ClickRankButton()
    {
        OrgMain.game.clickObj = gameObject;
        OrgMain.game.uiClickFlag = true;

        FindPanel.rankNum++;
        if (8 < FindPanel.rankNum) {FindPanel.rankNum = 1;}
        Func.EditText(gameObject.transform.GetChild(0).gameObject, FindPanel.rankNum.ToString());
    }
}
