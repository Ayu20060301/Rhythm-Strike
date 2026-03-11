using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class JudgeCore
{
    //---”»’è‚Ì‹–—eŽžŠÔ---
    private const float _criticalTime = 0.025f;
    private const float _hitTime = 0.05f;
    private const float _attackTime = 0.08f;

    /// <summary>
    /// ”»’è‚ÌŽæ“¾
    /// </summary>
    /// <param name="timeLag">”»’èŽžŠÔ‚Ì‚¸‚ê</param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public JudgeType GetJudge(float timeLag,float offset)
    {
        float adjust = offset * 0.01f;

        if (timeLag <= _criticalTime + adjust) return JudgeType.CRITICAL;
        if (timeLag <= _hitTime + adjust) return JudgeType.HIT;
        if (timeLag <= _attackTime + adjust) return JudgeType.ATTACK;

        return JudgeType.MISS;
    }
}
