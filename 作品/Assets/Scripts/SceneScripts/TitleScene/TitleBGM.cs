using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル画面のBGMを管理するクラス
/// </summary>
public class TitleBGM : MonoBehaviour
{
    
    /// <summary>
    /// タイトル画面のBGMを再生する関数
    /// </summary>
    public void BGMStart()
    {
        BGMManager.instance.SetBGMState(BGMType.TITLE);
    }
}
