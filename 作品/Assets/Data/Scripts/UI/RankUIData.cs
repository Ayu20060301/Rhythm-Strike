using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rankタイプ
public enum RankUIType
{
    SSS,
    SS,
    S,
    A,
    B,
    C,
    D
}

[CreateAssetMenu(fileName = "RankUIData", menuName = "UI/RankUIDataを作成")]

public class RankUIData : ScriptableObject
{
    public RankUIType type;
    public Sprite rankSprite; //画像用
}
