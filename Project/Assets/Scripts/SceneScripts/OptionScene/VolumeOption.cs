using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 音量調整を管理するクラス
/// </summary>
public class VolumeOption : MonoBehaviour
{
    [Header("スライダー参照")]
    [SerializeField]
    private Slider _masterVolumeSlider;
    [SerializeField]
    private Slider _bgmVolumeSlider;
    [SerializeField]
    private Slider _seVolumeSlider;
    [SerializeField]
    private Slider _musicVolumeSlider;


    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {

        //値の読み込み(デフォルト100)
        float masterVolume = PlayerPrefs.GetFloat("MASTER", 100.0f);
        float bgmVolume = PlayerPrefs.GetFloat("BGM", 100.0f);
        float seVolume = PlayerPrefs.GetFloat("SE", 100.0f);
        float musicVolume = PlayerPrefs.GetFloat("MUSIC", 100.0f);

        //SLiderに反映
        _masterVolumeSlider.value = masterVolume;
        _bgmVolumeSlider.value = bgmVolume;
        _seVolumeSlider.value = seVolume;
        _musicVolumeSlider.value = musicVolume;


        //GManagerに0～1で反映
        GManager.instance.masterVolume = masterVolume / 100.0f;
        GManager.instance.bgmVolume = bgmVolume / 100.0f;
        GManager.instance.seVolume = seVolume / 100.0f;
        GManager.instance.musicVolume = musicVolume / 100.0f;

    }

    /// <summary>
    /// スライダーのリスナー登録
    /// </summary>
    private void OnEnable()
    {
        //Listner登録
        _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        _bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        _seVolumeSlider.onValueChanged.AddListener(OnSEVolumeChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    /// <summary>
    /// スライダーのリスナー解除
    /// </summary>
    private void OnDisable()
    {
        _masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        _bgmVolumeSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);
        _seVolumeSlider.onValueChanged.RemoveListener(OnSEVolumeChanged);
        _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
    }


    /// <summary>
    /// 各音源Managerの音量を初回更新
    /// </summary>
    /// <returns>IEnumeratorを返す</returns>
    private IEnumerator Start()
    {
        yield return null;

        BGMManager.instance.RefreshVolume();
        SoundEffectManager.instance.RefreshVolume();
        MusicManager.instance.RefreshVolume();
    }

    //---スライダーが変化したときの処理---

    /// <summary>
    /// MASTERの音量が変化した際の処理
    /// </summary>
    /// <param name="value">スライダーの値</param>
    private void OnMasterVolumeChanged(float value)
    {
        GManager.instance.masterVolume = value / 100.0f;
        BGMManager.instance.RefreshVolume();
        SoundEffectManager.instance.RefreshVolume();
        MusicManager.instance.RefreshVolume();
    }

    /// <summary>
    /// BGMの音量が変化した際の処理
    /// </summary>
    /// <param name="value">スライダーの値</param>
    private void OnBGMVolumeChanged(float value)
    {
        GManager.instance.bgmVolume = value / 100.0f;
        BGMManager.instance.RefreshVolume();
    }

    /// <summary>
    /// 効果音の音量が変化した際の処理
    /// </summary>
    /// <param name="value">スライダーの値</param>
    private void OnSEVolumeChanged(float value)
    {
        GManager.instance.seVolume = value / 100.0f;
        SoundEffectManager.instance.RefreshVolume();
    }

    /// <summary>
    /// 曲の音量が変化した際の処理
    /// </summary>
    /// <param name="value">スライダーの値</param>
    private void OnMusicVolumeChanged(float value)
    {
        GManager.instance.musicVolume = value / 100.0f;
        MusicManager.instance.RefreshVolume();
    }

    //-----------------------------------

    /// <summary>
    /// 現在の設定を保存
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MASTER", _masterVolumeSlider.value);
        PlayerPrefs.SetFloat("BGM", _bgmVolumeSlider.value);
        PlayerPrefs.SetFloat("SE", _seVolumeSlider.value);
        PlayerPrefs.SetFloat("MUSIC", _musicVolumeSlider.value);
        PlayerPrefs.Save();
    }
}
