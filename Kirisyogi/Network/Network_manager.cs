using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

using System;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class Network_manager : MonoBehaviourPunCallbacks
{
    public static bool is_endGame = false;
    public static int i;
    private static RoomOptions roomOptions_data;

    public static Photon.Realtime.RoomOptions Return_roomOption() {
        // ルームのカスタムプロパティの初期値
        var initialProps = new ExitGames.Client.Photon.Hashtable();
        initialProps["DisplayName"] = $"{PhotonNetwork.NickName}の部屋";
        initialProps["Message"] = "誰でも参加OK！";

        // ロビーのルーム情報から取得できるカスタムプロパティ（キーの配列）
        var propsForLobby = new[] { "DisplayName", "Message" };

        // 作成するルームのルーム設定を行う
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomProperties = initialProps;
        roomOptions.CustomRoomPropertiesForLobby = propsForLobby;
        roomOptions_data = roomOptions;
        return roomOptions;
    }


    private void Start() {
        
        // プレイヤー自身の名前を"Player"に設定する
        print(PhotonNetwork.NickName);
        PhotonNetwork.NickName = $"{GameData.userName}/{DateTime.Now.ToString("HH:mm:ss")}";
        FuncD.pri("time:", DateTime.Now.ToString("HH:mm:ss"));
        i = 1;
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
        print("マスターサーバー接続要求");
    }


    public override void OnConnectedToMaster() {
        // from OnLeftRoom
        print("マスターサーバー接続完了");
        if (is_endGame) {
            is_endGame = false; print("サーバー待機");}
        else {
            // PhotonNetwork.JoinRandomRoom();
            print($"Room{i}");
            Main.server_pass = true;
            //PhotonNetwork.JoinOrCreateRoom($"Room{i}", Return_roomOption(), null);
        }


        //$p ルーム参加
        //PhotonNetwork.CreateRoom(""/*ルーム名*/, Return_roomOption());
        //PhotonNetwork.JoinRoom("Room");
        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), null);
        //// ランダムなルームに参加する
        //PhotonNetwork.JoinRandomRoom();
        //print("ルーム参加完了");
    }

    public override void OnDisconnected(DisconnectCause cause) {
        print("マスターサーバー切断");
        print($"理由：{cause.ToString()}");
    }

    public override void OnCreatedRoom() {
        print("ルーム作成完了");
    }

    public override void OnJoinedRoom()
    {
        print("ルーム参加完了");
        FuncD.pri("ルームID", $"{i}");
        Main.InitPlayer();
        print("通信待機中");

        if (PhotonNetwork.CurrentRoom.PlayerCount == roomOptions_data.MaxPlayers) {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        print("ルーム作成失敗");
        print($"理由：{message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        print($"ルームへの参加に失敗しました: {message}");
        print($"Room{++i}");
        PhotonNetwork.JoinOrCreateRoom($"Room{i}", Return_roomOption(), null);
        //PhotonNetwork.CreateRoom(null, Return_roomOption());
    }

    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed(short returnCode, string message) {
        print($"ランダムなルームへの参加に失敗しました: {message}");
        PhotonNetwork.CreateRoom(null, Return_roomOption());
    }

    //$p 通信切断
    public static void Discon() {
        //Func.sleep(1500); // 最後のデータ送信の時間稼ぎ
        is_endGame = true;
        PhotonNetwork.LeaveRoom();
    }


    public override void OnLeftRoom() {
        print("ルーム退出完了");
        // to OnConnectedToMaster
    }

    void OnDestroy() {
        PhotonNetwork.Disconnect();
        print("通信切断完了");
    }
}