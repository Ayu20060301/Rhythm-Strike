using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイ中の進行を管理するスクリプト
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    [Header("スクリプト参照")]
    [SerializeField]
    private ReadyTextAnimation _readyTextAnimation;
    [SerializeField]
    private StartTextAnimation _startTextAnimation;
    [SerializeField]
    private PlayerHp _playerHp;
    [SerializeField]
    private GameBar _gameBar;
    [SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private PlayerAnimation _playerAnimation;
    [SerializeField]
    private CameraShake _cameraShake;
    [SerializeField]
    private MissEffectController _missEffect;


    [Header("バッジ表示")]
    [SerializeField]
    private GameObject _badgeObj;
    private SpriteRenderer _badgeRenderer;

    [Header("チューニング")]
    //Ready表示からStartまでの待機時間
    private float _readyDuration = 1.5f;
    //Start表示のフェード/待機時間
    private float _startFadeDuration = 2.0f;
    //結果バッジ(クリア/失敗してからの待機時間)
    private float _resultBadgeShowDuration = 2.0f;
   
    [Header("ゲームプレイ設定")]
    [Tooltip("受けるダメージ")]
    private int _hitDamage = 20;
    [Tooltip("クリア判定に用いる値")]
    private float _clearGaugeThrehold = 0.75f;

    //外部アクセス用プロパティ
    private bool _isPlayActive = false;
    public bool isPlayActive => _isPlayActive;

    private bool _isEnding = false;

    //コルーチン参照
    private Coroutine _startCoroutine;
    private Coroutine _damageCoroutine;
    private Coroutine _endCoroutine;

    //プレイ終了通知イベント
    public event System.Action<GManager.PlayResult> OnPlayEnded;


    private void Start()
    {
        //バッジのSpriteRenderer取得
        _badgeRenderer = _badgeObj.GetComponent<SpriteRenderer>();

        //PlayerHpの死亡通知を購読
        _playerHp.OnPlayerDeath += OnPlayerDead;
    }


    private void OnDestroy()
    {
        //イベント解除
        if(_playerHp != null)
        {
            _playerHp.OnPlayerDeath -= OnPlayerDead;
        }

        //コルーチン停止
        StopCoroutineIfRunning(ref _startCoroutine);
        StopCoroutineIfRunning(ref _damageCoroutine);
        StopCoroutineIfRunning(ref _endCoroutine);
    }

    private void OnDisable()
    {
        //シーン切り替えや無効化時にもコルーチンを止める
        StopCoroutineIfRunning(ref _startCoroutine);
        StopCoroutineIfRunning(ref _damageCoroutine);
        StopCoroutineIfRunning(ref _endCoroutine);
    }

    private void StopCoroutineIfRunning(ref Coroutine coroutine)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }


    private void OnPlayerDead()
    {
        if (!_isPlayActive || _isEnding) return;

        GManager.instance.isEnd = true;

        //終了処理は一度だけ開始する
        if (_endCoroutine == null)
        {
          _endCoroutine = StartCoroutine(EndPlayGame(GManager.PlayResult.DEATH));
        }
    }

    /// <summary>
    /// 曲終了通知
    /// </summary>
    public void OnSongFinished()
    {
        if (_isEnding) return;

        Debug.Log("GameFlowManager: 曲終了");

        GManager.instance.isEnd = true;
        CheckPlayEnd();
    }

    /// <summary>
    /// 外部にゲーム開始処理を呼び出す関数
    /// </summary>
    public void PlayStart()
    {
        if (_isPlayActive) return;

        //既に開始シーケンス中なら停止して再開
        StopCoroutineIfRunning(ref _startCoroutine);
        _startCoroutine = StartCoroutine(PlayStartSequence());
    }

    /// <summary>
    /// 開始時のコルーチン
    /// </summary>
    /// <returns>開始時に再生させるコルーチン用のIEnumerator</returns>
    private IEnumerator PlayStartSequence()
    {
        //走行アニメーションの開始
        _playerAnimation.RunAnimation();

        //Ready → Start!演出
        _readyTextAnimation.PlayReadyAnimation();
        yield return new WaitForSeconds(_readyDuration);

        _startTextAnimation.PlayFadeOut();
        yield return new WaitForSeconds(_startFadeDuration);

        //曲の再生
        MusicManager.instance.PlayFromStart();

        //ゲーム開始フラグ類をセット
        if(GManager.instance != null)
        {
            GManager.instance.isStart = true;
            GManager.instance.playResult = GManager.PlayResult.NONE;
        }

        _isPlayActive = true;

        _startCoroutine = null;

        Debug.Log("開始");
    }


   /// <summary>
   /// 障害物や敵に当たった場合の処理
   /// </summary>
   /// <param name="damage">ダメージ量</param>
    public void OnHit(int damage)
    {
        if (!isPlayActive) return;

        //ダメージ適用
        _playerHp.TakeDamage(damage);

        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.MISS);

        //カメラが揺れる
        _cameraShake.Shake(0.15f, 0.12f);

        //多重再生防止
        if(_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
        }

        //ダメージアニメーションを再生
        _playerAnimation.DamageAnimation();

        //空が赤寄りに一瞬変わる
        _missEffect.PlayMissEffect();

    }

    /// <summary>
    /// 終了チェック
    /// </summary>
    public void CheckPlayEnd()
    {
        if (_isEnding) return;

        float gauge = _gameBar.GaugeValue;
        var result = gauge >= _clearGaugeThrehold ? GManager.PlayResult.CLEAR : GManager.PlayResult.FAIL;
        StartEndPlay(result);
    }


    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    /// <param name="result">リザルトの種類</param>
    private void StartEndPlay(GManager.PlayResult result)
    {
        if (_isEnding) return;

        _endCoroutine = StartCoroutine(EndPlayGame(result));
    }


    /// <summary>
    /// 終了時に開始するコルーチン
    /// </summary>
    /// <param name="result">リザルトタイプ</param>
    /// <returns>終了時に開始させるコルーチン用のIEnumerator</returns>
    private IEnumerator EndPlayGame(GManager.PlayResult result)
    {
        _isEnding = true;
        _isPlayActive = false;

        //ゲーム状態を落とす
        if (GManager.instance != null)
        {
            GManager.instance.isStart = false;
            GManager.instance.isEnd = true;
            GManager.instance.playResult = result;
        }
        
        if(result == GManager.PlayResult.DEATH)
        {
            HandleDeath();
            yield break;
        }

        yield return HandleBadges(result);

        SceneController.instance.SceneChange("ResultScene");
        OnPlayEnded?.Invoke(result);
        _endCoroutine = null;
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    private void HandleDeath()
    {
        Debug.Log("死亡");
        _playerHp.Death();

        //死亡後の処理を再生
        StartCoroutine(DeathSequence());
    }

    /// <summary>
    /// 死亡後に開始するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(2.0f);
        ShowBadge(ResultBadgeType.FAILED);
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.FAILED);
        yield return new WaitForSeconds(_resultBadgeShowDuration);
     
        SceneController.instance.SceneChange("ResultScene");
        OnPlayEnded?.Invoke(GManager.PlayResult.DEATH);
        _endCoroutine = null;
    }


  
    private IEnumerator HandleBadges(GManager.PlayResult result)
    {
        //All Critical
        if (IsAllCritical())
        {
            ShowBadge(ResultBadgeType.ALLCRITICAL);
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.AC);
        }

        //Full Combo
        else if (!GManager.instance.isComboBrokenOnce)
        {
            ShowBadge(ResultBadgeType.FULLCOMBO);
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.FC);
        }

        yield return new WaitForSeconds(2.0f);

        _badgeObj.SetActive(false);

        //結果ごとの処理
        switch (result)
        {
            case GManager.PlayResult.CLEAR: 
                ShowBadge(ResultBadgeType.STAGECLEAR);
                SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.CLEAR);
                break;
            case GManager.PlayResult.FAIL:
                ShowBadge(ResultBadgeType.FAILED);
                SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.FAILED);
                break;
        }

        yield return new WaitForSeconds(_resultBadgeShowDuration);
    }


    /// <summary>
    /// All Criticalかどうかのフラグ
    /// </summary>
    /// <returns>miss、hit、attack数が0かつ一回も障害物に衝突してない状態で返す</returns>
    private bool IsAllCritical()
    {
        var gm = GManager.instance;
        return gm.miss == 0 && gm.hit == 0 && gm.attack == 0 && !gm.isComboBrokenOnce;
    }

    /// <summary>
    /// バッジの表示
    /// </summary>
    /// <param name="type">バッジの種類</param>
    public void ShowBadge(ResultBadgeType type)
    {
        var badge = DataBaseManager.instance.uiDB.GetBadge(type);
        _badgeRenderer.sprite = badge.badgeSprite;
        _badgeObj.SetActive(true);
    }
}
