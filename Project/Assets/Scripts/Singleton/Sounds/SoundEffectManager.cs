using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : SingletonMonoBehaviour<SoundEffectManager>
{
    private AudioSource _audio;
  
    //DataBaseManagerから取得する
    private SoundDataBase _soundDB => DataBaseManager.instance.soundDB;

    private Dictionary<SoundEffectType, SoundEffectData> _soundEffectDict = new();

    protected override void Awake()
    {
        base.Awake();

        _audio = GetComponent<AudioSource>();

        

        //SoundDataBase内のデータをDictionaryに変換
        foreach (var data in _soundDB.soundEffectDatas)
        {
            _soundEffectDict[data.type] = data;
        }
    }

    private IEnumerator Start()
    {
        yield return null;

        //SoundDataBase内のデータをDictionaryに変換
        foreach (var data in _soundDB.soundEffectDatas)
        {
            _soundEffectDict[data.type] = data;
        }

        RefreshVolume();
    }
   

    /// <summary>
    /// 効果音を設定
    /// </summary>
    /// <param name="type">再生する効果音のタイプ</param>
    public void SetSoundEffectState(SoundEffectType type)
    {
        if(_soundEffectDict.TryGetValue(type,out var soundEffectData))
        {
            //   _audio.clip = soundEffectData.clip;
            // _audio.Play();
            _audio.PlayOneShot(soundEffectData.clip);
        }
        else
        {
            Debug.Log($"SoundEffectDataが見つかりません:{type}");
        }
    }

    /// <summary>
    /// 音量設定を再反映
    /// </summary>
    public void RefreshVolume()
    {
        _audio.volume = GManager.instance.seVolume * GManager.instance.masterVolume;
    }

}
