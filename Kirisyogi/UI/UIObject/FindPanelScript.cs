using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using System.Linq;

public class FindPanelScript : MonoBehaviour
{
    public List<GameObject> typeList = new List<GameObject> {};
    public List<GameObject> destList = new List<GameObject> {};
    public bool exclusionFlag = false;
    public bool orderFlag = false;
    public int rankNum = 8;

    public static FindPanelScript This = null;

    void Awake()
    {
        if (This == null) {
            This = this;
        }
    }
}
