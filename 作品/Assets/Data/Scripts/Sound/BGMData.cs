using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//BGM‚Ìí—Ş
public enum BGMType
{
    TITLE,
    RESULT,
    MENU,
    OPTION
}

[CreateAssetMenu(fileName = "BGMData", menuName = "Sound/BGMData‚ğì¬")]

public class BGMData : ScriptableObject
{
    public BGMType type;
    public AudioClip clip;
}
