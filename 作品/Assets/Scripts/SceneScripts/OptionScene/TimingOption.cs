using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// タイミング調整を管理するクラス
/// </summary>
public class TimingOption : MonoBehaviour
{
    [Header("タイミング調整")]
    [SerializeField]
    private Button _decreaseButton;
    [SerializeField]
    private Button _increaseButton;
    [SerializeField]
    private TextMeshProUGUI _timingText;

    [Header("設定値")]
    private float _minTiming = -2.0f; //最低タイミング時間
    private float _maxTiming = 2.0f; //最大タイミング時間
    private float _step = 0.05f;

    private float _timingOffset; //現在のタイミング時間

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //初期値読み込み
        _timingOffset = PlayerPrefs.GetFloat("TIMING_OFFSET", 0.0f);

        //テキストの更新
        UpdateText();
        GManager.instance.timingOffset = _timingOffset;

        //ボタンクリック設定
        _decreaseButton.onClick.AddListener(OnClickDecrease);
        _increaseButton.onClick.AddListener(OnClickIncrease);

        UpdateButtonState();
    }

    /// <summary>
    /// ボタンの状態の更新
    /// </summary>
    private void UpdateButtonState()
    {
        bool iscanDecrease = _timingOffset > _minTiming;
        bool iscanIncrease = _timingOffset < _maxTiming;

        _decreaseButton.interactable = iscanDecrease;
        _increaseButton.interactable = iscanIncrease;
    }

    /// <summary>
    /// 調整速度を下げる際の処理
    /// </summary>
    public void OnClickDecrease()
    {
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CLICK);

        _timingOffset = Mathf.Clamp(_timingOffset - _step, _minTiming, _maxTiming);
        GManager.instance.timingOffset = _timingOffset;
        UpdateText();

        UpdateButtonState();
    }

    /// <summary>
    /// 調整速度を上げる際の処理
    /// </summary>
    public void OnClickIncrease()
    {
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CLICK);

        _timingOffset = Mathf.Min(_maxTiming, _timingOffset + _step);
        GManager.instance.timingOffset = _timingOffset;
        UpdateText();
        UpdateButtonState();
    }

    /// <summary>
    /// テキストの更新
    /// </summary>
    void UpdateText()
    {
        _timingText.text = _timingOffset.ToString("f1"); //少数一桁で表示
    }

    /// <summary>
    /// 現在の設定を保存
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("TIMING_OFFSET", _timingOffset);
        PlayerPrefs.Save();
    }
}
