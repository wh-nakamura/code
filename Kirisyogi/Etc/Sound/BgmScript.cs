using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmScript : MonoBehaviour
{
    public AudioClip[] BgmList = new AudioClip[3];

    public AudioSource BgmAudioSource;

    public static BgmScript This = null;

    void Awake()
    {
        BgmAudioSource = GetComponent<AudioSource>();
        if (This == null) {
            This = this;
        }
    }

    public void ChangeBgm(int BgmNumber)
    {
        BgmAudioSource.clip = BgmList[BgmNumber-1];
        BgmAudioSource.Play();
    }

    public void StopBgm()
    {
        BgmAudioSource.Stop();
    }
}
