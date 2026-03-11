using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// コインノーツ
/// </summary>
public class CoinNotes : NotesBase
{
    private float _rotSpeed = 180.0f; //1秒で180度回転

    protected override void MoveNote()
    {
        base.MoveNote();

        //回転
        this.transform.Rotate(0.0f, _rotSpeed * Time.deltaTime, 0.0f);
    }

    public override void OnHit()
    {
        if (_isHit) return;
        _isHit = true;

        //コイン取得処理
        Debug.Log("コインをゲット"); //デバッグ用ログ

        if (_comboManager != null)
        {
            //コンボ追加
            _comboManager.AddCombo();
            _comboTextUI.UpdateComboUI();
        }

        //スコア処理

        if (_scoreManager != null)
        {
            _scoreManager.OnCoin();
            _scoreManager.UpdateScore();
        }

        //HPが10回復
        _playerHp.Heal(10);


        //効果音の再生
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.COIN);

        //エフェクトの再生
        base.PlayEffect(ParticleType.COIN_EFFECT);

        base.OnHit();
    }

 
    protected override void OnCollideWithPlayer(Collider other)
    {
        OnHit();
    }
}
