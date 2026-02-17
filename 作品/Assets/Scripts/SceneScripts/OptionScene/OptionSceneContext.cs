using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OptionReturnType
{
    NONE,
    MENU,
    SELECT
}

/// <summary>
/// オプション画面に遷移する直前のシーン種別を保持する
/// </summary>
public static class OptionSceneContext
{
    public static OptionReturnType ReturnType = OptionReturnType.NONE;
}
