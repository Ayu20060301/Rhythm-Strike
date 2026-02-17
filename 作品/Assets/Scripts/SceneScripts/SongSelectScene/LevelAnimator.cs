using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 難易度名を縦スライドして切り替える演出クラス
/// </summary>
public class LevelAnimator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelText;

    //スライドする距離
    private float _slideDistance = 60.0f;

    //スライドアニメーションの時間
    private float _duration = 0.15f; 

    //テキストの基準位置
    private Vector2 _basePos;

    //再生中のコルーチン管理用
    private Coroutine _animCoroutine;

    //フェード制御用
    private CanvasGroup _canvasGroup;

    //RectTransformキャッシュ
    private RectTransform _rect;


    private void Awake()
    {
        //RectTransformを取得
        _rect = _levelText.rectTransform;

        //初期位置を基準位置として保存
        _basePos = _rect.anchoredPosition;

        _canvasGroup = _levelText.GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = _levelText.gameObject.AddComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// 難易度名を更新してスライド演出を再生
    /// </summary>
    /// <param name="levelName">表示するレベル名</param>
    public void Play(string levelName,Color color)
    {
        //既に再生中のアニメーションがなければ停止
        if (_animCoroutine != null)
        {
            StopCoroutine(_animCoroutine);
        }

        //表示状態をリセット
        _rect.anchoredPosition = _basePos;
        _canvasGroup.alpha = 1.0f;

        //スライドアニメーション開始
        _animCoroutine = StartCoroutine(SlideCoroutine(levelName,color));
    }

    /// <summary>
    /// 縦スライドでテキストを切り替えるコルーチン
    /// </summary>
    /// <param name="levelName">表示するレベル名</param>
    /// <param name="color">変更するテキストの色</param>
    /// <returns>テキストを縦スライドさせるコルーチン用のIEnumerator</returns>
    private IEnumerator SlideCoroutine(string levelName,Color color)
    {
        //開始位置
        Vector2 startPos = _basePos;

        //上に消える位置
        Vector2 outPos = startPos + Vector2.up * _slideDistance;
 
        //下から出てくる位置
        Vector2 inPos = startPos - Vector2.up * _slideDistance;

        float t = 0.0f;

        //上へスライドして消える
        while (t < _duration)
        {
            t += Time.deltaTime;
            float r = t / _duration;

            _rect.anchoredPosition = Vector2.Lerp(startPos, outPos, r);
            _canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, r);
           
            yield return null;
        }

        //テキスト更新
        _levelText.text = levelName;

        //色の変更
        _levelText.color = color;

        //下から出てくる準備
        t = 0.0f;
        _rect.anchoredPosition = inPos;
        _canvasGroup.alpha = 0.0f;

        //下からスライドして出る
        while (t < _duration)
        {
            t += Time.deltaTime;
            float r = t / _duration;

            _rect.anchoredPosition = Vector2.Lerp(inPos, startPos, r);
            _canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, r);
           
            yield return null;
        }

        //最終位置・表示状態を保証
        _rect.anchoredPosition = _basePos;
        _canvasGroup.alpha = 1.0f;

        //再生終了
        _animCoroutine = null;
    }
}
