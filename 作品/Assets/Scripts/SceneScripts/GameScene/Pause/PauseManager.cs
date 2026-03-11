using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private PauseOverlay _pauseOverlay;
    [SerializeField]
    private StartTextAnimation _startTextAnimation;

    [SerializeField]
    private GameObject _pauseFrame;
    [SerializeField]
    private Image[] _image;

    [SerializeField]
    private TextMeshProUGUI _countDownText;

    [SerializeField]
    private TextMeshProUGUI[] _selectText;

    private int _index = 0;

    private bool _isCanUseEsc = true; //ESCを受け付けていい状態か

    private Vector3 _currentScale;
    private Vector3 _bigScale;

    // Start is called before the first frame update
    void Start()
    {
        GManager.instance.isPause = false;
        _index = 0;
        _pauseFrame.SetActive(false);
        _countDownText.text = " ";

        _currentScale = Vector3.one;
        _bigScale = Vector3.one * 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームが開始していない場合は処理をしない
        if (!GManager.instance.isStart) return;

        HandleInput();
        
    }

    
    /// <summary>
    /// 入力処理
    /// </summary>
    private void HandleInput()
    {

        //フェードアウト中 or ESC無効中
        if (GManager.instance.isFading) return;

        //ポーズ中でない場合
        if(!GManager.instance.isPause)
        {
            if(_isCanUseEsc && Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
            return;
        }

        //左右キーでボタン移動
        int previousIndex = _index;

        //ESCキーを押したらポーズ中にする
        if (_isCanUseEsc && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            return;
        }

        //---------------------------
        //左右キー
        //---------------------------
        if (Input.GetKeyDown(KeyCode.UpArrow) && _index > 0)
        {
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.SELECT);
            _index--;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && _index < _image.Length - 1)
        {
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.SELECT);
            _index++;
        }

        //選択が変わったら見た目更新
        if (previousIndex != _index)
        {
            HighLightButton();
        }

        //Enterキー
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ButtonUpdate();
        }

        //ボタンの見た目更新
        HighLightButton();
    }

    /// <summary>
    /// ポーズ切り替え
    /// </summary>
    private void TogglePause()
    {

        //ゲームが開始してない場合は無効
        if (!GManager.instance.isStart) return;

        //ポーズ状態を反転
        GManager.instance.isPause = !GManager.instance.isPause;

        //ポーズフレーム表示切り替え
        _pauseFrame.SetActive(GManager.instance.isPause);

        //ESCキーを入力する際の効果音を再生
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CANCEL);

        //Overlayの表示切替
        if (_pauseOverlay != null)
        {
            if(GManager.instance.isPause)
            {
                //ポーズ開始時
                _pauseOverlay.ShowOverlay();

                //Esc連打防止
                _isCanUseEsc = false; 
            }
            else
            {
                //ポーズ解除
                _pauseOverlay.HideOverlay();
                _isCanUseEsc = true;
            }
        }

    }

    /// <summary>
    /// ボタンの見た目更新
    /// </summary>
    void HighLightButton()
    {
        for(int i =0; i < _image.Length; i++)
        {
            bool isSelected = (i == _index);

            //選択中のみ水色、それ以外は白
            _image[i].color = (i == _index) ? new Color(0,191,255) : Color.gray;

            //選択中のみ大きくする
            _image[i].transform.localScale = (i == _index) ? _bigScale : _currentScale;

            _selectText[i].color = (i == _index) ? Color.yellow : Color.white;


            //シェーダーのSelectedフラグ更新
            if (_image[i].material.HasProperty("_Selected"))
            {
                _image[i].material.SetFloat("_Selected", isSelected ? 1.0f : 0.0f);
            }
        }
    }

    /// <summary>
    /// ボタンの決定処理
    /// </summary>
    void ButtonUpdate()
    {
        //決定音
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.DECIDE);

        switch(_index)
        {
            case 0: //続ける       
              
                if (_pauseOverlay != null)
                {
                    _pauseOverlay.HideOverlay();
                }

                _pauseFrame.SetActive(false);
                StartCoroutine(CountDownCoroutine());
                break;

            case 1: //最初から
                GManager.instance.isStart = false;
                SceneController.instance.SceneChange("GameScene");
                GManager.instance.ResetForNewGame();
                break;

            case 2: //セレクトシーンに戻る
                GManager.instance.isStart = false;
                SceneController.instance.SceneChange("SongSelectScene");
                GManager.instance.ResetForNewGame();
                break;

        }

    }

    /// <summary>
    /// カウントダウン復帰処理
    /// </summary>
    private IEnumerator CountDownCoroutine()
    {
        
        float countDown = 3.0f;

        _isCanUseEsc = false;

        //ポーズ中はノーツと曲を止める
        GManager.instance.isPause = true;
        GManager.instance.isStart = false;

        while(countDown > 0)
        {
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.COUNT);

            _countDownText.text = Mathf.Ceil(countDown).ToString();
            yield return new WaitForSecondsRealtime(1.0f);
            countDown -= 1.0f;
        }

        //表示リセット
        _countDownText.text = " ";
        _startTextAnimation.PlayFadeOut();

        yield return new WaitForSeconds(1.5f);

        //ゲーム再開
        GManager.instance.isPause = false;
        GManager.instance.isStart = true;

        _isCanUseEsc = true;

        //曲の再開
        MusicManager.instance.UnPause();

    }
}
