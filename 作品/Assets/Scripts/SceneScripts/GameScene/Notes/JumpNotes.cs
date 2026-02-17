using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ジャンプノーツ
/// </summary>
public class JumpNotes : NotesBase
{
    protected override void MoveNote()
    {
        base.MoveNote();
    }

    public override void OnHit()
    {
        if (_isHit) return; //二重ヒットしない

        _isHit = true;

        Debug.Log("ジャンプノーツに当たった");


        //20ダメージ
        _gameFlowManager.OnHit(20);

        if (_comboManager != null)
        {
            _comboManager.ResetCombo();
            _comboTextUI.UpdateComboUI();
        }

        //エフェクトの再生
        PlayEffect(ParticleType.BREAK_EFFECT);

        base.OnHit();
    }

    protected override void OnCollideWithPlayer(Collider other)
    {
        OnHit();
    }
}
