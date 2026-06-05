using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UIDataBase", menuName = "UI/UIDataBase‚ğì¬")]

public class UIDataBase : ScriptableObject
{

    [Header("ResultBadgeData")]
    public List<ResultBadgeData> badgeDatas = new List<ResultBadgeData>();

    [Header("RankUIData")]
    public List<RankUIData> rankUIDatas = new List<RankUIData>();

    [Header("MenuUIData")]
    public List<MenuUIData> menuUIData = new List<MenuUIData>();

    //BadgeData‚Ìæ“¾
    public ResultBadgeData GetBadge(ResultBadgeType type)
    {
        return badgeDatas.Find(x => x.type == type);
    }

    //RankData‚Ìæ“¾
    public RankUIData GetRank(RankUIType type)
    {
        return rankUIDatas.Find(x => x.type == type);
    }

    //MenuUIData‚Ìæ“¾
    public MenuUIData GetMenu(MenuUIType type)
    {
        return menuUIData.Find(x => x.type == type);
    }

}
