using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mouse
{

    //$p 押下したオブジェクトを取得
    public static GameObject GetDownObj() {
        GameObject downObj = null;

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d) {downObj = hit2d.transform.gameObject;}
        }
        return downObj;
    }

    //$p クリックしたオブジェクトを取得
    public static GameObject GetUpObj()
    {
        GameObject upObj = null;

        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d) {upObj = hit2d.transform.gameObject;}
        }
        return upObj;
    }

    //$p 長押しまたはホバーしたオブジェクトを取得
    public static GameObject GetHoverObj()
    {
        GameObject clickedGameObj = null;

        //$p スマホ用
        //$b 長押し
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d) {clickedGameObj = hit2d.transform.gameObject;}
        
        //$p PC用
        //$b ホバー
        } else {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d) {clickedGameObj = hit2d.transform.gameObject;}
        }

        return clickedGameObj;
    }
}