using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class MouseRay : MonoBehaviour
{
    public GameObject collisionObject = null;
    public GameObject collisionObject_t = null;

    public float mousePos_x;
    public float mousePos_y;

    public float mousePos_x_t;
    public float mousePos_y_t;

    public static Camera cam;

    public float pos_x_origin1;
    public float pos_x_origin2;
    public float pos_y_origin1;
    public float pos_y_origin2;

    public static float camera_defaultPos_x = 2.5f; // 4.125
    public static float camera_defaultPos_y = 4.5f; // 2.5

    void Start()
    {
        Vector3 CameraPosition = this.transform.position;
        CameraPosition.x += Coord.x*0.5f - 1.5f;
        CameraPosition.y += Coord.y*0.5f - 2f;
        this.transform.position = CameraPosition;
    }

    //!! staticにするのに時間がかかる
    //public static void ResetCameraPos()
    //{
    //    cam.orthographicSize = 3;
    //    Vector3 CameraPosition = this.transform.position;
    //    CameraPosition.x = camera_defaultPos_x + Coord.x*0.5f - 1.5f;
    //    CameraPosition.y = camera_defaultPos_y + Coord.y*0.5f - 2f;
    //    this.transform.position = CameraPosition;
    //}

    void Update()
    {
        ChangeSizeHover();
        try {
            if (!OrgMain.game.addPieceFlag && (4 < Coord.y)) {
                ////if ((0 < Input.mousePosition.x && Input.mousePosition.x < 490) && 
                ////(0 < Input.mousePosition.y && Input.mousePosition.y < 280)) {
                ////    ZoomCameraScroll();
                ////}
                ZoomCameraScroll();

                if (!Main.game.clickedChara) {MoveCameraDrag();}
                //if (!Mouse.GetHoverObj()) {
                //    MoveCameraDrag();
                //}
            }
        }
        catch (System.NullReferenceException)
        {
            if (4 < Coord.y)
            {
                ////if ((0 < Input.mousePosition.x && Input.mousePosition.x < 490) && 
                ////(0 < Input.mousePosition.y && Input.mousePosition.y < 280)) {
                ////    ZoomCameraScroll();
                ////}
                ZoomCameraScroll();
                MoveCameraDrag();
                //if (!Mouse.GetHoverObj()) {
                //    MoveCameraDrag();
                //}
            }
        }
    }

    //$p ホバーによるキャラオブジェクトのサイズ変更
    private void ChangeSizeHover()
    {
        collisionObject = Mouse.GetHoverObj();

        //$b オブジェクトをホバーしているか
        if (collisionObject) {
            //$b ホバーしているオブジェクトの変更時
            if (collisionObject != collisionObject_t)
            {
                Vector3 scale = collisionObject.transform.localScale;

                //$b 拡大したオブジェクトがあるなら
                if (collisionObject_t) {
                    scale = collisionObject_t.transform.localScale;
                    scale.x /= 1.1f; scale.y /= 1.1f;
                    collisionObject_t.transform.localScale = scale;
                    collisionObject_t = null;
                }
                
                //$b 当たり判定のあるオブジェクトをホバーしているなら
                else if (IsCollisionObject(collisionObject)) {
                    scale.x *= 1.1f; scale.y *= 1.1f;
                    collisionObject.transform.localScale = scale;
                    collisionObject_t = collisionObject;
                }

                
            }
        //$b オブジェクトのホバーをしていない場合
        } else {
            //$b 直前までオブジェクトをホバーしていたなら
            if (collisionObject_t) {
                Vector3 scale = collisionObject_t.transform.localScale;
                scale.x /= 1.1f; scale.y /= 1.1f;
                collisionObject_t.transform.localScale = scale;
                collisionObject_t = null;
            }
        }
    }

    private bool IsCollisionObject(GameObject collisionObject) {
        if (collisionObject.CompareTag("CharaTag")) {return true;}
        ////if (collisionObject.CompareTag("FieldTag")) {return true;}
        return false;
    }

    //$p ドラッグによるカメラの移動
    private void MoveCameraDrag()
    {
        //$b 押した瞬間
        if (Input.GetMouseButtonDown(0)) {
            mousePos_x_t = Input.mousePosition.x;
            mousePos_y_t = Input.mousePosition.y;
        }
        //$b マウスを離した時
        else if (Input.GetMouseButtonUp(0)) {
            ////mousePos_x_t = 0;
            ////mousePos_y_t = 0;
        }
        
        else if (Input.GetMouseButton(0)) {
            //$b 押してから動かしていない時
            if (mousePos_x_t == Input.mousePosition.x &&
            mousePos_y_t == Input.mousePosition.y) {
                mousePos_x_t = Input.mousePosition.x;
                mousePos_y_t = Input.mousePosition.y;
            }
            //$b 押してから動かした時
            else if (mousePos_x_t != Input.mousePosition.x ||
            mousePos_y_t != Input.mousePosition.y) {
                //$b 押してから動かしてから止まった時
                if (mousePos_x_t == Input.mousePosition.x &&
                mousePos_y_t == Input.mousePosition.y) {
                    mousePos_x_t = Input.mousePosition.x;
                    mousePos_y_t = Input.mousePosition.y;
                }
                else {
                    // 移動処理
                    // クリック時の座標ー今のマウスの座標
                    //mousePos_x_t - Input.mousePosition.x;
                    //mousePos_y_t - Input.mousePosition.y;
                    // 結果をカメラに加算
                    this.transform.position += new Vector3(
                        //0.1f, 0.1f, 0);
                    //print(mousePos_x_t); print(Input.mousePosition.x);
                        (mousePos_x_t - Input.mousePosition.x)/70, 
                        (mousePos_y_t - Input.mousePosition.y)/70, 0);

                    Vector3 pos = this.transform.position;

                    ////if (pos.x < cam.orthographicSize - 2) {pos.x = cam.orthographicSize - 2;}
                    ////else if (12 - cam.orthographicSize < pos.x) {pos.x = 12 - cam.orthographicSize;}
                    ////if (pos.y < cam.orthographicSize - 1) {pos.y = cam.orthographicSize - 1;}
                    ////else if (9 - pos.y < cam.orthographicSize) {pos.y = 9 - cam.orthographicSize;}
                    ////if (pos.x < cam.orthographicSize+0.5 - (6-cam.orthographicSize)*2) {pos.x = cam.orthographicSize - 2;}
                    ////if (16 - pos.x < cam.orthographicSize) {pos.x = 16 - cam.orthographicSize;}
                    ////if (pos.y < cam.orthographicSize - 1) {pos.y = cam.orthographicSize - 1;}
                    //// 4.5 - 4.5 : 3 - 6 : 1 - 8 : 0 - 9
                    //// 9/16/3/4   x = 16/3   y = 9/4    x = 16/7   y = 9/9
                    ////5*   3.5  7.5  2.5  4
                    ////4*   1.5   9   1.5  5
                    ////3*   0.5
                    ////if (cam.orthographicSize+0.5 - 2*(6-cam.orthographicSize) < pos.x) {pos.x = cam.orthographicSize - 2;}
                    pos = IsCameraPosBorder(pos);

                    this.transform.position = pos;
                }
                
                mousePos_x_t = Input.mousePosition.x;
                mousePos_y_t = Input.mousePosition.y;
            }
        }
    }

    //$p スクロールによるカメラのズーム
    private void ZoomCameraScroll() {

        //$p スマホ用
        if (2 <= Input.touchCount) {
            var touch1 = Input.GetTouch(0);
            var touch2 = Input.GetTouch(1);

            if (pos_x_origin1 != 10000f) {

                //$p x
                float pos_now_x_left = (touch1.position.x < touch2.position.x) ? touch1.position.x : touch2.position.x;
                float pos_now_x_right = (touch1.position.x > touch2.position.x) ? touch1.position.x : touch2.position.x;

                float pos_x_t_left = (pos_x_origin1 < pos_x_origin2) ? pos_x_origin1 : pos_x_origin2;
                float pos_x_t_right = (pos_x_origin1 > pos_x_origin2) ? pos_x_origin1 : pos_x_origin2;

                // zoom in
                if ((pos_now_x_left - pos_x_t_left) <= 0 &&
                0 <= pos_now_x_right - pos_x_t_right) {
                    cam.orthographicSize -= 0.2f * 1;
                }

                // zoom out
                if (0 <= (pos_now_x_left - pos_x_t_left) &&
                pos_now_x_right - pos_x_t_right <= 0) {
                    cam.orthographicSize -= 0.2f * -1;
                }
                if (cam.orthographicSize < 1) {cam.orthographicSize = 1;}
                else if (5 < cam.orthographicSize) {cam.orthographicSize = 5;}

                pos_x_origin1 = touch1.position.x;
                pos_x_origin2 = touch2.position.x;


                //$p y
                float pos_now_y_left = (touch1.position.y < touch2.position.y) ? touch1.position.y : touch2.position.y;
                float pos_now_y_right = (touch1.position.y > touch2.position.y) ? touch1.position.y : touch2.position.y;

                float pos_y_t_left = (pos_y_origin1 < pos_y_origin2) ? pos_y_origin1 : pos_y_origin2;
                float pos_y_t_right = (pos_y_origin1 > pos_y_origin2) ? pos_y_origin1 : pos_y_origin2;

                // in
                if ((pos_now_y_left - pos_y_t_left) <= 0 &&
                0 <= pos_now_y_right - pos_y_t_right) {
                    cam.orthographicSize -= 0.2f * 1;
                }

                // out
                if (0 <= (pos_now_y_left - pos_y_t_left) &&
                pos_now_y_right - pos_y_t_right <= 0) {
                    cam.orthographicSize -= 0.2f * -1;
                }
                if (cam.orthographicSize < 1) {cam.orthographicSize = 1;}
                else if (5 < cam.orthographicSize) {cam.orthographicSize = 5;}

                pos_y_origin1 = touch1.position.y;
                pos_y_origin2 = touch2.position.y;
            }
            pos_x_origin1 = touch1.position.x;
            pos_x_origin2 = touch2.position.x;
            pos_y_origin1 = touch1.position.y;
            pos_y_origin2 = touch2.position.y;
        }
        //$p PC用
        else {
            pos_x_origin1 = 10000f;
            pos_x_origin2 = 10000f;
            pos_y_origin1 = 10000f;
            pos_y_origin2 = 10000f;


            //回転の取得
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            cam = this.GetComponent<Camera>();

            //$b スクロールしたなら
            if (wheel != 0) {
                int wheel_num = (0 < wheel) ? 1 : -1;
                cam.orthographicSize -= 0.3f * wheel_num;
                if (cam.orthographicSize < 1) {cam.orthographicSize = 1;}
                else if (5 < cam.orthographicSize) {cam.orthographicSize = 5;}
                else {Move_camera_zoom(wheel_num);}
            }
        }
    }

    //$c リサイズによるカメラの移動
    private void Move_camera_zoom(int wheel_num) {
        // x = 0...0.25, x = 500...0.25, x = 250...0
        // y = 0...0.125, x = 250...0.125, y = 125...0
        // y = (x - 250)*0.001
        Vector3 pos = this.transform.position;
        pos.x += wheel_num * (Input.mousePosition.x - 250) / 500;
        pos.y += wheel_num * (Input.mousePosition.y - 125) / 500;
        pos = IsCameraPosBorder(pos);
        this.transform.position = pos;
    }

    //$c カメラの可動範囲内か
    private Vector3 IsCameraPosBorder(Vector3 pos) {
        ////5*   3.5  7.5  2.5  4
        ////4*   1.5   9   1.5  5
        ////3*   0.5  11
        ////2*  -1.5
        ////1*  -3.5
        ////if (pos.x < 1.5f*cam.orthographicSize - 4) {pos.x = 1.5f*cam.orthographicSize - 4;}
        ////else if (3.5f*cam.orthographicSize - 3 < pos.x) {pos.x = 3.5f*cam.orthographicSize - 3;}
        ////if (pos.y < cam.orthographicSize - 2.5f) {pos.y = cam.orthographicSize - 2.5f;}
        ////else if (9 - cam.orthographicSize < pos.y) {pos.y = 9 - cam.orthographicSize;}
        // 1 < -1
        float maxBorderX = -0.5f;
        float minBorderX = 6.5f;
        float maxBorderY = -1f;
        float minBorderY = 7f;

        if (pos.x < maxBorderX) {pos.x = maxBorderX;}
        else if (minBorderX < pos.x) {pos.x = minBorderX;}
        if (pos.y < maxBorderY) {pos.y = maxBorderY;}
        else if (minBorderY < pos.y) {pos.y = minBorderY;}
        return pos;
    }
}