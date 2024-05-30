using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;

public class Button_sc : MonoBehaviour
{
    public GameObject gameObj;
    public GameObject gameObj_2;

    public Button testaa;

    public void Transition_Battle() {
        SceneManager.LoadScene("Scenes/Battle");
    }

    public void Transition_Org() {
        SceneManager.LoadScene("Scenes/Org");
    }

    public void Transition_Title() {
        SceneManager.LoadScene("Scenes/Title");
    }

    public void Transition_Config() {
        SceneManager.LoadScene("Scenes/Config");
    }

    public void EndGame() {
        Application.Quit();
    }

    public void SurrenderProcess() {
        Main.game.SurrenderProcess();
        SEScript.This.RingSE(SoundKind.negativeSE);
    }

    public void Save() {
        OrgMain.game.Save();
        SceneManager.LoadScene("Scenes/Battle");
    }

    public void Delete() {
        OrgMain.game.DeleteChara();
        OrgMain.game.EndMove();
    }

    public void DeleteAllOrgChara()
    {
        OrgMain.game.DeleteAllOrgChara();
        OrgMain.game.EndMove();
    }

    public void ResetOrgChara()
    {
        OrgMain.game.ResetOrgChara();
        OrgMain.game.EndMove();
    }

    public void Hide_clicked() {
        gameObj.SetActive (false);
        GameObject.Find("CollisionFilter").GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Disp_clicked() {
        gameObj.SetActive (true);
        GameObject.Find("CollisionFilter").GetComponent<BoxCollider2D>().enabled = true;
    }

    public void Change_UserData() {
        RefUserData.AsgUserData(gameObj);
        RefUserData.ReflUserData(gameObj_2);
    }

    public void OpenAddPieceList() {
        OrgMain.game.OpenAddPieceList();
        GameObject.Find("CollisionFilter").GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ChangeFieldSize()
    {
        Coord.y = (((Coord.y-4) % 2) == 0) ? 5 : 4;
    }

    public void SortButton()
    {
        AddPieceListScript.This.SortPieceList();
    }

    public void EndDeleteMist()
    {
        Main.game.EndDeleteMist();
    }

    public void SoundOnOff()
    {
        bool isSound = (SEScript.This.SEAudioSource.mute == true) ? false : true;
        SEScript.This.SEAudioSource.mute = isSound;
        BgmScript.This.BgmAudioSource.mute = isSound;
    }

    public void test() {
        print(1);
    }
    

    void Update() {
        if (testaa) {
            testaa.interactable = ((TimeManager.second_time % 2) == 0) ? true: false;
        }
    }

    


}
