using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Œø‰Ê‰¹‚Ìí—Ş
public enum SoundEffectType
{
    HIT, 
    MISS, 
    STRIKE, 
    COIN, 
    JUMP, 
    FAILED, 
    CLEAR, 
    DEATH, 
    START1,
    START2,
    SELECT,  
    DECIDE, 
    AC,  
    FC, 
    CLICK, 
    RANK,
    CANCEL,
    GLOW,
    COUNT
}

[CreateAssetMenu(fileName = "SoundEffectData", menuName = "Sound/SoundEffectData‚ğì¬")]

//Œø‰Ê‰¹‚Ìƒf[ƒ^
public class SoundEffectData : ScriptableObject
{
    public SoundEffectType type;
    public AudioClip clip;
}
