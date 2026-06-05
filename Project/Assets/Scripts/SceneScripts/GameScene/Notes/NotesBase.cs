using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ.GameObjectExtensions;

/// <summary>
/// ノーツの基底クラス
/// Awakeで参照をキャッシュしてStartで初期値を決定
/// </summary>
public class NotesBase : MonoBehaviour
{
    //実行時に決定されるノーツの速度
    protected float _noteSpeed;
   
    //ヒット二重判定フラグ
    protected bool _isHit; 


    protected NotesManager _notesManager;
    protected GameFlowManager _gameFlowManager;
    protected PlayerHp _playerHp;
    protected ScoreManager _scoreManager;
    protected ComboManager _comboManager;
    protected ComboTextUI _comboTextUI;

    //プレイヤー本体
    protected GameObject _player;

    /// <summary>
    /// 自分のレーン番号(外部から設定する)
    /// </summary>
    public int laneNum { get; set; }


    protected virtual void Start()
    {
        _noteSpeed = GManager.instance.notesSpeed * 2; //ノーツスピード

        _isHit = false;

        //NotesManagerを取得
        _notesManager = GameObject.Find("NotesManager").GetComponent<NotesManager>();

        //GameFlowManagerの取得
        _gameFlowManager = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();

        //PlayerHpの取得
        _playerHp = GameObject.Find("PlayerHp").GetComponent<PlayerHp>();

        //ScoreManagerの取得
        _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        //ComboManagerの取得
        _comboManager = GameObject.Find("ComboManager").GetComponent<ComboManager>();

        //ComboTextUIの取得
        _comboTextUI = GameObject.Find("ComboTextUI").GetComponent<ComboTextUI>();

        _player = GameObject.FindWithTag("Player");

    }

    /// <summary>
    /// ゲーム未開始、ポーズ時のフローをチェックして移動を行う
    /// </summary>
    protected virtual void Update()
    {
        //ゲーム開始していない場合は処理をしない
        if (!GManager.instance.isStart) return;

        MoveNote();
    }

    /// <summary>
    /// ノーツの移動
    /// </summary>
    protected virtual void MoveNote()
    {
        //ポーズ中は動かない
        if (GManager.instance.isPause) return;

        this.transform.Translate(0, 0, -_noteSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// ノーツがヒットされたときの基本処理
    /// </summary>
    public virtual void OnHit()
    {
        if (_isHit) return; //二重ヒット防止
        _isHit = true;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 共通のエフェクト再生メソッド
    /// </summary>
    /// <param name="type">再生するエフェクトの種類</param>
    protected virtual void PlayEffect(ParticleType type)
    {
        //ParticleManagerが存在しない場合はなにもしない
        if (ParticleManager.instance == null) return;

        //プレイヤー位置の少し上
        Vector3 effectPos = this._player.transform.position
                           + new Vector3(0.0f,0.36f,0.0f); //中心

        Quaternion rot = Quaternion.identity;

        ParticleManager.instance.PlayParticle(type, effectPos, rot);
    }


    /// <summary>
    /// 共通の当たり判定トリガー
    /// </summary>
    /// <param name="other">当たったコライダー</param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if(other.gameObject.CompareTag("Player"))
        {
            OnCollideWithPlayer(other);
        }
    }

    /// <summary>
    /// プレイヤーと衝突した時の挙動(派生クラスでオーバーライドする)
    /// </summary>
    /// <param name="other">プレイヤーのコライダー</param>
    protected virtual void OnCollideWithPlayer(Collider other)
    {
        //派生クラスで書き換える
    }
}
