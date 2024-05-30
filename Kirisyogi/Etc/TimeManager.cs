using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static int hour_time;
    public static int minute_time;
    public static int second_time;

    public GameObject test;
    
    void Start()
    {
        hour_time = DateTime.Now.Hour;
        minute_time = DateTime.Now.Minute;
        second_time = DateTime.Now.Second;
        test.GetComponent<TextMeshProUGUI>().text = $"{hour_time}/{minute_time}/{second_time}";
    }

    void Update()
    {
        hour_time = DateTime.Now.Hour;
        minute_time = DateTime.Now.Minute;
        second_time = DateTime.Now.Second;
        test.GetComponent<TextMeshProUGUI>().text = $"{hour_time}/{minute_time}/{second_time}";
    }
}
