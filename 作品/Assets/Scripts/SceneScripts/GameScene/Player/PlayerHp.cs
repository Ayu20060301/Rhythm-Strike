using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHp : MonoBehaviour
{
    [SerializeField]
    private Image _currentImage; //現在のHPを表すゲージ
    [SerializeField]
    private Image _redImage; //ダメージ受けたときに遅れて追従するゲージ
    [SerializeField]
    private TextMeshProUGUI _hpText; //Hpテキスト

    [SerializeField]
    private PlayerAnimation _playerAnimation;


    private int _currentHp; //現在のHP
    private int _maxHp = 300; //最大HP
    private float _lerpSpeed = 2.0f; //赤ゲージが追いつく速さ


    //死亡イベント
    public event Action OnPlayerDeath;

    //プロパティで外部アクセス可能
    public int currentHp => _currentHp;
    public int maxHp => _maxHp;
    public float hpRatio => _maxHp > 0 ? (float)_currentHp / _maxHp : 0.0f;
    public bool isDead => _currentHp <= 0;


    private void Start()
    {
        Initialize(_maxHp);
    }


    //毎フレームの更新処理
    private void Update()
    {
        if (_redImage == null || _currentImage == null) return;

        if(_redImage.fillAmount > _currentImage.fillAmount)
        {
            //ダメージ受けた場合は徐々に追いつく
            _redImage.fillAmount = Mathf.Lerp(_redImage.fillAmount
                                              , _currentImage.fillAmount
                                              , Time.deltaTime * _lerpSpeed);
        }
        else if (_redImage.fillAmount < _currentImage.fillAmount)
        {
            // 回復等で現在ゲージが増えた場合は赤ゲージを瞬時に合わせる（遅延追従はダメージ用）
            _redImage.fillAmount = _currentImage.fillAmount;
        }
    }

    /// <summary>
    /// HP初期化
    /// </summary>
    /// <param name="hp">現在のHP</param>
    private void Initialize(int hp)
    {

        _maxHp = hp;
        _currentHp = _maxHp;
        UpdateGauge();
    }


    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">受けるダメージ量</param>
    public void TakeDamage(int damage)
    {
        if (isDead) return; //既に死亡している場合は処理をしない



        int oldHp = _currentHp;
        _currentHp -= damage;
        _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);

        UpdateGauge();

        if(_currentHp <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// HP回復処理
    /// </summary>
    /// <param name="heal">回復量</param>
    public void Heal(int heal)
    {
        if (isDead) return; //死亡後は回復しない

        _currentHp += heal;
        _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);

        UpdateGauge();

    }

    /// <summary>
    /// ゲージの見た目更新
    /// </summary>
    void UpdateGauge()
    {
        if(_hpText != null)
        {
            //数値の表示
            _hpText.text = $"{_currentHp}";
        }
        if(_currentImage != null)
        {
            //現在のゲージの更新
            _currentImage.fillAmount = hpRatio;
        }
    }


    /// <summary>
    /// 死亡時の処理
    /// </summary>
    public void Death()
    {
        Debug.Log("死んだ");

        GManager.instance.isDeath = true;
        
        _playerAnimation.Death();

        //GameFlowManagerに死亡を通知
        OnPlayerDeath?.Invoke();
    }

}
