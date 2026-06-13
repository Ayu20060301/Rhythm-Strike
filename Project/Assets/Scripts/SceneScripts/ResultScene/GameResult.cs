using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム結果画面の管理
/// </summary>
public class GameResult : MonoBehaviour
{
    [Header("Audio / Animation")]
    [SerializeField]
    private ResultBGM _resultBGM; //リザルト用のBGM
    [SerializeField]
    private PlayerAnimation _playerAnimation;

    [Header("判定UI")]
    [SerializeField]
    private TextMeshProUGUI _resultCriticalTextUI; //Critical数
    [SerializeField]
    private TextMeshProUGUI _resultHitTextUI; //Hit数
    [SerializeField]
    private TextMeshProUGUI _resultAttackTextUI; //Attack数
    [SerializeField]
    private TextMeshProUGUI _resultMissTextUI; //Miss数
    [SerializeField]
    private TextMeshProUGUI _resultCoinTextUI; //Coin数
    [SerializeField]
    private TextMeshProUGUI _resultScoreTextUI; //総合スコア
    [SerializeField]
    private TextMeshProUGUI _resultMaxComboTextUI; //最大コンボ数

    [Header("ランクUI")]
    [SerializeField]
    private Image _rankUI;

    [Header("演出時間")]
    [Tooltip("スコアのカウントアップにかかる時間")]
    private float _scoreCountDuration = 0.4f;
    [Tooltip("判定表示開始までの遅延")]
    private float _judgeDelay = 0.2f;
    [Tooltip("ランク表示までの遅延")]
    private float _rankDelay =0.3f;

    //入力受付フラグ
    private bool _iscanAcceptInput = false;

    /// <summary>
    /// ランク判定テーブル(降順)
    /// </summary>
    private readonly (int score, RankUIType rank)[] rankThresholds = new[]
    {
        (1000000,RankUIType.SSS),
        (975000,RankUIType.SS),
        (900000,RankUIType.S),
        (800000,RankUIType.A),
        (700000,RankUIType.B),
        (600000,RankUIType.C),
        (0,RankUIType.D)
    };

    private Coroutine _mainCoroutine;

    private void Start()
    {

        //BGM再生
        _resultBGM.BGMStart();

        //ランクは非表示
        _rankUI.gameObject.SetActive(false);

        //表示のための視覚的な適用
        ApplyResultVisual();

        _mainCoroutine = StartCoroutine(ShowResultSequence());
    }

    private void Update()
    {
        if (!_iscanAcceptInput) return;
        HandleInput();
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    private void HandleInput()
    {
        //フェードアウト開始したらキーを無効化する
        if (GManager.instance.isFading) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _resultBGM.StopBGM();
            SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.DECIDE);
            GManager.instance.ResetForNewGame(); //スコアなどをリセット
            SceneController.instance.SceneChange("SongSelectScene");
        }
    }

    private void ApplyResultVisual()
    {
        if (GManager.instance == null) return;

        switch(GManager.instance.playResult)
        {
            case GManager.PlayResult.CLEAR:
                _playerAnimation.ClearPlayerAnimation();
                break;
            case GManager.PlayResult.FAIL:
                _playerAnimation.FailedPlayerAnimation();
                break;
            case GManager.PlayResult.DEATH:
                _playerAnimation.FailedPlayerAnimation();
                break;
            default:
                break;
        }
    }

   

    /// <summary>
    /// リザルト演出の全体シーケンス
    /// </summary>
    private IEnumerator ShowResultSequence()
    {
        //スコアをカウントアップ
        yield return StartCoroutine(CountUpInt(_resultScoreTextUI, 0, GManager.instance.score, _scoreCountDuration));

        //判定を表示
        yield return StartCoroutine(ShowJudge());

        //ランクを表示
        yield return StartCoroutine(ShowRank());

        //入力解禁
        _iscanAcceptInput = true; 

    }


    /// <summary>
    /// 数値のカウントアップ表示
    /// </summary>
    /// <param name="text">カウントアップ表示するUIテキスト</param>
    /// <param name="from">0からカウント</param>
    /// <param name="to">ゲーム終了時のスコア等</param>
    /// <param name="duration">演出時間</param>
    private IEnumerator CountUpInt(TextMeshProUGUI text,int  from,int to,float duration)
    {

        if (text == null) yield break;

        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = t * t;
            int value = Mathf.RoundToInt(Mathf.Lerp(from, to, eased));
            text.text = value.ToString();
            yield return null;
        }

        text.text = to.ToString();
    }

   
    /// <summary>
    /// Coin用(現在値 / 最大値)
    /// </summary>
    /// <param name="text">カウントアップ表示するUIテキスト</param>
    /// <param name="from">コイン数を0からカウント</param>
    /// <param name="to">取得したコイン数</param>
    /// <param name="duration">演出時間</param>
    /// <param name="maxValue">最大コイン数</param>
    private IEnumerator CountUpInt(TextMeshProUGUI text, int from, int to, float duration, int maxValue)
    {
        if (text == null) yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int value = Mathf.RoundToInt(Mathf.Lerp(from, to, elapsed / duration));
            text.text = $"{value} / {maxValue}";
            yield return null;
        }

        text.text = $"{to} / {maxValue}";
    }

    /// <summary>
    /// 判定表示演出
    /// </summary>
    private IEnumerator ShowJudge()
    {
        yield return new WaitForSeconds(_judgeDelay);

        if (GManager.instance == null) yield break;

        //最大コンボ
        yield return StartCoroutine(CountUpInt(_resultMaxComboTextUI, 0, GManager.instance.maxCombo, 0.3f));

        //Critical
        yield return StartCoroutine(CountUpInt(_resultCriticalTextUI, 0, GManager.instance.critical, 0.3f));

        //Hit
        yield return StartCoroutine(CountUpInt(_resultHitTextUI, 0, GManager.instance.hit, 0.3f));

        //Attack
        yield return StartCoroutine(CountUpInt(_resultAttackTextUI, 0, GManager.instance.attack, 0.3f));

        //Miss
        yield return StartCoroutine(CountUpInt(_resultMissTextUI, 0, GManager.instance.miss, 0.3f));

        //Coin(現在 / 最大)
        yield return StartCoroutine(CountUpInt(_resultCoinTextUI, 0, GManager.instance.coin, 0.3f, GManager.instance.maxCoin));

    }


    /// <summary>
    /// ランク表示演出
    /// </summary>
    private IEnumerator ShowRank()
    {
        yield return new WaitForSeconds(_rankDelay);

        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.RANK);

        if(GManager.instance == null || DataBaseManager.instance == null)
        {
            yield break;
        }

        //スコアに合うランクを検索
        foreach(var threshold in rankThresholds)
        {
            if(GManager.instance.score >= threshold.score)
            {
                ShowRankUI(threshold.rank);
                break;
            }
        }
        
    }

    /// <summary>
    /// ランクUI表示
    /// </summary>
    /// <param name="type">ランクの種類</param>
    private void ShowRankUI(RankUIType type)
    {
        var rank = DataBaseManager.instance.uiDB.GetRank(type);
        _rankUI.sprite = rank.rankSprite;
        _rankUI.gameObject.SetActive(true);
    }
}
