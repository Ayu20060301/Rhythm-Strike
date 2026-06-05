using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 「Start!」テキストのフェードアウト演出を管理するクラス
/// </summary>
public class StartTextAnimation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _startText;

    private string _originalText; //元のテキスト内容


    /// <summary>
    /// 初期化処理
    /// テキスト設定と、全長・カラーを透明にする
    /// </summary>
    private void Awake()
    {
        _originalText = "Start!";
        _startText.text = _originalText;

        //メッシュ情報を即時更新
        _startText.ForceMeshUpdate();

        //最初は透明にする
        TMP_TextInfo textInfo = _startText.textInfo;
        for(int i = 0; i <textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int meshIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            Color32[] newVertexColors = textInfo.meshInfo[meshIndex].colors32;

            for(int j = 0; j < 4; j++)
            {
                newVertexColors[vertexIndex + j].a = 0; //透明に
            }
        }

        //カラー情報のみ更新
        _startText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    /// <summary>
    /// フェードアウト演出を開始
    /// </summary>
    public void PlayFadeOut()
    {
        StartCoroutine(FadeOutAnimation());
    }

    /// <summary>
    /// 「Start!」テキストを透明にするコルーチン
    /// </summary>
    /// <returns>コルーチンを返す</returns>
    private IEnumerator FadeOutAnimation()
    {
        //テキストの再設定
        _startText.text = _originalText;
        _startText.ForceMeshUpdate();

        yield return null;

        //効果音の再生
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.START2);


        TMP_TextInfo textInfo = _startText.textInfo;
        int charCount = textInfo.characterCount;

        //表示文字がなければ終了
        if (charCount == 0) yield break;

        float duration = 0.6f;
        float t = 0.0f;

        //元の色を取得
        Color32[] newVertexColors;
        Color32 c0;

        //フェードアウト処理
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            for (int i = 0; i < charCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible) continue;

                int meshIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                newVertexColors = textInfo.meshInfo[meshIndex].colors32;

                //アルファを減らす
                c0 = newVertexColors[vertexIndex];
                byte alpha = (byte)Mathf.Lerp(255, 0, normalized);

                //1文字分の(4頂点)分のアルファ更新
                for (int j = 0; j < 4; j++)
                {
                    newVertexColors[vertexIndex + j].a = alpha;
                }

            }

            //カラー更新反映
            _startText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return null;
        }
    }
}
