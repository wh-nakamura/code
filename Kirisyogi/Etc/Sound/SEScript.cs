using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEScript : MonoBehaviour
{
    //** hoverCharaSE, hoverFieldSE, otherSE, autoSE, positiveSE, negativeSE,
    //** PieceMoveSE, TurnChangeSE, VictorySE, DefeatSE, ExtinctionSE, CreateMistSE,
    public AudioClip[] SEList = new AudioClip[12];
    
    public AudioSource SEAudioSource;

    public static SEScript This = null;

    void Awake()
    {
        SEAudioSource = GetComponent<AudioSource>();
        if (This == null) {
            This = this;
        }
    }

    public void RingSE(int SENumber) 
    {
        SEAudioSource.volume = (SENumber <= 6) ? 0.1f : 0.6f;
        SEAudioSource.PlayOneShot(SEList[SENumber-1]);
    }
}
