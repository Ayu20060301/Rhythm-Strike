using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コンボ数管理クラス
/// </summary>
public class ComboManager : MonoBehaviour
{
    /// <summary>
    /// コンボ数の加算
    /// </summary>
    public void AddCombo()
    {
        GManager.instance.combo++;

        if(GManager.instance.combo > GManager.instance.maxCombo)
        {
            GManager.instance.maxCombo = GManager.instance.combo;
        }

      
    }

    /// <summary>
    /// コンボ数をリセット
    /// </summary>
    public void ResetCombo()
    {
        GManager.instance.combo = 0;


        //一度でもコンボが途切れたことを記録
        GManager.instance.isComboBrokenOnce = true;
    }
}
