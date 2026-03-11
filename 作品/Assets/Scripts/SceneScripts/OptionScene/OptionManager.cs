using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オプション画面全体を管理するクラス
/// 各オプションの保存処理や、戻る操作を制御する
/// </summary>
public class OptionManager : MonoBehaviour
{
    //音量設定のオプション
    [SerializeField]
    private VolumeOption _volumeOption;

    //ノーツ速度設定のオプション
    [SerializeField]
    private NotesOption _noteOption;

    //エフェクト設定のオプション
    [SerializeField]
    private EffectOption _effectOption;

    //タイミング設定のオプション
    [SerializeField]
    private TimingOption _timingOption;

    //判定表示設定のオプション
    [SerializeField]
    private JudgeDisplayOption _judgeDisPlayOption;

    //オプション画面用BGM
    [SerializeField]
    private OptionBGM _optionBGM;

    //シーン名を集中管理
    private const string MenuSceneName = "MenuScene";
    private const string StageSelectSceneName = "SongSelectScene";

    private void Start()
    {
        _optionBGM.BGMStart();  
    }

    private void Update()
    {
        //ESCキーが押されたらメニューに戻る
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //フェード中はUpdate内の処理を無視する
            if (GManager.instance.isFading) return;

            //設定の保存
            SaveAllOptions();

            //効果音の再生
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CANCEL);

            //BGM OFF
            _optionBGM.BGMStop();

            //遷移元のシーンに移動
            switch(OptionSceneContext.ReturnType)
            {
                //メニューシーン
                case OptionReturnType.MENU:
                    SceneController.instance.SceneChange(MenuSceneName);
                    break;
                //ステージシーン
                case OptionReturnType.SELECT:
                    SceneController.instance.SceneChange(StageSelectSceneName);
                    break;

                default:
                    //保険としてメニューシーンに戻す
                    SceneController.instance.SceneChange(MenuSceneName);
                    break;
            }
        }
    }

    /// <summary>
    /// 各オプションの設定を保存
    /// </summary>
    private void SaveAllOptions()
    {
        _volumeOption.SaveSettings();
        _noteOption.SaveSettings();
        _effectOption.SaveSettings();
        _timingOption.SaveSettings();
        _judgeDisPlayOption.SaveSettings();
    }
}
