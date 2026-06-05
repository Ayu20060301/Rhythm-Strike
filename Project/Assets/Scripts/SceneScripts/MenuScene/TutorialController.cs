using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアルの進行を管理するクラス
/// </summary>
public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private TutorialManager _tutorialManager;

    [SerializeField]
    private GameObject _tutorialRoot; //チュートリアルUIのオブジェクト
    
    //チュートリアルの進行順
    private TutorialType[] _flow =
    {
        TutorialType.NOTES, 
        TutorialType.JUMPNOTES,
        TutorialType.DAMAGENOTES,
    };

    private int _currentIndex; //現在のインデックス

    //チュートリアルが表示中かどうか
    public bool IsShowing { get; private set; }


    private void Update()
    {
        HandleInput(); //入力処理を毎フレーム実行
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    void HandleInput()
    {
        //チュートリアル中のみキーを処理
        if (!IsShowing) return;

        //→キーで次のチュートリアルへ
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.SELECT);
            OnNext();
        }

        //←キーで前のチュートリアルへ
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.SELECT);
            OnBack();
        }

        //Escキーを入力してチュートリアルを終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CANCEL);
            EndTutorial();
        }
    }

    /// <summary>
    /// チュートリアルを開始
    /// </summary>
    public void StartTutorial()
    {
        IsShowing = true;
        _currentIndex = 0;
        _tutorialRoot.SetActive(true);
        _tutorialManager.PlayTutorialType(_flow[_currentIndex]);
    }

    /// <summary>
    /// 次のチュートリアルへ進む
    /// </summary>
    public void OnNext()
    {
        _currentIndex++;

        //最後のチュートリアルで止める
        if(_currentIndex >= _flow.Length)
        {
            _currentIndex = _flow.Length - 1; //最後のインデックスに戻す
            return;
        }


        _tutorialManager.PlayTutorialType(_flow[_currentIndex]);
    }

    /// <summary>
    /// 前のチュートリアルに戻る
    /// </summary>
    private void OnBack()
    {
        _currentIndex--;

        if(_currentIndex < 0)
        {
            _currentIndex = 0;
            return;
        }

        _tutorialManager.PlayTutorialType(_flow[_currentIndex]);
    }

    /// <summary>
    /// チュートリアルを終了
    /// </summary>
    public void EndTutorial()
    {
        _tutorialRoot.SetActive(false);
        IsShowing = false;

    }
}
