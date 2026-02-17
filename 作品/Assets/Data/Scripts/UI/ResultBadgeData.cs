using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResultBadgeType
{
    ALLCRITICAL, 
    FULLCOMBO, 
    FAILED,
    DEATH,
    STAGECLEAR
}

[CreateAssetMenu(fileName = "ResultBadgeData", menuName = "UI/ResultBadgeDataÇçÏê¨")]

public class ResultBadgeData : ScriptableObject
{
    public ResultBadgeType type;
    public Sprite badgeSprite;
}
