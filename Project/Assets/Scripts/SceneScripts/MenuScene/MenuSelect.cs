using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// メニューの選択操作
/// </summary>
public class MenuSelect : MonoBehaviour
{
    [SerializeField]
    private MenuBGM _menuBGM; //メニューBGMの制御クラス
    [SerializeField]
    private MenuUIController _menuUIController;
    [SerializeField]
    private TutorialController _tutorialController;

    [SerializeField]
    private Image[] _menuPanel;  //メニュー項目のパネル
    [SerializeField]
    private TextMeshProUGUI _messageText;

    //各メニューのデフォルト(初期)位置を保持する配列
    private Vector3[] _defaultPositions;

    //現在選択しているメニューのインデックス
    private int _currentIndex = 0;

    //初回効果音を鳴らさないためのフラグ
    private bool _isFirstUpdate = true;


    // Start is called before the first frame update
    void Start()
    {
        _defaultPositions = new Vector3[_menuPanel.Length];

        for(int i =0; i < _menuPanel.Length; i++)
        {
            _defaultPositions[i] = _menuPanel[i].rectTransform.localPosition;
        }

        //メニューBGMを開始
        _menuBGM.MenuBGMStart();

        //パネルの更新
        UpdateMenuPanel(_currentIndex);

        //初回はスライドせず直接表示
        Sprite initialSprite = DataBaseManager.instance.uiDB.GetMenu(GetMenuUIType(_currentIndex)).menuUI;

        _menuUIController.SetInitialite(initialSprite);

        //メッセージの更新
        UpdateText(_currentIndex);

    }

    // Update is called once per frame
    void Update()
    {
        //入力処理
        HandleInput();
    }


    /// <summary>
    /// 入力処理
    /// </summary>
    private void HandleInput()
    {

        //フェードアウト中はキーを無効化にする
        if (GManager.instance.isFading) return;

        //チュートリアル中はキーを無効化
        if (_tutorialController != null && _tutorialController.IsShowing) return;

        //下キーで選択を下に移動
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(_currentIndex < _menuPanel.Length - 1)
            {
                _currentIndex++;
                UpdateMenuPanel(_currentIndex);
            }
        }

        //上キーで選択を上に移動
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(_currentIndex > 0)
            {
                _currentIndex--;
                UpdateMenuPanel(_currentIndex);
            }
        }

        //Enterキーで決定する
        if(Input.GetKeyDown(KeyCode.Return))
        {
            DecideMenu(_currentIndex);
        }
    }

    /// <summary>
    /// メニューパネルの更新
    /// </summary>
    /// <param name="index">対象のインデックス</param>
    private void UpdateMenuPanel(int index)
    {
        for(int i = 0; i < _menuPanel.Length; i++)
        {
            //元の位置に戻す
            _menuPanel[i].rectTransform.localPosition = _defaultPositions[i];

            _menuPanel[i].color = Color.green;
        }

        //選択中だけ横にずらす
        Vector3 pos = _defaultPositions[index];
        pos.x += 100.0f; //ずらしたい量
        _menuPanel[index].rectTransform.localPosition = pos;

        //初回だけ効果音を鳴らさない
        if (!_isFirstUpdate)
        {
            //選択音再生
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.SELECT);
        }
        else
        {
            _isFirstUpdate = false; //次回以降は鳴らすようにする
        }
            //選択のパネルだけ強調する
            _menuPanel[index].color = Color.yellow;


        //MenuControllerにスクロールで切り替えを依頼
        _menuUIController.ScrollTo(
            DataBaseManager.instance.uiDB.GetMenu(GetMenuUIType(index)).menuUI
        );

        //テキストの更新
        UpdateText(index);
        
    }

    /// <summary>
    /// メッセージの更新
    /// </summary>
    /// <param name="index">選択中のインデックス</param>
    private void UpdateText(int index)
    {
        switch(index)
        {
            case 0:
                _messageText.text = "ゲームの難易度を選択します";
                break;
            case 1:
                _messageText.text = "ゲームの遊び方を説明します";
                break;
            case 2:
                _messageText.text = "ゲームの設定を変更します";
                break;
            case 3:
                _messageText.text = "タイトル画面にもどります";
                break;
            default:
                _messageText.text = string.Empty;
                break;
        }

    }

    /// <summary>
    /// Enterキーで実行する処理
    /// </summary>
    /// <param name="index">対象のインデックス</param>
    private void DecideMenu(int index)
    {
        //決定音
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.DECIDE);

        var menuData = DataBaseManager.instance.uiDB.GetMenu(GetMenuUIType(index));

        if (menuData.stopBGMOnDecide)
        {
            //決定キーを押したと同時にBGMを止める
            _menuBGM.StopBGM();
        }
        

        //現在の選択に応じた処理
        switch(index)
        {
            case 0:
                //PLAY 選択時 : ステージセレクトに遷移
                Debug.Log("PLAY");
                SceneController.instance.SceneChange("SongSelectScene");
                break;
            case 1:
                //チュートリアル 選択時 
                Debug.Log("チュートリアル");
                _tutorialController.StartTutorial();
                break;
            case 2:
                //ゲーム設定 選択時 : ゲーム設定シーンに遷移
                Debug.Log("ゲーム設定");
                OptionSceneContext.ReturnType = OptionReturnType.MENU;
                SceneController.instance.SceneChange("OptionScene");
                break;
            case 3:
                //タイトルに戻る 選択時 : タイトル画面に遷移
                Debug.Log("タイトルに戻る");
                SceneController.instance.SceneChange("TitleScene");
                break;
        }
    }
 

    /// <summary>
    /// 指定インデックスに対応するMenuUIypeを返す
    /// </summary>
    /// <param name="index">選択中のインデックス</param>
    /// <returns>対応する MenuUIType</returns>
    MenuUIType GetMenuUIType(int index)
    {
        switch(index)
        {
            case 0:
                return MenuUIType.PLAY;
            case 1:
                return MenuUIType.TUTORIAL; 
            case 2:
                return MenuUIType.OPTION;
            case 3:
                return MenuUIType.TITLE;
            default:
                return MenuUIType.PLAY;
        }
    }
}
