using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBarManager : MonoBehaviour
{
    private bool start_pass = true;

    public bool countPass = false;

    public float max_value;

    public Slider slider;
    public Color fillColor;
    public GameObject Fill;

    public static TimeBarManager This = null;

    void Awake() {
        if (This == null) {
            This = this;
        }
    }
    
    void Update()
    {
        //$b Startの代わり
        if (Main.game && start_pass) {
            Main.game.time_bar_sc = this;
            start_pass = false;
            fillColor = Fill.GetComponent<Image>().color;
        }

        if (countPass)
        {
            slider.value -= Time.deltaTime;

            //$b 時間切れになった場合
            if (slider.value == 0) {
                Main.game.TimeOutProcess();
                countPass = false;
            }
            //$b 残り時間4分の1の場合
            else if (slider.value < max_value/4) {
                fillColor = new Color32(255, 100, 75, 255);
            }
            //$b 残り時間半分の場合
            else if (slider.value < max_value/2) {
                fillColor = new Color32(255, 200, 30, 255);
            }
            //$b 時間に余裕がある場合
            else {
                fillColor = new Color32(140, 255, 220, 255);
            }
        }
        //$b 相手ターンの場合
        else {
            fillColor = new Color32(160, 160, 160, 255);
        }
        Fill.GetComponent<Image>().color = fillColor;
    }
}
