using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// シーンを管理するクラス
/// SingletonMonoBehaviourを継承している
/// </summary>
public class SceneController : SingletonMonoBehaviour<SceneController>
{
    public BaseScene currentScene;
  
    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Awake()
    {
        //60フレーム
        Application.targetFrameRate = 60;
        base.Awake();
    }

    /// <summary>
    /// シーンチェンジ
    /// </summary>
    /// <param name="sceneName">シーンの名前</param>
    public void SceneChange(string sceneName)
    {
        StartCoroutine(SceneChangeCoroutine(sceneName));
    }

    /// <summary>
    /// シーン遷移のコルーチン
    /// </summary>
    /// <param name="sceneName">シーンの名前</param>
    /// <returns>遷移させるコルーチンを返す</returns>
    private IEnumerator SceneChangeCoroutine(string sceneName)
    {
        //フェードアウト
        yield return FadeManager.instance.FadeOut(0.5f);

        //シーンを非同期ロード
        yield return LoadingManager.instance.LoadScene(sceneName);

        //ロード完了後、シーンをアクティブ化
        LoadingManager.instance.ActivateScene();
        yield return null;

        //新しくロードされたシーンのBaseSceneを取得
        currentScene = FindObjectOfType<BaseScene>();
       
        //フェードイン
        yield return FadeManager.instance.FadeIn(0.6f);
    }
}
