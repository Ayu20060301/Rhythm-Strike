using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 楽曲プレビュー再生を管理するクラス
/// </summary>
public class SongPreviewPlayer : MonoBehaviour
{
    //楽曲データベース
    private SongDataBase _songDB;

    //再生用AudioSource
    private AudioSource _audio;

    //ロード済みAudioClipのキャッシュ
    private readonly Dictionary<string, AudioClip> _clipCache = new();

    //現在動作中のプレビューコルーチン
    private Coroutine _previewCoroutine;

    /// <summary>
    /// 現在再生中のAudioClipを外部から取得
    /// </summary>
    public AudioClip CurrentClip => _audio.clip;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="db">楽曲データベース</param>
    public void Initialize(SongDataBase db)
    {
        _songDB = db;
        _audio = GetComponent<AudioSource>();

        if (_audio != null)
        {
            _audio.volume = GManager.instance.musicVolume * GManager.instance.masterVolume;
        }  
    }

    /// <summary>
    /// 指定インデックスの楽曲プレビューを再生
    /// </summary>
    /// <param name="index">現在選択中のインデックス</param>
    public void PlayPreview(int index)
    {
        if (_previewCoroutine != null)
        {
            StopCoroutine(_previewCoroutine);
        }

        _previewCoroutine = StartCoroutine(PreviewRoutine(index));
    }

    /// <summary>
    /// プレビュー再生処理
    /// </summary>
    /// <param name="index">現在選択中のインデックス</param>
    /// <returns>IEnumeratorを返す</returns>
    private IEnumerator PreviewRoutine(int index)
    {
        var data = _songDB.songData[index];
        string songNamre = data.songName;

        //未キャッシュなら非同期ロード
        if(!_clipCache.ContainsKey(data.songName))
        {
            var req = Resources.LoadAsync<AudioClip>("Musics/" + data.songName);
            yield return req;

            var loadedClip = req.asset as AudioClip;

            _clipCache[songNamre] = loadedClip;

        }

        var clip = _clipCache[data.songName];

        //AudioSource未取得 or clipがnullなら終了
        if(_audio == null || clip == null)
        {
            yield break;
        }

        if(_audio.clip == clip)
        {
            yield break;
        }

        _audio.Stop();

        _audio.clip = clip;

        //プレビュー開始位置(50秒)
        _audio.time = 50.0f;
        _audio.Play();
    }
}
