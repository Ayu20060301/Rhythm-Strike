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

        var stageData = _songDB.songData[GManager.instance.songID]; 

        _JacketImage.sprite = stageData.songImage;

        _songNameText.text = stageData.songName;

        //Color →16進数に変換
        string colorCode = ColorUtility.ToHtmlStringRGB(stageData.levelColor);

        _levelText.text = $"Level :  <color=#{colorCode}>{stageData.levelName}</color>";
      
    }
}
