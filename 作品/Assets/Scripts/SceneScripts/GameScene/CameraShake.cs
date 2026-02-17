using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _defaultPos;
    private Coroutine _shakeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _defaultPos = this.transform.localPosition;
    }

    public void Shake(float duration,float power)
    {
        if(_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }
        
        _shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, power));
    }

    IEnumerator ShakeCoroutine(float duration,float power)
    {
        float time = 0.0f;

        while(time < duration)
        {
            float x = Random.Range(-2.0f, 2.0f) * power;
            float y = Random.Range(-2.0f, 2.0f) * power;
            this.transform.localPosition = _defaultPos + new Vector3(x, y, 0.0f);

            time += Time.deltaTime;
            yield return null;
        }

        this.transform.localPosition = _defaultPos;
        _shakeCoroutine = null;
    }
}
