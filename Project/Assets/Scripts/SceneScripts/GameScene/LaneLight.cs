using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーン入力時に光るライト演出
/// </summary>
public class LaneLight : MonoBehaviour
{
    private float _fadeSpeed = 3.0f; //ライトの消えるスピード

    [SerializeField]
    private int _laneIndex = 0; //レーン番号

    private float _maxAlpha = 0.8f; //最大アルファ値

    private float _currentAlpha = 0.0f; //現在のアルファ値
    private Renderer _rend;
    private KeyCode _bindKey;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        _bindKey = GetKeyFromLane(_laneIndex);

        //最初はアルファ値を0にしておく
        _currentAlpha = 0.0F;
        UpdateColor();
    }

    void Update()
    {
        HandleInput();
        FadeOut();
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetKeyDown(_bindKey))
        {
            LightOn();
        }
    }

    /// <summary>
    /// ライトを点灯
    /// </summary>
    private void LightOn()
    {
        _currentAlpha = _maxAlpha;
        UpdateColor();
    }

    /// <summary>
    /// フェードアウト処理
    /// </summary>
    private void FadeOut()
    {
        if (_currentAlpha <= 0.0f) return;

        _currentAlpha -= _fadeSpeed * Time.deltaTime;
        _currentAlpha = Mathf.Max(_currentAlpha, 0.0f);

        UpdateColor();
    }

    /// <summary>
    /// マテリアルのアルファを更新
    /// </summary>
    private void UpdateColor()
    {
        Color color = _rend.material.color;
        color.a = _currentAlpha;
        _rend.material.color = color;
    }

    /// <summary>
    /// レーン番号のキーを取得
    /// </summary>
    /// <param name="lane">レーン番号</param>
    /// <returns>レーン番号のキーを返す</returns>
    private KeyCode GetKeyFromLane(int lane)
    {
        return lane switch
        {
            0 => KeyCode.D,
            1 => KeyCode.F,
            2 => KeyCode.J,
            3 => KeyCode.K,
            _ => KeyCode.None
        };
    }
}
