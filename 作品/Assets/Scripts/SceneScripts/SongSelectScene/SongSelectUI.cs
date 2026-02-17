using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 楽曲選択画面のUI表示を管理するクラス
/// </summary>
public class SongSelectUI : MonoBehaviour
{
    [Header("スクリプト参照")]
    [SerializeField]
    private LevelAnimator _levelAnim;

    [Header("Song Info UI")]
    [SerializeField]
    private TextMeshProUGUI[] _songText;
    [SerializeField]
    private TextMeshProUGUI[] _artistText;
    [SerializeField]
    private Image[] _songImages;

    [Header("Panel UI")]
    [SerializeField]
    private Image[] _panelImages;
    [SerializeField]
    private RectTransform _content;
    [SerializeField]
    private RectTransform[] _panelRects;

    //外部から渡される楽曲データ
    private SongDataBase _songDB;

    //スクロール用ターゲット位置
    private Vector2 _targetPos;

    //SmoothDamp用速度保持変数
    private Vector2 _velocity;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="db">楽曲データベース</param>
    public void Initialize(SongDataBase db)
    {
        _songDB = db;

        //パネル数とデータ数の小さいほうまでループ
        for (int i = 0; i < _panelRects.Length && i < db.songData.Count; i++)
        {
            var data = db.songData[i];

            _songText[i].text = data.songName;
            _artistText[i].text = data.artistName;
            _songImages[i].sprite = data.songImage;

            _panelImages[i].material = Instantiate(_panelImages[i].material);

            _panelImages[i].material.SetColor("_MainColor1", data.levelColor);
        }

    }

    /// <summary>
    /// 現在選択されているインデックスをUIに反映する
    /// </summary>
    /// <param name="index">現在のインデックス</param>
    public void SetIndex(int index)
    {
        UpdatePosition(index);
        UpdateUI(index);
    }

    private void Update()
    {
        _content.anchoredPosition = Vector2.SmoothDamp(
            _content.anchoredPosition,
            _targetPos,
            ref _velocity,
            0.2f
            );
    }

    /// <summary>
    /// パネルの位置の更新
    /// </summary>
    /// <param name="index">現在のインデックス</param>
    private void UpdatePosition(int index)
    {
        _targetPos = -_panelRects[index].anchoredPosition;
    }

    /// <summary>
    /// UIの更新
    /// </summary>
    /// <param name="index">現在のインデックス</param>
    private void UpdateUI(int index)
    {
        var data = _songDB.songData[index];

        //難易度表示の更新
        _levelAnim.Play(data.levelName, data.levelColor);
    }
}
