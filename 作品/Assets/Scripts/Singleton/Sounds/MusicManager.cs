using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曲を管理するクラス
/// </summary>
public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    //DataBaseManagerの取得
    private SongDataBase _songDB => DataBaseManager.instance.songDB;

    private AudioSource _audio; //AudioSourceの変数
    private AudioClip _audioClip; //AudioClipの変数
    private string _songName; //曲名を入れる変数
    private bool _isPlayed; //曲が流れるかのフラグ
    private bool _isStoppedByDeath = false;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //AudioSource取得
        _audio = GetComponent<AudioSource>(); 

        //状態初期化
        _isStoppedByDeath = false;
        GManager.instance.isStart = false;
        GManager.instance.isPause = false;
        GManager.instance.isDeath = false;

        //IDから曲名を取得
        _songName = _songDB.songData[GManager.instance.songID].songName; //音楽のファイル名
        
        //Resourcesから曲をロード
        _audioClip = (AudioClip)Resources.Load("Musics/" + _songName); 
        
        //AudioSourceに設定
        _audio.clip = _audioClip;
    }

    /// <summary>
    /// 毎フレーム更新処理
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //死亡時は曲を止める
        if (GManager.instance.isDeath && !_isStoppedByDeath)
        {
            GManager.instance.isStart = false;
            GManager.instance.isDeath = true;
            StopMusic();
            _isStoppedByDeath = true;
            return;
        }

        //ゲームが開始していない場合は処理をしない
        if (!GManager.instance.isStart) return;


        //ポーズ処理
        if (GManager.instance.isPause)
        {
           //再生中なら一時停止
            if (_audio.isPlaying)
                Pause();
            return;
        }
        else
        {
            //ポーズ解除時に再生復帰
            if (!_audio.isPlaying)
            {
                UnPause();
            }
        }  
    }

    /// <summary>
    /// 死亡停止フラグのリセット
    /// </summary>
    public void ResetDeathState()
    {
        _isStoppedByDeath = false;
    }

    /// <summary>
    /// 曲を最初から再生
    /// </summary>
    public void PlayFromStart()
    {
        _audio.time = 0;
        GManager.instance.startTime = Time.time;
        _audio.Play();
    }

    /// <summary>
    /// 現在位置から再生
    /// </summary>
    public void PlayFromCurrent()
    {
        if(!_audio.isPlaying)
        {
            _audio.Play();
        }
    }

    /// <summary>
    /// 曲を完全停止
    /// </summary>
    public void StopMusic()
    {
        _audio.Stop();
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    public void Pause()
    {
        _audio.Pause();
    }

    /// <summary>
    /// 一時停止解除
    /// </summary>
    public void UnPause()
    {
        _audio.UnPause();
    }

    /// <summary>
    /// 音量設定を再反映
    /// マスター音量　×　BGM音量
    /// </summary>
    public void RefreshVolume()
    {
        _audio.volume = GManager.instance.musicVolume * GManager.instance.masterVolume;
    }

    /// <summary>
    /// 再生するAudioClipを差し替える
    /// </summary>
    /// <param name="clip">オーディオファイル</param>
    public void SetAudioClip(AudioClip clip)
    {
        _audioClip = clip;
        _audio.clip = clip;
    }

    /// <summary>
    /// 曲の再生時間の取得
    /// </summary>
    /// <returns>曲の長さを返す</returns>
    public float GetSongLength()
    {
        return _audio.clip.length;
    }
}
