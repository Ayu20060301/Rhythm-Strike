using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{

    [SerializeField]
    private TitleBGM _titleBGM;
    [SerializeField]
    private GameExit _gameExit;
    [SerializeField]
    private UIFadeAndGrow _uiFadeAndGrow;

    private void Start()
    {
        _titleBGM.BGMStart();
    }

    // Update is called once per frame
    void Update()
    {
        //フェードアウト中ならキーを無効化にする
        if (GManager.instance.isFading) return;

        //演出が終わるまでキーを無効化する
        if (!_uiFadeAndGrow.IsFinished) return;


        //Enterキーを入力したらメニューシーンに遷移する
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.DECIDE);
            SceneController.instance.SceneChange("MenuScene");
        }

        //ESCキーを入力したらゲームを終了する
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _gameExit.Exit();
        }
    }
}
