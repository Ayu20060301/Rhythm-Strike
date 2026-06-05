using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DifficultyData
{
    [Header("基本情報")]
    public int difficultyID;
    public string levelName; //表示名
    public Color levelColor; //表示カラー

    public TextAsset chartFile; //譜面JSON
}
