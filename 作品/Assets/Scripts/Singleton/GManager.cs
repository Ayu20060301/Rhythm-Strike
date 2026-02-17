using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体の進行を管理するクラス
/// SingletonMonoBehaviourを継承
/// どこのクラスにもアクセス可能にする
/// </summary>
public class GManager : SingletonMonoBehaviour<GManager>
{

    [Header("進行管理")]
    public int songID;  //曲のID
    public float notesSpeed; //ノーツスピード
    public float timingOffset; //タイミングオフセット
    public bool isStart; //ゲームが開始したかどうかのフラグ
    public float startTime; //ゲーム開始時間
    public bool isDeath; //死亡したかどうかのフラグ 
    public bool isEnd; //ゲームが終了したかどうかのフラグ

    [Header("スコア関連")]
    public int score;
    public float maxScore;
    public float ratioScore; //スコアの比率

    [Header("コンボ数関連")]
    public int combo;
    public int maxCombo;

    public bool isComboBrokenOnce; //コンボが途切れたかどうかのフラグ

    [Header("判定")]
    public int critical;
    public int hit;
    public int attack;
    public int miss;
    public int coin;
    public int maxCoin;

    [Header("音量")]
    public float masterVolume;
    public float bgmVolume;
    public float seVolume;
    public float musicVolume;

    [Header("設定")]
    public bool isEffectOn;
    public bool isJudgeDisplay;

    [Header("リザルト")]
    public PlayResult playResult = PlayResult.NONE;

    public bool isPause;
    public bool isFading;

    //リザルトタイプ
    public enum PlayResult
    {
        NONE,
        CLEAR, 
        FAIL,
        DEATH
    }

    protected override void Awake()
    {
        base.Awake();

        //PlayerPrefsから音量を読み込む
        masterVolume = PlayerPrefs.GetFloat("MASTER", 100.0f) / 100.0f;
        bgmVolume = PlayerPrefs.GetFloat("BGM", 100.0f) / 100.0f;
        seVolume = PlayerPrefs.GetFloat("SE", 100.0f) / 100.0f;
        musicVolume = PlayerPrefs.GetFloat("MUSIC", 100.0f) / 100.0f;

        notesSpeed = PlayerPrefs.GetFloat("NOTE_SPEED", 12.0f);

        timingOffset = PlayerPrefs.GetFloat("TIMING_OFFSET", 0.0f);

        isJudgeDisplay = PlayerPrefs.GetInt("JUDGE_DISPLAY", 0) == 1;

        isEffectOn = PlayerPrefs.GetInt("EFFECT", 0) == 1;
    }

    public void ResetForNewGame()
    {
        isDeath = false;
        isStart = false;
        isComboBrokenOnce = false;

        score = 0;
        maxScore = 0;
        ratioScore = 0;
        critical = 0;
        hit = 0;
        attack = 0;
        miss = 0;
        coin = 0;
        combo = 0;
        maxCombo = 0;

        MusicManager.instance.ResetDeathState();

    }

}
