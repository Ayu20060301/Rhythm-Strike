using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ノーツ判定を管理するクラス
/// </summary>
public class Judge : MonoBehaviour
{

    //判定の許容時間
    private const float _kCriticalTime = 0.025f; //25ms
    private const float _kHitTime = 0.05f; //50ms
    private const float _kAttackTime = 0.08f; //80ms


    //ノーツタイプ
    private const int _kNoteTypeSingle = 1; //シングルノーツ
    private const int _kNoteTypeCoin = 4; //コインノーツ

    //メッセージインデックス
    private const int _kMessageIndexCritical = 0;
    private const int _kMessageIndexHit = 1;
    private const int _kMessageIndexAttack = 2;
    private const int _kMessageIndexMiss = 3;

    [SerializeField] 
    private GameObject[] _messageObj;
    [SerializeField]
    private NotesManager _notesManager;
    [SerializeField]
    private ComboTextUI _comboTextUI;
    [SerializeField]
    private GameFlowManager _gameFlowManager;
    [SerializeField]
    private PlayerHp _playerHp;
    [SerializeField]
    private GameBar _gameBar;
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private ComboManager _comboManager;

   

    //ポーズ時間補正
    private float _totalPauseTime = 0.0f;
    private float _pauseStartTime = 0.0f;

    private bool _judgeThisFrame = false;

    //判定タイプ
    public enum JudgeType
    {
        CRITICAL,
        HIT,
        ATTACK,
        MISS,
        COIN
    }


    //初期化処理
    private void Start()
    {
        _gameFlowManager.PlayStart();

    }

    //毎フレームの更新処理
    void Update()
    {
        _judgeThisFrame = false;

        //ゲームが開始していない場合は処理をしない
        if (!GManager.instance.isStart) return;

        //HPが0になった場合は処理をしない
        if (_playerHp.currentHp <= 0) return;



        //ポーズ中は判定をやめる
        if (GManager.instance.isPause)
        {
            if (_pauseStartTime == 0.0f)
            {
                _pauseStartTime = Time.time;

            }
            return;
        }
        else
        {
            if (_pauseStartTime > 0.0f)
            {
                _totalPauseTime += Time.time - _pauseStartTime;
                _pauseStartTime = 0.0f;
            }

        }


        //各レーンのキー入力をチェック
        CheckKey(KeyCode.D, 0); //左端レーン
        CheckKey(KeyCode.F, 1); //左中レーン
        CheckKey(KeyCode.J, 2); //右中レーン
        CheckKey(KeyCode.K, 3); //右端レーン

        if (!_judgeThisFrame)
        {
            //Miss判定のチェック
            CheckMissCondition();
        }
    }



    /// <summary>
    /// Miss判定の条件チェック
    /// </summary>
    private void CheckMissCondition()
    {
        if (_notesManager.notesTime.Count == 0) return;

        int targetIndex = -1;

        //一番手前のシングルノーツを探す
        for(int i = 0; i < _notesManager.notesTime.Count; i++)
        {
            if (_notesManager.noteType[i] == _kNoteTypeSingle)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex < 0) return;

        float currentTime = Time.time - _totalPauseTime;
        float noteTime = _notesManager.notesTime[targetIndex] + GManager.instance.startTime;

        if (currentTime > noteTime + _kAttackTime)
        {
            //Miss
            HandleMiss(targetIndex);
        }
    }

    /// <summary>
    /// キー入力判定
    /// </summary>
    /// <param name="key">キーコード</param>
    /// <param name="num">対象レーン</param>
    void CheckKey(KeyCode key,int num)
    {
        //対象のキー以外のキーを押した場合は処理をしない
        if (!Input.GetKeyDown(key))  return;

        //ノーツが存在しない場合は処理をしない
        if (_notesManager.notesTime.Count == 0) return;

        int targetNoteIndex = -1;

        //シングルノーツだけを探す
        for(int i = 0; i < _notesManager.notesTime.Count; i++)
        {
            if (_notesManager.noteType[i] != _kNoteTypeSingle)
            {
                continue;
            }

            if (_notesManager.laneNum[i] == num)
            {
                targetNoteIndex = i;
                break; //一番手前のシングル
            }
        }

        if (targetNoteIndex >= 0)
        {
            //ポーズを補正して現在時間を使ってtimeLagを計算する
            float currentTime = Time.time - _totalPauseTime;
            float noteTime = _notesManager.notesTime[targetNoteIndex] + GManager.instance.startTime;
            float timeLag = Mathf.Abs(currentTime - noteTime);
            Judgement(timeLag, targetNoteIndex);
        }

    }

    /// <summary>
    /// ノーツの判定処理
    /// </summary>
    /// <param name="timeLag">たたいた時間の誤差</param>
    /// <param name="noteIndex">対象ノーツのインデックス</param>
    void Judgement(float timeLag, int noteIndex)
    {
        //判定範囲外の場合は処理をしない
        if (timeLag > _kAttackTime) return;

        //インデックスの範囲チェック
        if (!IsValidNoteIndex(noteIndex)) return;

        //シングルノーツ以外は処理をしない
        int noteType = _notesManager.noteType[noteIndex];
        if (noteType != _kNoteTypeSingle) return;

        JudgeType judgeType;

        //判定の決定
        if (timeLag <= _kCriticalTime + GManager.instance.timingOffset * 0.01f)
        {
            //Critical
            judgeType = JudgeType.CRITICAL;
        }
        else if(timeLag <= _kHitTime + GManager.instance.timingOffset * 0.01f)
        {
            //Hit
            judgeType = JudgeType.HIT;
        }
        else if(timeLag <= _kAttackTime + GManager.instance.timingOffset * 0.01f)
        {
            //Attack
            judgeType = JudgeType.ATTACK;
        }
        else
        {
            return;
        }
        HandleHit(judgeType, noteIndex);
    }

    /// <summary>
    /// ヒット時の処理
    /// </summary>
    /// <param name="type">判定タイプ</param>
    /// <param name="noteIndex">対象ノーツのインデックス</param>
    private void HandleHit(JudgeType type,int noteIndex)
    {

        _judgeThisFrame = true;

        //インデックスの範囲チェック
        if (!IsValidNoteIndex(noteIndex)) return;

        //ヒット音の再生
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.HIT);

        //ノーツの位置を取得する
        Vector3 notePos = _notesManager.notesObj[noteIndex].transform.position;

        //ヒットエフェクトの再生
        ParticleManager.instance.PlayParticle(ParticleType.HIT_EFFECT, notePos, Quaternion.identity);

        //ノーツタイプに応じた1ノーツ当たりのゲージ割合
        float gaugeAmount = 0.0f;

        int noteType = _notesManager.noteType[noteIndex];
        switch(noteType)
        {
            case _kNoteTypeSingle:
                gaugeAmount = 1.0f / _notesManager.singleNoteNum;
                break;
            case _kNoteTypeCoin:
                gaugeAmount = 1.0f / _notesManager.coinNoteNum;
                break;
        }

        switch(type)
        {
            case JudgeType.CRITICAL:
                gaugeAmount *= 1.0f;
                break;
            case JudgeType.HIT:
                gaugeAmount *= 1.0f;
                break;
            case JudgeType.ATTACK:
                gaugeAmount *= 0.4f;
                break;
        }

        //ゲージの更新
        _gameBar.AddGauge(gaugeAmount);

        //スコアとコンボの更新
        switch (type)
        {
            case JudgeType.CRITICAL:
                _scoreManager.OnCritical();
                Message(_kMessageIndexCritical, noteIndex);
                break;
            case JudgeType.HIT:
                _scoreManager.OnHit();
                Message(_kMessageIndexHit, noteIndex);
                break;
            case JudgeType.ATTACK:
                _scoreManager.OnAttack();
                Message(_kMessageIndexAttack, noteIndex);
                break;
            case JudgeType.MISS:
                _scoreManager.OnMiss();
                Message(_kMessageIndexMiss, noteIndex);
                break;
        }

        //コンボの加算
        _comboManager.AddCombo();


        //ノーツの削除
        RemoveNoteData(noteIndex);

    }

   /// <summary>
   /// ミスの処理
   /// </summary>
   /// <param name="noteIndex">対象ノーツのインデックス</param>
    private void HandleMiss(int noteIndex)
    {
        _judgeThisFrame = true;

        //インデックスの範囲チェック
        if (!IsValidNoteIndex(noteIndex)) return;

        int noteType = _notesManager.noteType[noteIndex];

        float gaugeAmount = 0.0f;

        //シングルノーツ以外は無視
        if (noteType != _kNoteTypeSingle) return;

        gaugeAmount = 1.0f / _notesManager.singleNoteNum;

        //ゲージの更新
        _gameBar.DecreaseGauge(gaugeAmount);

        //5ダメージ
        _gameFlowManager.OnHit(5);

        _scoreManager.OnMiss();
        _comboManager.ResetCombo();


        //メッセージの表示
        Message(_kMessageIndexMiss, noteIndex);

        //ノーツの削除
        RemoveNoteData(noteIndex);


    }




    /// <summary>
    /// ノーツインデックスが有効かチェック
    /// </summary>
    /// <param name="index">チェックするインデックス</param>
    /// <returns>有効ならtrue</returns>
    private bool IsValidNoteIndex(int index)
    {
        return index >= 0 &&
               index < _notesManager.notesTime.Count &&
               index < _notesManager.laneNum.Count &&
               index < _notesManager.noteType.Count;
    }

    /// <summary>
    /// ノーツを削除してスコアを更新
    /// </summary>
    /// <param name="noteIndex">削除するノーツのインデックス</param>
    void RemoveNoteData(int noteIndex)
    {

        //インデックスの範囲チェック
        if (!IsValidNoteIndex(noteIndex)) return;

        bool isSingle = _notesManager.noteType[noteIndex] == _kNoteTypeSingle;

        //シングルノーツだけオブジェクトを消す
        if (isSingle)
        {
            if (_notesManager.notesObj[noteIndex] != null)
            {
                Destroy(_notesManager.notesObj[noteIndex]);
            }

        }

        //データリストから削除
        _notesManager.notesObj.RemoveAt(noteIndex);
        _notesManager.notesTime.RemoveAt(noteIndex);
        _notesManager.laneNum.RemoveAt(noteIndex);
        _notesManager.noteType.RemoveAt(noteIndex);


        //スコアの更新
        _scoreManager.UpdateScore();

        //コンボの更新
        _comboTextUI.UpdateComboUI();
        
    }

    /// <summary>
    /// メッセージを表示させる
    /// </summary>
    /// <param name="judgeIndex">判定インデックス</param>
    /// <param name="noteIndex">対象ノーツのインデックス</param>
    void Message(int judgeIndex,int noteIndex)
    {

        if (!GManager.instance.isJudgeDisplay) return;

        //配列の範囲チェック
        if (judgeIndex < 0 || judgeIndex >= _messageObj.Length) return;
        if (!IsValidNoteIndex(noteIndex)) return;

        int noteType = _notesManager.noteType[noteIndex];
        float laneX = (_notesManager.laneNum[noteIndex] - 1.5f);
        Vector3 spawnPos = new Vector3(laneX, 0.76f, 0.15f);

        GameObject prefabToSpawn = _messageObj[judgeIndex];

        //ノーツのタイプごとにメッセージ表示させる
        switch(noteType)
        {
            case _kNoteTypeSingle:
                prefabToSpawn = _messageObj[judgeIndex];
                break;
            case _kNoteTypeCoin:
                prefabToSpawn = _messageObj[judgeIndex];
                break;
            default:
                prefabToSpawn = _messageObj[judgeIndex];
                break;
        }

        Instantiate(prefabToSpawn, spawnPos, Quaternion.Euler(45, 0, 0));
    }
}
