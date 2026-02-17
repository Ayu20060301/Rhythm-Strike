using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メニューの右側の大きなUI画像を下から上にスクロールで切り替える
/// </summary>
public class MenuUIController : MonoBehaviour
{
    [SerializeField]
    private Image _menuUI;

    private Image _nextImage;

    private float _slideDistance = 20.0f;
    private float _duration = 0.15f;

    private Vector3 _menuUIInitialPosition;

    private Coroutine _slideCoroutine;

    private bool _isInitialized = false;

    private void Awake()
    {
        _menuUIInitialPosition = _menuUI.rectTransform.localPosition;
    }

    /// <summary>
    /// 初期表示
    /// </summary>
    /// <param name="sprite">表示する画像</param>
    public void SetInitialite(Sprite sprite)
    {

        if(_slideCoroutine != null)
        {
            StopCoroutine(_slideCoroutine);
            _slideCoroutine = null;
        }
        if(_nextImage != null)
        {
            Destroy(this._nextImage.gameObject);
            _nextImage = null;
        }

        if(_menuUI != null)
        {
            _menuUI.sprite = sprite;
            _menuUIInitialPosition = _menuUI.rectTransform.localPosition;
            _menuUI.color = Color.white;
        }

        _isInitialized = true;
    }

    /// <summary>
    /// スクロール切り替え
    /// </summary>
    /// <param name="newSprite">新しい画像</param>
    public void ScrollTo(Sprite newSprite)
    {

        //初回はアニメーション市内
        if(!_isInitialized)
        {
            SetInitialite(newSprite);
            return;
        }

        //スライド中であれば、古い画像を即座に終了
        if(_slideCoroutine != null)
        {
            StopCoroutine(_slideCoroutine);
        }

        //途中で残ったNextUIを削除
        if(_nextImage != null)
        {
            Destroy(_nextImage.gameObject);
            _nextImage = null;
        }

        _slideCoroutine = StartCoroutine(SlideCoroutine(newSprite));
    }

    /// <summary>
    /// UIが下から上にスライドするコルーチン
    /// </summary>
    /// <param name="newSprite">新しい画像</param>
    /// <returns>スライドさせるコルーチン用のIEnumerator</returns>
    private IEnumerator SlideCoroutine(Sprite newSprite)
    {
        //前の画像
        Image oldImage = _menuUI;

        //新しい画像を生成して下に配置
        GameObject go = new GameObject("NextUI", typeof(Image));
        Image newImage = go.GetComponent<Image>();
        _nextImage = newImage;


        newImage.sprite = newSprite;
        newImage.rectTransform.SetParent(_menuUI.transform.parent);
        newImage.rectTransform.SetAsLastSibling(); //描画順を最前綿に
        newImage.rectTransform.localScale = Vector3.one;


        //RectTransformコピー
        RectTransform oldRect = oldImage.rectTransform;
        RectTransform newRect = newImage.rectTransform;

        //サイズとアンカーをコピー
        newRect.sizeDelta = oldRect.sizeDelta;
        newRect.anchorMin = oldRect.anchorMin;
        newRect.anchorMax = oldRect.anchorMax;
        newRect.pivot = oldRect.pivot;

        Vector3 startPos = _menuUIInitialPosition;
        Vector3 outPos = startPos + Vector3.up * _slideDistance;
        Vector3 inPos = startPos - Vector3.up * _slideDistance;

        newRect.localPosition = inPos;

        CanvasGroup oldCanvas = oldImage.GetComponent<CanvasGroup>();
        CanvasGroup newCanvas = newImage.GetComponent<CanvasGroup>();

        if (oldCanvas == null) oldCanvas = oldImage.gameObject.AddComponent<CanvasGroup>();
        if (newCanvas == null) newCanvas = newImage.gameObject.AddComponent<CanvasGroup>();

        oldCanvas.alpha = 1.0f;
        newCanvas.alpha = 0.0f;

        float t = 0.0f;

        while(t < _duration)
        {
            t += Time.deltaTime;
            float r = Mathf.Clamp01(t / _duration);

            oldRect.localPosition = Vector3.Lerp(startPos,outPos,r);
            newRect.localPosition = Vector3.Lerp(inPos,startPos,r);

            oldCanvas.alpha = Mathf.Lerp(1.0f, 0.0f, r);
            newCanvas.alpha = Mathf.Lerp(0.0f, 1.0f, r);

            yield return null;

        }

        //最終調整
        newRect.localPosition = startPos;
        newCanvas.alpha = 1.0f;

        oldImage.rectTransform.localPosition = startPos;
        oldCanvas.alpha = 1.0f;

        //古い画像は削除
        if (oldImage != null)
        {
            Destroy(oldImage.gameObject);
        }

        //新しい画像を_menuUIに置き換え
        _menuUI = newImage;
        _nextImage = null;
        _slideCoroutine = null; //Coroutine終了フラグ
    }
}
