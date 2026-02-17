using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 楽曲選択画面の制御を行うクラス
/// </summary>
public class SongSelectController : MonoBehaviour
{
    //楽曲データベース(シングルトンから取得)
    private SongDataBase _songDB => DataBaseManager.instance.songDB;

    [SerializeField]
    private SongSelectUI _songSelectUI;　//UI表示処理

    [SerializeField]
    private SongPreviewPlayer _songPreviewPlayer; //プレビュー音再生管理

    //現在選択中のインデックス
    private int _selectIndex = 0;

    private void Start()
    {
        //UI初期化
        _songSelectUI.Initialize(_songDB);
        _songSelectUI.SetIndex(_selectIndex);

        //プレビュー再生初期化
        _songPreviewPlayer.Initialize(_songDB);
        _songPreviewPlayer.PlayPreview(_selectIndex);
    }

    private void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// キー入力処理
    /// </summary>
    private void HandleInput()
    {
        //フェード中は操作不可
        if (GManager.instance.isFading) return;

        //右キー:次の楽曲へ
        if(Input.GetKeyDown(KeyCode.RightArrow) && _selectIndex < _songDB.songData.Count - 1)
        {
            ChangeIndex(1);
        }

        //左キー:前の楽曲へ
        if(Input.GetKeyDown(KeyCode.LeftArrow) && _selectIndex > 0)
        {
            ChangeIndex(-1);
        }

        //Enterキー:ゲーム開始
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SongStart();
        }

        //Escキー:Menuに戻る
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CANCEL);
            SceneController.instance.SceneChange("MenuScene");
        }

        //Oキー:オプション画面に遷移
        if(Input.GetKeyDown(KeyCode.O))
        {
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.DECIDE);
            OptionSceneContext.ReturnType = OptionReturnType.SELECT;
            SceneController.instance.SceneChange("OptionScene");
        }

    }

    /// <summary>
    /// インデックスを変更し,UIと音声を変更する
    /// </summary>
    /// <param name="dir">+1 or -1</param>
    private void ChangeIndex(int dir)
    {
        _selectIndex += dir;

        //範囲外防止
        _selectIndex = Mathf.Clamp(_selectIndex, 0, _songDB.songData.Count - 1);

        //UIの更新
        _songSelectUI.SetIndex(_selectIndex);
        
        //プレビュー音再生
        _songPreviewPlayer.PlayPreview(_selectIndex);

        //SE再生
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.SELECT);
    }

    /// <summary>
    /// 楽曲決定処理
    /// </summary>
    private void SongStart()
    {
        GManager.instance.songID = _selectIndex;
       
        //プレビューで使用しているAudioClipを引き継ぐ
        MusicManager.instance.SetAudioClip(_songPreviewPlayer.CurrentClip);
        
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.DECIDE);

        SceneController.instance.SceneChange("GameScene");
    }
}
