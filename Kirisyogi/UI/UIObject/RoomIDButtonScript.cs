using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Linq;

public class RoomIDButtonScript : MonoBehaviour
{
    public RoomIDScript roomIDScript;

    public void InputNumber()
    {
        string  roomIDText = roomIDScript.roomIDText.GetComponent<TextMeshProUGUI>().text;
        int IDLen = (roomIDText == null) ? 0 : roomIDText.Length;

        if (6 < IDLen) {return;}
        roomIDScript.roomIDText.GetComponent<TextMeshProUGUI>().text += gameObject.name + " ";
    }

    public void RemoveNumber()
    {
        string  roomIDText = roomIDScript.roomIDText.GetComponent<TextMeshProUGUI>().text;
        int IDLen = (roomIDText == null) ? 0 : roomIDText.Length;

        if (IDLen < 2) {return;}
        roomIDScript.roomIDText.GetComponent<TextMeshProUGUI>().text = roomIDText.Remove(IDLen-2, 2);
    }

    public void RoomBattleTransition()
    {
        string  roomIDText = roomIDScript.roomIDText.GetComponent<TextMeshProUGUI>().text;
        int IDLen = (roomIDText == null) ? 0 : roomIDText.Length;
        if (IDLen == 0) {GameData.roomID = null;}
        else {GameData.roomID = roomIDText.Replace(" ", "");}
    }
}
