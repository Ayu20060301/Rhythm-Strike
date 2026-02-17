using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
   
    protected override void Awake()
    {
        base.Awake();

        // Canvasごと残す
        DontDestroyOnLoad(_canvasGroup.gameObject);


        // 起動時は透明にしておく
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.gameObject.SetActive(false);
    }

    //フェードイン
    public IEnumerator FadeIn(float duration)
    {

        if (GManager.instance.isFading) yield break;

        GManager.instance.isFading = true;

        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.blocksRaycasts = true;

        if (duration <= 0.0f)
        {
            _canvasGroup.alpha = 0.0f;
        }
        else
        {
            float time = 0.0f;
            while(time < duration)
            {
                time += Time.deltaTime;
                _canvasGroup.alpha = 1.0f - (time / duration);
                yield return null;
            }
        }
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.gameObject.SetActive(false);

        GManager.instance.isFading = false;
    }

    //フェードアウト
    public IEnumerator FadeOut(float duration)
    {

        if (GManager.instance.isFading) yield break;

        GManager.instance.isFading = true;


        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.blocksRaycasts = true;

        if(duration <= 0.0f)
        {
            _canvasGroup.alpha = 1.0f;
        }
        else
        {
            float time = 0.0f;
            while(time < duration)
            {
                time += Time.deltaTime;
                _canvasGroup.alpha = time / duration;
                yield return null;
            }
        }

            _canvasGroup.alpha = 1.0f;

        GManager.instance.isFading = false;

    }
}
