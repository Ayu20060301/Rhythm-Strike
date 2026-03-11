using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongHeaderUI : MonoBehaviour
{

    //DataBaseManagerの取得
    private SongDataBase _songDB => DataBaseManager.instance.songDB;

    [SerializeField]
    private Image _JacketImage;
    [SerializeField]
    private TextMeshProUGUI _songNameText;
    [SerializeField]
    private TextMeshProUGUI _levelText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSongHeaderUI();
    }

    //ジャケットと曲名とステージ番号の更新
    void UpdateSongHeaderUI()
    {
        if (_songNameText == null || _JacketImage == null || _levelText == null) return;

        var song = _songDB.songData[GManager.instance.songID];

        //難易度取得
        var diff = song.difficultyDatas[GManager.instance.difficultyIndex];

        _JacketImage.sprite = song.songImage;

        _songNameText.text = song.songName;

        //Color →16進数に変換
        string colorCode = ColorUtility.ToHtmlStringRGB(diff.levelColor);

        _levelText.text = $"Level :  <color=#{colorCode}>{diff.levelName}</color>";
      
    }
}
