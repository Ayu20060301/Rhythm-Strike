using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : SingletonMonoBehaviour<BGMManager>
{ 
    private AudioSource _audio;

    //DataBaseManagerから取得する
    private SoundDataBase _soundDB => DataBaseManager.instance.soundDB;

    private Dictionary<BGMType, BGMData> _bgmDict = new();

    protected override void Awake()
    {
        base.Awake();

        _audio = GetComponent<AudioSource>();


        //SoundDataBase内のデータをDictionaryに変換
        foreach (var data in _soundDB.bgmDatas)
        {
            _bgmDict[data.type] = data;
        }

        RefreshVolume();
    }



    /// <summary>
    /// BGMを設定
    /// </summary>
    /// <param name="type">再生するBGMのタイプ</param>
    public void SetBGMState(BGMType type)
    {
        if (_bgmDict.TryGetValue(type, out var bgmData))
        {
            _audio.clip = bgmData.clip;
            _audio.loop = true;
            _audio.Play();
        }
        else
        {
            Debug.Log($"BGMDataが見つかりません:{type}");
        }
    }

    /// <summary>
    /// BGMを止める
    /// </summary>
    public void BGMStop()
    {
        _audio.Stop();
    }

    /// <summary>
    /// 音量設定を再反映
    /// マスター音量　×　SE音量
    /// </summary>
    public void RefreshVolume()
    {
        _audio.volume = GManager.instance.bgmVolume * GManager.instance.masterVolume;
    }
}
