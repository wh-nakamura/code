using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using System.Linq;

public class RoomIDScript : MonoBehaviour
{
    public GameObject roomIDText;

    public static RoomIDScript This = null;

    void Awake()
    {
        if (This == null) {
            This = this;
        }
    }
}
