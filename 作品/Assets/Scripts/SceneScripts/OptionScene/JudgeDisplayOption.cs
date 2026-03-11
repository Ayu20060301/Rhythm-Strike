using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 判定表示のON/OFFを管理するクラス
/// </summary>
public class JudgeDisplayOption : MonoBehaviour
{

    [SerializeField]
    private Button _leftButton; //OFFボタン
    [SerializeField]
    private Button _rightButton; //ONボタン
    [SerializeField]
    private TextMeshProUGUI _judgeDPText;

    private bool _isOn = false; //現在の判定表示状態

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //保存値読み込み(1 : ON、0 : OFF)
        _isOn = PlayerPrefs.GetInt("JUDGE_DISPLAY", 1) == 1;

        //ボタンクリック設定
        _leftButton.onClick.AddListener(() => SetState(false));
        _rightButton.onClick.AddListener(() => SetState(true));


        UpdateButtonState();

    }

    /// <summary>
    /// 判定表示の状態を変更
    /// </summary>
    /// <param name="on">ONにするならtrue、OFFにするならOFF</param>
    public void SetState(bool on)
    {
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CLICK);

        _isOn = on;

        //ゲーム側に反映
        GManager.instance.isJudgeDisplay = _isOn;

        UpdateButtonState();

    }

    /// <summary>
    /// ボタンとテキストの状態を更新
    /// </summary>
    private void UpdateButtonState()
    {
        //OFFが選択中ならOFFボタンは押せない
        _leftButton.interactable = _isOn;

        //ONが選択中ならONボタンを押せない
        _rightButton.interactable = !_isOn;

        //テキストの更新
        _judgeDPText.text = _isOn ? "ON" : "OFF";
    }

    /// <summary>
    /// 現在の設定を保存
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("JUDGE_DISPLAY", _isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
