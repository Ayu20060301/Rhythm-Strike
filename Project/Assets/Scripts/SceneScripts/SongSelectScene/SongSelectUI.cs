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

            //マテリアル複製
            _panelImages[i].material = Instantiate(_panelImages[i].material);

        }

    }

    /// <summary>
    /// 現在選択されているインデックスをUIに反映する
    /// </summary>
    /// <param name="index">現在のインデックス</param>
    public void SetIndex(int index)
    {
        UpdatePosition(index);
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
        index = Mathf.Clamp(index, 0, _panelRects.Length - 1);
        _targetPos = -_panelRects[index].anchoredPosition;
    }


    /// <summary>
    /// 難易度表示の更新
    /// </summary>
    /// <param name="diff"></param>
    public void UpdateDifficulty(DifficultyData diff)
    {
        if (diff == null) return;

        //レベル表示
        _levelAnim.Play(diff.levelName,diff.levelColor);

        //パネル色変更
        foreach(var img in _panelImages)
        {
            img.material.SetColor("_MainColor1", diff.levelColor);
        }
    }

}
