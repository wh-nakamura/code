using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandObj: MonoBehaviour
{
    public List<List<CharaScript>> hand_li = new List<List<CharaScript>>(){
        new List<CharaScript>(){},
        new List<CharaScript>(){},
    };

    //$p 手札の駒の更新
    public void hand_update(List<List<CharaScript>> hand_li) {
        
        foreach (List<CharaScript> hand_t in hand_li) {

            int numI = 0;
            foreach (CharaScript handChara_sc in hand_t) {

                try {
                    Vector3 pos = handChara_sc.obj.transform.position;
                    if (handChara_sc.mine) {
                        pos.y = Coord.o - 1;
                        pos.x = Coord.o + numI;
                    }
                    else {
                        pos.y = Coord.y + 1;
                        pos.x = Coord.x - numI;
                    }
                    numI += 1;

                    handChara_sc.obj.transform.position = pos;
                    handChara_sc.coordTuple = ((int)pos.x, (int)pos.y);
                    
                    ////Active_collider(handChara_sc.obj);
                } catch (System.NullReferenceException) {continue;}
            }
        }
    }
}