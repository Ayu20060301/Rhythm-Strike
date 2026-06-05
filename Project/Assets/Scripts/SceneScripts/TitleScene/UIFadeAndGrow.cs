using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadeAndGrow : MonoBehaviour
{
    [SerializeField]
    private Material _uiMat;

    private float _fadeTime = 1.0f;  //フェード時間
    private float _glowTime = 1.0f; //光らせる時間
    private float _maxGlow = 3.0f; //輝き度

    //他クラスから演出完了を参照可能
    public bool IsFinished { get; private set; } = false;

    private void Start()
    {
        StartCoroutine(PlayCoroutine());
    }

    /// <summary>
    /// UIを一瞬輝かせるコルーチン
    /// </summary>
    /// <returns>IEnumeratorを返す</returns>
    private IEnumerator PlayCoroutine()
    {
        //フェードイン
        float t = 0.0f;
        _uiMat.SetFloat("_Alpha", 0);
        _uiMat.SetFloat("_Glow", 0);

        while (t < _fadeTime)
        {
            t += Time.deltaTime;
            _uiMat.SetFloat("_Alpha", t / _fadeTime);
            yield return null;
        }
        _uiMat.SetFloat("_Alpha", 1.0f);

        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.GLOW);

        //一瞬だけ輝かせる
        t = 0;
        while (t < _glowTime)
        {
           
            t += Time.deltaTime;
            float g = Mathf.Sin((t / _glowTime) * Mathf.PI);
            _uiMat.SetFloat("_Glow", g * _maxGlow);
            yield return null;
        }
        _uiMat.SetFloat("_Glow", 0);

        //演出終了
        IsFinished = true;

    }
}
