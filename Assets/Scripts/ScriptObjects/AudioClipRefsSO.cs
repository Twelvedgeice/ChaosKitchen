using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    //AudioClip: “Ù∆µ∆¨∂Œ
    //AudioSource: “Ù∆µ≤•∑≈∆˜
    //AudioListener: …˘“ÙΩ” ’∆˜

    public AudioClip[] chop;
    public AudioClip[] deliverySuccess;
    public AudioClip[] deliveryFail;
    public AudioClip[] footstep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickUp;
    public AudioClip stroveSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;

}
