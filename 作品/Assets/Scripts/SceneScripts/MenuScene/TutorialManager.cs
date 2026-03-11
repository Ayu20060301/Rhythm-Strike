using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

/// <summary>
/// チュートリアルの動画の再生を管理するクラス
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer _videoPlayer; //動画再生用のVideoPlayer

    [SerializeField]
    private TextMeshProUGUI _descriptionText; //動画に関連する説明文を表示
    [SerializeField]
    private TextMeshProUGUI _tutorialDisplayName; //度のチュートリアル化を表示


    //DataBaseManagerから取得する
    private MovieDataBase _movieDB => DataBaseManager.instance.movieDataBase;

    private Dictionary<TutorialType, TutorialMovieData> _tutorialDict = new();

    
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        //MovieDataBase内のデータをDictionaryに変換
        foreach(var data in _movieDB.tutorialMovieDatas)
        {
            _tutorialDict[data.type] = data;
        }
    }

    /// <summary>
    /// 指定したチュートリアルの動画を再生する
    /// </summary>
    /// <param name="type">チュートリアルの種類</param>
    public void PlayTutorialType(TutorialType type)
    {
        //指定したTutorialTypeが存在するかチェック
        if(!_tutorialDict.TryGetValue(type, out  var data))
        {
            Debug.LogWarning($"TutorialType {type}が見つかりません");
            return;
        }

        //既に再生中の動画を停止
        _videoPlayer.Stop();

        //ループ設定を反映
        _videoPlayer.isLooping = data.loop;

        //StreamingAssets内から動画URLを生成
        _videoPlayer.url = Path.Combine(
            Application.streamingAssetsPath,
            data.moviePath
            ).Replace("\\", "/");　//Windows環境でも正しく動作するようにパスを置換

        //動画再生開始
        _videoPlayer.Play();

        //説明文を表示
        if(_descriptionText != null)
        {
            _descriptionText.text = data.descriptionName;
        }

        //チュートリアル名をUIに反映
        if (_tutorialDisplayName != null)
        {
            _tutorialDisplayName.text = data.tutorialDisplayName;
        }
    }

}
