using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private PlayerAnimation _playerAnimation;

    [SerializeField]
    private Transform _modelTransform;

    private int _playerDataIndex = 0; //DataBaseManagerから取得するインデックス
    private int _laneCount = 5; //レーン数
    private float _laneWidth = 1.0f;

    //---プレイヤー能力
    private float _moveSpeed = 8.0f; //移動速度
    private float _jumpPower = 3.0f; //ジャンプ力
  
    //---状態---
    private bool  _isGround = true; //地面にいるかどうか
    private int _currentLane = 3; //現在のレーン

    private float _rollDuration = 0.6f; //回転にかかる時間

    private bool _isRolling = false;

    private Coroutine _rollCoroutine; //プレイヤー回転用のコルーチン
    private float _currentAngle = 0.0f; //現在のY回転角

    //---Unityコーポネント & 補助変数
    private  Rigidbody _rb;
    private Vector3 _targetPosition; //目標位置
    private bool _jumpRequested = false; //ジャンプ要求フラグ


    private KeyCode[] _attackKeys =
    {
        KeyCode.D,
        KeyCode.F,
        KeyCode.J,
        KeyCode.K,
    };

    //移動方向の種類
    private enum MoveDirection
    {
        LEFT = -1, //左
        RIGHT = 1 //右
    }

    //初期化処理
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        //レーン値が妥当な範囲にあるかチェックして正規化
        _currentLane = Mathf.Clamp(_currentLane, 1, Mathf.Max(1, _laneCount));

        //初期ターゲット位置を設定
        _targetPosition = this.transform.position;
    }

    //毎フレームの更新処理
    // Update is called once per frame
    void Update()
    {
      

        //フェード中、ポーズ中または死亡時は入力を無視
        if (GManager.instance.isFading || GManager.instance.isPause || GManager.instance.isDeath) return;

        HandleLaneInput();
        HandleJumpInput();

        float targetX = CalculateLaneCenterX(_currentLane);
        _targetPosition = new Vector3(targetX, this.transform.position.y, this.transform.position.z);

    }

    
    /// <summary>
    /// 物理移動
    /// </summary>
    private void FixedUpdate()
    {
        if (_rb == null) return;

        //現在のRigidBodyの高さ(Y)と前後(Z)を維持しつつ、Xを目標にスムーズに合わせる
        Vector3 currentPos = _rb.position;
        Vector3 desiredPos = new Vector3(_targetPosition.x, currentPos.y, currentPos.z);
        Vector3 newPos = Vector3.Lerp(currentPos, desiredPos, Time.fixedDeltaTime * _moveSpeed);

        _rb.MovePosition(newPos);

        //ジャンプ
        if(_jumpRequested && _isGround)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);


            _playerAnimation.Jump();
           

            if(SoundEffectManager.instance != null)
            {
                SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.JUMP);
            }

            _isGround = false; //空中状態にする

        }

        //リクエストは一度だけ処理する
        _jumpRequested = false;
    }


  public void SetPlayerIndex(int index)
    {
        _playerDataIndex = index;
    }


    /// <summary>
    /// レーン切り替えの入力処理
    /// </summary>
    private void HandleLaneInput()
    {
        //左移動
        if(Input.GetKeyDown(KeyCode.S))
        { 
            if (_currentLane > 1)
            {
                _currentLane = Mathf.Max(1, _currentLane - 1);
                UpdateTargetXForLane();
                StartRoll(MoveDirection.LEFT); //左方向に1回転
            }
        }

        //右移動
        if(Input.GetKeyDown(KeyCode.L))
        {
            if (_currentLane < _laneCount)
            {
                _currentLane = Mathf.Min(_laneCount, _currentLane + 1);
                UpdateTargetXForLane();
                StartRoll(MoveDirection.RIGHT); //右方向に1回転
            }
        }

       //攻撃
       foreach(var key in _attackKeys)
        {
            if(Input.GetKeyDown(key))
            {
                TryAttack();
                break;
            }
        }
    }

    //地上のみ攻撃可
    private void TryAttack()
    {
        if (!_isGround) return;
        if (GManager.instance.isDeath) return;

        _playerAnimation.Attack();
    }

    /// <summary>
    /// ジャンプ入力の受付
    /// </summary>
    private void HandleJumpInput()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            _jumpRequested = true;
        }
    }

    /// <summary>
    /// 現在のレーンに基づいてtargetPositionのXを更新する
    /// </summary>
    private void UpdateTargetXForLane()
    {
        float x = CalculateLaneCenterX(_currentLane);
        _targetPosition = new Vector3(x, _targetPosition.y, _targetPosition.z);
    }

    /// <summary>
    /// レーン番号からX座標を算出する。レーン中央をx = 0にする
    /// </summary>
    /// <param name="lane">レーン番号</param>
    /// <returns></returns>
    private float CalculateLaneCenterX(int lane)
    {
        float centerOffset = (_laneCount + 1) / 2.0f;
        return (lane - centerOffset) * _laneWidth;
    }

    private void StartRoll(MoveDirection dir)
    {

        float tiltAngle = 60.0f * (int)dir; //一度傾かせる
        float returnAngle = 360.0f * (int)dir; //同方向に一周して戻る
       
        //回転中なら一旦止める
        if(_rollCoroutine != null)
        {
            StopCoroutine(_rollCoroutine);
            _rollCoroutine = null;
        }

        //キー押下直後に傾ける
        _currentAngle += tiltAngle;
        this._modelTransform.localRotation = Quaternion.Euler(0.0f, _currentAngle, 0.0f);

        //残りの回転だけコルーチンで処理
        _rollCoroutine = StartCoroutine(RollAndReturn(returnAngle));
    }

    //回して元に戻すコルーチン
    private IEnumerator RollAndReturn(float returnAngle)
    {
        _isRolling = true;
        _playerAnimation.MoveTurnAnimation();

        float startAngle = _currentAngle;
        float targetAngle = startAngle + returnAngle;
        float elapsed = 0.0f;

        while (elapsed < _rollDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _rollDuration);
            _currentAngle = Mathf.Lerp(startAngle, targetAngle, t);
            this._modelTransform.localRotation = Quaternion.Euler(0.0f, _currentAngle, 0.0f);
            yield return null;
        }

        _currentAngle = Mathf.Round(_currentAngle / 360.0f) * 360.0f;
        this._modelTransform.localRotation = Quaternion.Euler(0.0f, _currentAngle, 0.0f);

        _isRolling = false;
        _rollCoroutine = null;
    }

   


    //接地判定
    void OnCollisionEnter(Collision collision)
    {
        //地面の判定
        if(collision.gameObject.CompareTag("Base"))
        {
            _isGround = true;
        }
    }

    //地面から離れたら_isGround = falseにする
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            _isGround = false;
        }
    }
}
