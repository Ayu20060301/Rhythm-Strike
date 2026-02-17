using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

/// <summary>
/// シーンロード中に進捗表示とフェード演出を管理するクラス
/// </summary>
public class LoadingManager : SingletonMonoBehaviour<LoadingManager>
{
    [SerializeField]
    private CanvasGroup _canvas; //ロード画面全体(フェード制御用)
    [SerializeField]
    private Image _bar;　//進捗バー

    private AsyncOperation _currentOp; //現在のシーンロード処理


    protected override void Awake()
    {
        base.Awake();

        //シーンをまたいでロード画面を保持する
        if(_canvas != null)
        {
            DontDestroyOnLoad(_canvas.gameObject);
        }

        //初期状態では非表示
        _canvas.gameObject.SetActive(false);

    }

    /// <summary>
    /// ロード開始時の初期化処理
    /// </summary>
    private void Begin()
    {
        _canvas.gameObject.SetActive(true);
        _canvas.alpha = 1.0f;
        SetProgress(0.0f);
        GManager.instance.isFading = true;
    }

    /// <summary>
    /// 指定したシーンを非同期でロードする
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <returns>コルーチンを返す</returns>
    public IEnumerator LoadScene(string sceneName)
    {
        Begin();

        float minTime = 4.0f; //最低表示時間
        float elapsed = 0.0f;

        //非同期ロード開始
        _currentOp = SceneManager.LoadSceneAsync(sceneName);
        _currentOp.allowSceneActivation = false;

        //-------------------------
        //進捗バー演出(0.0→0.9)
        //-------------------------
        float progressTime = 0.0f;
        float targetTime = minTime * 0.5f; //前半演出時間


        while(progressTime < targetTime)
        {
            progressTime += Time.deltaTime;
            SetProgress(Mathf.Lerp(0.0f,0.9f,progressTime / targetTime));
            elapsed += Time.deltaTime;
            yield return null;
        }

        //シーンのロード進捗に合わせて残り0.9→1.0
        while(_currentOp.progress < 0.9f)
        {
            yield return null; 
        }

        //--------------------------
        //余韻演出(0.9→1.0)
        //--------------------------
        float t = 0.0f;
        targetTime = minTime * 0.5f;

        while(t < targetTime)
        {
            t += Time.deltaTime;
            SetProgress(Mathf.Lerp(0.9f, 1.0f, t / targetTime));
            elapsed += Time.deltaTime;
            yield return null;
        }

        //最低表示時間を待つ
        if(elapsed < minTime)
        {
            yield return new WaitForSeconds(minTime - elapsed);
        }

        //--------------------------
        //フェードアウト演出
        //--------------------------
        float fadeTime = 0.5f;
        float tFade = 0.0f;

        while(tFade < fadeTime)
        {
            tFade += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, tFade / fadeTime);
            _canvas.alpha = alpha;
            yield return null;
        }

        //ロード画面完了
        Finish();

        //シーン切り替えを許可
        ActivateScene();

    }

    /// <summary>
    /// ロード画面終了処理
    /// </summary>
    private  void Finish()
    {
        _canvas.gameObject.SetActive(false);
        GManager.instance.isFading = false;
    }

    /// <summary>
    /// シーンのアクティベーションを許可する
    /// </summary>
    public void ActivateScene()
    {
        if(_currentOp != null)
        {
            _currentOp.allowSceneActivation = true;
        }
    }

    /// <summary>
    /// 進捗バーの更新
    /// </summary>
    /// <param name="value">増加量</param>
    private void SetProgress(float value)
    {
        _bar.fillAmount = Mathf.Clamp01(value);
    }
}
