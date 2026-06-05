using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メニューBGMの制御
/// </summary>
public class MenuBGM : MonoBehaviour
{

    /// <summary>
    /// メニュー画面のBGMを再生する関数
    /// </summary>
    public void MenuBGMStart()
    {
        BGMManager.instance.SetBGMState(BGMType.MENU);
    }

    /// <summary>
    /// メニュー画面のBGMを止める関数
    /// </summary>
    public void StopBGM()
    {
        BGMManager.instance.BGMStop();
    }
}
