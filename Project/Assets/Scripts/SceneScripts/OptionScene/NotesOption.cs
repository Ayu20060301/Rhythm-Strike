using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ノーツの速度を管理するクラス
/// </summary>
public class NotesOption : MonoBehaviour
{

    [SerializeField]
    private Button _decreaseNoteSpeed; //速度を下げる用のボタン
    [SerializeField]
    private Button _increaseNoteSpeed; //速度を上げる用のボタン
    [SerializeField]
    private TextMeshProUGUI _speedText;


    private float _minSpeed = 1.0f; //最低速度
    private float _maxSpeed = 20.0f; //最大速度
    private float _step = 0.5f;

    private float _noteSpeed; //現在のノーツ速度


    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        _noteSpeed = PlayerPrefs.GetFloat("NOTE_SPEED", 12.0f);


        UpdateText();
        GManager.instance.notesSpeed = _noteSpeed;

        _decreaseNoteSpeed.onClick.AddListener(OnClickDecrease);
        _increaseNoteSpeed.onClick.AddListener(OnClickIncrease);

        UpdateButtonState();
    }

    /// <summary>
    /// ボタンの状態を更新
    /// </summary>
    private void UpdateButtonState()
    {
        bool iscanDecrease = _noteSpeed > _minSpeed;
        bool iscanIncrease = _noteSpeed < _maxSpeed;

        _decreaseNoteSpeed.interactable = iscanDecrease;
        _increaseNoteSpeed.interactable = iscanIncrease;

    }

    /// <summary>
    /// 速度を下げるボタンを押した際の処理
    /// </summary>
    public void OnClickDecrease()
    {
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CLICK);

        _noteSpeed = Mathf.Clamp(_noteSpeed - _step, _minSpeed, _maxSpeed);
        GManager.instance.notesSpeed = _noteSpeed;
        UpdateText();

        UpdateButtonState();
    }

    /// <summary>
    /// 速度を上げるボタンを押した際の処理
    /// </summary>
    public void OnClickIncrease()
    {
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CLICK);

        _noteSpeed = Mathf.Min(_maxSpeed, _noteSpeed + _step);
        GManager.instance.notesSpeed = _noteSpeed;
        UpdateText();
        UpdateButtonState();
    }


    /// <summary>
    /// テキストの更新
    /// </summary>

    void UpdateText()
    {
        _speedText.text = _noteSpeed.ToString("f1");
    }

    /// <summary>
    /// 現在の設定を保存
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("NOTE_SPEED", _noteSpeed);
        PlayerPrefs.Save();
    }
}