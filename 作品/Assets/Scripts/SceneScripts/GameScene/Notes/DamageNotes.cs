using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージノーツ
/// </summary>
public class DamageNotes : NotesBase
{
    protected override void MoveNote()
    {
        base.MoveNote();
    }

    public override void OnHit()
    {
        if (_isHit) return; //二重ヒットしない

        _isHit = true;

        Debug.Log("ダメージ");

        //20ダメージ
        _gameFlowManager.OnHit(20);

        //コンボ数をリセット
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

