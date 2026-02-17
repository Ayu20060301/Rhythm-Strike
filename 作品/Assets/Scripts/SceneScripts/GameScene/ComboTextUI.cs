using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboTextUI : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _comboTextUI;

    private int _prevCombo = 0;

    private Coroutine _comboBounceCoroutine;

    private Vector3 _initialScale;

    //外部プロパティでアクセス可能
    public TextMeshProUGUI comboTextUI => _comboTextUI;


    private void Awake()
    {
        _initialScale = this._comboTextUI.transform.localScale;
    }


    private void Start()
    {
        UpdateComboUI();
    }

    /// <summary>
    /// UIの更新
    /// </summary>
    public void UpdateComboUI()
    {
        if (GManager.instance == null) return;

        int currentCombo = GManager.instance.combo;

        //コンボ表示
        if (currentCombo > 0)
        {
            _comboTextUI.text = currentCombo + "\n<size=40%>Combo!</size>";

            //コンボが増えた瞬間だけ弾ませる
            if (currentCombo > _prevCombo)
            {
                PlayCombobounce();
            }
        }
        else
        {
            _comboTextUI.text = "";
        }

        _prevCombo = currentCombo;

        //---コンボ色制御---

        //critical継続中ならテキスト色を黄色にする
       if(GManager.instance.hit == 0 &&
          GManager.instance.attack == 0 &&
          GManager.instance.miss == 0
          )
       {
            _comboTextUI.color = Color.yellow; //黄色
       }

       
       if(GManager.instance.hit > 0 ||
          GManager.instance.attack > 0
          )
       {
            _comboTextUI.color = new Color(1.0f,0.6f,0.0f); //オレンジ色
       }

       //コンボが一度でも途切れた場合は色を赤色(少し暗め)にする
       if(GManager.instance.isComboBrokenOnce)
       {
            _comboTextUI.color = new Color(1.0f,0.3f,0.2f); 
       }

       //--------------------

       if(!GManager.instance.isStart)
        {
            _comboTextUI.text = " ";
        }

    }

    private void PlayCombobounce()
    {
        if (_comboBounceCoroutine != null)
        {
            StopCoroutine(_comboBounceCoroutine);
        }
        _comboBounceCoroutine = StartCoroutine(ComboBounceRoutine());
    }


    private IEnumerator ComboBounceRoutine()
    {

       
        Vector3 bounceScale = _initialScale * 1.1f;

        float duration = 0.1f;
        float time = 0.0f;

        //拡大
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            this._comboTextUI.transform.localScale =
                Vector3.Lerp(_initialScale, bounceScale, EaseOut(t));
            yield return null;

        }

        //縮小
        time = 0.0f;
        while(time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            this._comboTextUI.transform.localScale =
                Vector3.Lerp(bounceScale, _initialScale, t);
            yield return null;
        }

        this._comboTextUI.transform.localScale = _initialScale;
    }

    private float EaseOut(float t)
    {
        return 1.0f - Mathf.Pow(1.0f - t, 2.0f);
    }
}
