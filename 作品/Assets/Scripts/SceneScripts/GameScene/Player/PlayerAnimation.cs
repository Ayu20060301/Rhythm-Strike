using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField]
    private Animator _playerAnim;

    //攻撃
    public void Attack()
    {
        _playerAnim.SetTrigger("Attack1Trigger");
    }


    //走行
    public void RunAnimation()
    {
        _playerAnim.SetTrigger("RunTrigger");
    }

    //ダメージ
    public void DamageAnimation()
    {
        _playerAnim.SetTrigger("DamageTrigger");
    }

    public void MoveTurnAnimation()
    {
        StartCoroutine(SmileCoroutine());
        _playerAnim.SetTrigger("TurnTrigger");
    }

    private IEnumerator SmileCoroutine()
    {
        _playerAnim.SetBool("isSmileFace", true);

        yield return new WaitForSeconds(0.5f);

        _playerAnim.SetBool("isSmileFace", false);

    }

    //クリア時のアニメーション
    public void ClearPlayerAnimation()
    {
        _playerAnim.SetBool("isSmileFace", true);
        _playerAnim.SetTrigger("ClearTrigger");
    }

    //クリア失敗時のアニメーション
    public void FailedPlayerAnimation()
    {
        _playerAnim.SetTrigger("SadFaceTrigger");
        _playerAnim.SetTrigger("FailedTrigger");
    }

    //ジャンプ

    public void Jump()
    {
        _playerAnim.SetTrigger("JumpTrigger");
    }

    //死ぬ
    public void Death()
    {
        _playerAnim.SetTrigger("DeathFaceTrigger");
        _playerAnim.SetTrigger("DeathTrigger");
    }


}
