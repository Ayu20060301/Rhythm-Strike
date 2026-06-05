using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissEffectController : MonoBehaviour
{
    private float _fadeDuration = 0.4f;

    private Material _skyboxMat;


    private void Awake()
    {
        _skyboxMat = RenderSettings.skybox; 
    }


    public void PlayMissEffect()
    {
        StopAllCoroutines();
        StartCoroutine(FadeMiss());
    }

    private IEnumerator FadeMiss()
    {
        float t = 0.0f;

        while ((t < _fadeDuration))
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(1.0f, 0.0f, t / _fadeDuration);
            Shader.SetGlobalFloat("_MissIntensity", v);
            DynamicGI.UpdateEnvironment();

            yield return null;
        }

        _skyboxMat.SetFloat("_MissIntensity", 0.0f);
    }
}
