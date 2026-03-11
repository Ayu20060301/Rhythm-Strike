using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JudgeManager : MonoBehaviour
{
    [SerializeField]
    private NotesManager _notesManager;
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private ComboManager _comboManager;
    [SerializeField]
    private GameBar _gameBar;
    [SerializeField]
    private PlayerHp _playerHp;
    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private MessageManager _messageManager;

    private JudgeCore _judgeCore = new JudgeCore();

    private float _totalPauseTime = 0.0f;
    private float _pauseStartTime = 0.0f;

    private const int NOTE_TYPE_SINGLE = 1;


    private void Start()
    {
        _inputHandler.OnLaneInput += OnInput; 
    }


    private void OnDestroy()
    {
        _inputHandler.OnLaneInput -= OnInput;
    }

    private void Update()
    {

        if (!GManager.instance.isStart) return;
        if (_playerHp.currentHp <= 0.0f) return;

        CheckMiss();
    }

    private void OnInput(int lane)
    {

        int notesIndex = FindNotes(lane);
        if (notesIndex < 0) return;

        float currentTime = Time.time - _totalPauseTime;
        float notesTime = _notesManager.notesTime[notesIndex] + GManager.instance.startTime;

        float timeLag = Mathf.Abs(currentTime - notesTime);

        var result = _judgeCore.GetJudge(timeLag, GManager.instance.timingOffset);

        ApplyResult(result, notesIndex);
    }

   /// <summary>
   /// ノーツを探す
   /// </summary>
   /// <param name="lane">各レーン</param>
   /// <returns>対象のノーツを返す</returns>
    private int FindNotes(int lane)
    {
        for(int i = 0; i < _notesManager.notesTime.Count; i++)
        {

            if (_notesManager.notesTime[i] != NOTE_TYPE_SINGLE) continue;
            if (_notesManager.laneNum[i] == lane) return i;
        }

        return -1;
    }


    /// <summary>
    /// Miss判定
    /// </summary>
    private void CheckMiss()
    {
        if (_notesManager.notesTime.Count == 0) return;

        int index = 0;

        if (_notesManager.noteType[index] != NOTE_TYPE_SINGLE) return;

        float currentTime = Time.time - _totalPauseTime;
        float notesTime = _notesManager.notesTime[index] + GManager.instance.startTime;

        if (currentTime > notesTime + 0.08f)
        {
            ApplyResult(JudgeType.MISS, index);
        }
    }

    /// <summary>
    /// 判定結果の適用
    /// </summary>
    /// <param name="result">判定の種類</param>
    /// <param name="notesIndex">対象ノーツのインデックス</param>
    private void ApplyResult(JudgeType result, int notesIndex)
    {

        switch(result)
        {
            case JudgeType.CRITICAL:
                _scoreManager.OnCritical();
                _comboManager.AddCombo();
                _gameBar.AddGauge(1.0f);
                break;

            case JudgeType.HIT:
                _scoreManager.OnHit();
                _comboManager.AddCombo();
                _gameBar.AddGauge(1.0f);
                break;

            case JudgeType.ATTACK:
                _scoreManager.OnAttack();
                _comboManager.AddCombo();
                _gameBar.AddGauge(0.4f);
                break;

            case JudgeType.MISS:
                _scoreManager.OnMiss();
                _comboManager.ResetCombo();
                _gameBar.DecreaseGauge(1.0f);
                _playerHp.TakeDamage(5);
                break;
        }

        //レーン番号を渡す
        int lane = _notesManager.laneNum[notesIndex];
        _messageManager.ShowMessage(result, notesIndex);

        RemoveNotes(notesIndex);
    }

    /// <summary>
    /// ノーツデータの削除
    /// </summary>
    /// <param name="index">対象ノーツのインデックス</param>
    private void RemoveNotes(int index)
    {
        if (_notesManager.notesObj[index] != null)
        {
            Destroy(_notesManager.notesObj[index]);
        }

        _notesManager.notesObj.RemoveAt(index);
        _notesManager.notesTime.RemoveAt(index);
        _notesManager.laneNum.RemoveAt(index);
        _notesManager.noteType.RemoveAt(index);
    }
}
