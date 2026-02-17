using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBar : MonoBehaviour
{
    [SerializeField]
    private Image _currentGauge; 
    [SerializeField]
    private Image _clearLine; 

    private float _clearLineValue = 0.7f;


    private float _currentValue = 0.0f; 
    private float _targetValue = 0.0f; 
    private float _speed = 2.0f;

    private bool _isFull = false;
    private float _hue = 0.0f;

    public float GaugeValue => _targetValue;



    private void Update()
    {
        _currentGauge.fillAmount = Mathf.Lerp(_currentGauge.fillAmount, _targetValue, Time.deltaTime * _speed);

        //ゲージが満タンになったら虹色にする
        if(_targetValue >= 1.0f)
        {
            _isFull = true;
        }
        else
        {
            _isFull = false;
        }

        //虹色アニメーション
        if(_isFull)
        {
            _hue += Time.deltaTime; //色相を少しずつずらす
            if (_hue > 1.0f) _hue = 0.0f;
            _currentGauge.color = Color.HSVToRGB(_hue, 1.0f, 1.0f);
        }

    }

    /// <summary>
    /// ゲージの上昇
    /// </summary>
    /// <param name="amount">上昇割合</param>
    public void AddGauge(float amount)
    {
        _targetValue = Mathf.Clamp01(_targetValue + amount);
    }

    /// <summary>
    /// ゲージの減少
    /// </summary>
    /// <param name="amount">ゲージの減少割合</param>
    public void DecreaseGauge(float amount)
    {
        _targetValue = Mathf.Clamp01(_targetValue - amount);
    }
}
