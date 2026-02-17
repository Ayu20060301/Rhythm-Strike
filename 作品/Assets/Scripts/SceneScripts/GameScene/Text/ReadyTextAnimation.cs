using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 「Ready?」テキストの演出を管理するクラス
/// </summary>
public class ReadyTextAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _readyText;
    [SerializeField] private float _charDelay = 0.1f; //文字ごとの表示間隔
    [SerializeField] private Vector3 startOffset = new Vector3(-50f, 0f, 0f); //開始時のずらし量

    private string _originalText; //表示する元のテキスト

    /// <summary>
    /// 初期化処理
    /// テキスト設定と、全長・カラーを透明にする
    /// </summary>
    private void Awake()
    {
        _originalText = "Ready?";
        _readyText.text = _originalText;

        //TextMeshProのメッシュ情報を即時更新
        _readyText.ForceMeshUpdate();

        //最初に全文字透明にする
        var textInfo = _readyText.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            //非表示文字はスキップ
            if (!textInfo.characterInfo[i].isVisible) continue;

            int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertIndex = textInfo.characterInfo[i].vertexIndex;
            var colors = textInfo.meshInfo[matIndex].colors32;
            
            //1文字=4頂点分のアルファを0にする
            for (int j = 0; j < 4; j++)
            {
                colors[vertIndex + j].a = 0;
            }
        }
    }


    /// <summary>
    /// 演出開始
    /// </summary>
    public void PlayReadyAnimation()
    {
        StartCoroutine(ReadySequence());
    }

    /// <summary>
    /// 演出全体の流れ
    /// </summary>
    /// <returns>コルーチンを返す</returns>
    private IEnumerator ReadySequence()
    {
        //文字表示演出
        yield return StartCoroutine(ShowTextCoroutine());

        //少し表示を残してから消す
        yield return new WaitForSeconds(0.8f);
        _readyText.text = " ";
    }


    /// <summary>
    /// 文字を順番に表示するコルーチン
    /// </summary>
    /// <returns>コルーチンを返す</returns>
    private IEnumerator ShowTextCoroutine()
    {

        yield return null;

        //最初の文字のフェードイン開始と同時に音を流す
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.START1);

        var textInfo = _readyText.textInfo;

        // 文字を透明にする
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            var colors = textInfo.meshInfo[materialIndex].colors32;

            for (int j = 0; j < 4; j++)
                colors[vertexIndex + j].a = 0;
        }

        //カラー情報のみ更新
        _readyText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        //文字を1文字ずつフェードイン
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;
            StartCoroutine(FadeInCharacter(i));
            yield return new WaitForSeconds(_charDelay);
        }
    }

    /// <summary>
    /// 指定した文字をフェードイン＋スライド表示するコルーチン
    /// </summary>
    /// <param name="charIndex"></param>
    /// <returns>コルーチンを返す</returns>
    private IEnumerator FadeInCharacter(int charIndex)
    {
        var textInfo = _readyText.textInfo;

        int materialIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
        int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;

        Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
        Color32[] colors = textInfo.meshInfo[materialIndex].colors32;

        // 元の頂点位置を保存
        Vector3[] origVertices = new Vector3[4];
        for (int i = 0; i < 4; i++)
            origVertices[i] = vertices[vertexIndex + i];

        float t = 0f;
        float duration = 0.3f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

           //位置をお布施とから元に戻す
            Vector3 offset = Vector3.Lerp(startOffset, Vector3.zero, normalized);

            for (int i = 0; i < 4; i++)
            {
                //頂点位置更新
                vertices[vertexIndex + i] = origVertices[i] + offset;

                //アルファ更新
                var c = colors[vertexIndex + i];
                c.a = (byte)Mathf.Lerp(0, 255, normalized);
                colors[vertexIndex + i] = c;
            }

            //頂点・カラーを反映
            _readyText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            yield return null;
        }
    }

    /// <summary>
    /// 全文字をフェードアウトさせる処理    
    /// </summary>
    /// <returns>コルーチンを返す</returns>
    private IEnumerator FadeOutAll()
    {
        var textInfo = _readyText.textInfo;
        float t = 0.0f;
        float duration = 0.4f;

        //現在の頂点カラーを取得
        Color32[][] allColors = new Color32[textInfo.meshInfo.Length][];
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            allColors[i] = textInfo.meshInfo[i].colors32;
        }

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            byte alpha = (byte)Mathf.Lerp(255, 0, normalized);

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible) continue;

                int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertIndex = textInfo.characterInfo[i].vertexIndex;

                for (int j = 0; j < 4; j++)
                {
                    var c = allColors[matIndex][vertIndex + j];
                    c.a = alpha;
                    allColors[matIndex][vertIndex + j] = c;
                }
            }

            _readyText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return null;

        }
    }
}
