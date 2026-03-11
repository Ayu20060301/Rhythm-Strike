using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オプション画面のBGMを管理する関数
/// </summary>
public class OptionBGM : MonoBehaviour
{

    /// <summary>
    /// オプション画面のBGMを再生する関数
    /// </summary>
    public void BGMStart()
    {
        BGMManager.instance.SetBGMState(BGMType.OPTION);
    }


    /// <summary>
    /// オプション画面のBGMを再生する関数
    /// </summary>
    public void BGMStop()
    {
        BGMManager.instance.BGMStop();
    }
}
