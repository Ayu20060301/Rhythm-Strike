using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ポーズ中に表示する半透明オーバーレイのフェード制御
/// </summary>
public class PauseOverlay : MonoBehaviour
{

    [SerializeField]
    private Image _pauseOverlay;

    private Coroutine _fadeCoroutine;
    
    //ポーズON: フェードイン
    public void ShowOverlay(float duration = 0.2f)
    {
        if(_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _pauseOverlay.gameObject.SetActive(true);
        _fadeCoroutine = StartCoroutine(FadeOverlay(true, duration));
    }

    //ポーズOFF: フェードアウト
    public void HideOverlay(float duration = 0.2f)
    {
        if(_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeOverlay(false,duration));
    }

    //フェードアウト・フェードイン共通
   private IEnumerator FadeOverlay(bool fadeIn,float duration)
    {
        float start = fadeIn ? 0.0f : _pauseOverlay.color.a; //現在のアルファから開始
        float end = fadeIn ? 0.5f : 0.0f;
        float t = 0.0f;

        while(t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, Mathf.Clamp01(t / duration));
            Color c = _pauseOverlay.color;
            c.a = alpha;
            _pauseOverlay.color = c;
            yield return null;
        }

        //最終値
        Color final = _pauseOverlay.color;
        final.a = end;
        _pauseOverlay.color = final;

        //フェードアウト完了後に非表示にする
        if(!fadeIn)
        {
            _pauseOverlay.gameObject.SetActive(false);
        }

        _fadeCoroutine = null;

    }
}
