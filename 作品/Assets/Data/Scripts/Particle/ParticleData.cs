using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//パーティクル(エフェクト)の種類
public enum ParticleType
{
    HIT_EFFECT,
    COIN_EFFECT,
    BREAK_EFFECT
}

[CreateAssetMenu(fileName = "ParticleData", menuName = "Particle/ParticleDataを作成")]

public class ParticleData : ScriptableObject
{
    public ParticleType type;
    public GameObject particlePrefab;
    
}
