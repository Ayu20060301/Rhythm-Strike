using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    private float _speed = 5.0f; //点滅速度
    private float _time; //点滅時間

    [SerializeField]
    private TextMeshProUGUI _blinkText;

    void Update()
    {
        _blinkText.color = GetTextColorAlpha(_blinkText.color);
    }

    /// <summary>
    /// 色を取得する
    /// </summary>
    /// <param name="color">指定する色</param>
    /// <returns>指定した色を返す</returns>
    Color GetTextColorAlpha(Color color)
    {
        _time += Time.deltaTime * _speed;

        //Sin波を0～1に変換
        color.a = (Mathf.Sin(_time) + 1.0f) * 0.5f;

        return color;
    }
}
