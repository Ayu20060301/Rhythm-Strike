using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ƒƒjƒ…[UI‚Ìí—Ş
public enum MenuUIType
{
    TITLE, 
    OPTION,
    TUTORIAL,
    PLAY
}


[CreateAssetMenu(fileName = "MenuUIData", menuName = "UI/MenuUIData‚ğì¬")]
public class MenuUIData : ScriptableObject
{
    public int id;
    public Sprite menuUI;
    public MenuUIType type;

    [Header("BGM")]
    public bool stopBGMOnDecide = true;
}
