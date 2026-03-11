using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// スコア管理クラス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreTextUI;


    

    //スコアの加算
    public void AddScore(int points)
    {
        GManager.instance.ratioScore += points;
    }

    //スコアの更新
    public void UpdateScore()
    {
        //1010000点基準で計算
        if(GManager.instance.maxScore <= 0.0f)
        {
            GManager.instance.score = 0;
            return;
        }

        //スコア比率に基づいて101万点基準でスコアを算出
        GManager.instance.score = Mathf.RoundToInt((GManager.instance.ratioScore / GManager.instance.maxScore) * 1010000);

        //上限を超えないようにクリップ
        GManager.instance.score = Mathf.Clamp(GManager.instance.score, 0, 1010000);

        //UIの更新
        _scoreTextUI.text = GManager.instance.score.ToString();

    }

    //Critical
    public void OnCritical()
    {
        GManager.instance.critical++;
        AddScore(5);
    }

    //Hit
    public void OnHit()
    {
        GManager.instance.hit++;
        AddScore(3);
    }

    //Attack
    public void OnAttack()
    {
        GManager.instance.attack++;
        AddScore(1);
    }

    //Miss
    public void OnMiss()
    {
        GManager.instance.miss++;
    }

    //Coin
    public void OnCoin()
    {
        GManager.instance.coin++;
        AddScore(5);
    }


    public void SetMaxScore(float value)
    {
        GManager.instance.maxScore = value;
    }

    public void SetMaxCoin(int value)
    {
        GManager.instance.maxCoin = value;
    }
}
